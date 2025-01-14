using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using WargameExplainer.Explanation;
using WargameExplainer.TrainingSystem;
using WargameExplainer.Utils;

namespace Server;

/// <summary>
///     Creates the base runner as a server, it can be used in two modes.
///     If it wants training it dispatches calls to clients to generate episodes and collects them and saves the model.
///     Otherwise it loads the model and performs an evaluation over several runs on the clients.
/// </summary>
/// <param name="modelFile">The model file to generate in training and to load in evaluation.</param>
/// <param name="wantsTraining">Flags if we are in training or evaluation mode.</param>
public class BaseRunner(bool wantsTraining, string? modelFile)
{
    /// <summary>
    ///     The already connected elements to filter out doubles.
    /// </summary>
    private readonly List<IPEndPoint> m_alreadyConnected = new List<IPEndPoint>();

    /// <summary>
    ///     The lists with the threads that serve the clients.
    /// </summary>
    private readonly List<Thread> m_clientThreads = new List<Thread>(10);

    /// <summary>
    ///     The list with the episodic records we use.
    /// </summary>
    private readonly List<EpisodicRecord> m_episodicList = new List<EpisodicRecord>();

    /// <summary>
    ///     The linear model used for the non training case.
    /// </summary>
    private  LinearModel? m_evaluationModel = null;


    /// <summary>
    ///     The auto reset event for the main thread to signal, that analysis / episode generation is over.
    /// </summary>
    private readonly AutoResetEvent m_finishAnalysis = new AutoResetEvent(false);

    /// <summary>
    ///     The list with the game outcomes used for the collection for the non training case.
    /// </summary>
    private readonly List<int> m_gameOutComes = new List<int>();

    /// <summary>
    ///     The lock object for the threads.
    /// </summary>
    private readonly object m_taskCounterLock = new object();

    /// <summary>
    ///     The lock of threads for result generation.
    /// </summary>
    private readonly object m_taskResultLock = new object();

    /// <summary>
    ///     The training provider that has to be adjusted for every game.
    /// </summary>
    private readonly TrainingInfoProvider m_trainingProvider = GlobalNetworkConstants.GenerateInfoProvider();

    /// <summary>
    ///     The thread for getting client requests.
    /// </summary>
    private Thread? m_dispatcherThread;

    /// <summary>
    ///     The number of tasks left for dispatching.
    /// </summary>
    private int m_numOfTasksToDispatch;

    /// <summary>
    ///     The number of tasks that need finising.
    /// </summary>
    private int m_numOfTasksToFinish;

    /// <summary>
    ///     Indicates that we search for clients.
    /// </summary>
    private bool m_searchingForClients = true;

    /// <summary>
    ///     The udp client to receive requests from.
    /// </summary>
    private UdpClient? m_udpClient;

    /// <summary>
    ///     Executes an analysis over several clients with the indicated batch size and the epsilon value in
    ///     epsilon greedy evaluation. The result will be saved in model file.
    /// </summary>
    public void Run()
    {
        if ((!wantsTraining) && (modelFile != null))
        {
            m_evaluationModel = new LinearModel();
            m_evaluationModel.LoadFromFile(modelFile);
        }

        m_numOfTasksToDispatch = m_trainingProvider.BatchSize;
        m_numOfTasksToFinish = m_trainingProvider.BatchSize;

        m_udpClient = new UdpClient(GlobalNetworkConstants.DiscoveryHostPort)
            { EnableBroadcast = true, MulticastLoopback = false };
        m_udpClient.Client.ReceiveTimeout = 1000;
        m_dispatcherThread = new Thread(DispatchClients);
        m_dispatcherThread.Name = "Dispatcher";
        m_dispatcherThread.Start();


        // Wait for all to get finished.
        m_finishAnalysis.WaitOne();
        m_searchingForClients = false;


        if ((wantsTraining) && (modelFile != null))
            // Generate model and we are done.
            m_trainingProvider.GenerateAndSaveModel(m_episodicList, modelFile);
        else
            LogGameOutcome();
    }

    /// <summary>
    ///     Generates the dump with the game outcome.
    /// </summary>
    private void LogGameOutcome()
    {
        IGameOutcomeClassifier interpreter = m_trainingProvider.GetGameOutcomeClassifier();

        // Now we need to compute the outcome results.
        int[] accumulatedResults = Enumerable.Range(0, interpreter.NumOfCategories)
            .Select(cat => m_gameOutComes.Count(i => i == cat)).ToArray();

        Console.WriteLine("============= Statistics result ============");
        for (int i = 0; i < interpreter.NumOfCategories; ++i)
            Console.WriteLine($"{interpreter.GetDescription(i)}: {accumulatedResults[i]} ");

        Console.ReadLine();
    }

    /// <summary>
    ///     Gets invoked as a thread to receive signals from clients and establish a connection.
    /// </summary>
    private void DispatchClients()
    {
        Debug.Assert(m_udpClient != null, "That should have been completed by now.");
        while (m_searchingForClients)
            try
            {
                IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = m_udpClient.Receive(ref endpoint);

                if (m_alreadyConnected.Any(connect => connect.Equals(endpoint)))
                    continue;

                m_alreadyConnected.Add(endpoint);

                int clientPort = int.Parse(Encoding.ASCII.GetString(data));

                IPEndPoint clientAddress = new IPEndPoint(endpoint.Address, clientPort);
                Thread clientManager = new Thread(ManageClient);
                m_clientThreads.Add(clientManager);
                clientManager.Start(clientAddress);
            }
            // Happens because of time out.
            catch (SocketException)
            {
            }
    }


    /// <summary>
    ///     Deals with a client.
    /// </summary>
    /// <param name="obj">The encapsulated TCP address where we should go to.</param>
    private void ManageClient(object? obj)
    {
        Debug.Assert(obj != null, "Address should be set.");
        IPEndPoint endPoint = (IPEndPoint)obj;

        using TcpClient tcpClient = new TcpClient();
        tcpClient.Connect(endPoint);
        using NetworkStream netStream = tcpClient.GetStream();
        using BinaryWriter writer = new BinaryWriter(netStream);
        using BinaryReader reader = new BinaryReader(netStream);

        // For the case we are in evaluation we first send the linear model.
        if ((!wantsTraining) && (m_evaluationModel != null))
        {
            writer.Write(1);
            m_evaluationModel.WriteToStream(writer);
        }

        while (true)
        {
            // Save guard the initialization stuff.
            lock (m_taskCounterLock)
            {
                // Nothing more to do.
                if (m_numOfTasksToDispatch <= 0)
                    return;

                m_numOfTasksToDispatch--;
            }

            if (wantsTraining)
                ProcessTrainingCommand(reader, writer);
            else
                ProcessEvaluationCommand(reader, writer);
        }
    }

    /// <summary>
    ///     Simply does the game evaluation.
    /// </summary>
    /// <param name="reader">Reader to read data from.</param>
    /// <param name="writer">Writer to write data to.</param>
    private void ProcessEvaluationCommand(BinaryReader reader, BinaryWriter writer)
    {
        // Write the command to play a game for evaluation.
        writer.Write(2);

        int gameOutcome = reader.ReadInt32();

        lock (m_taskResultLock)
        {
            m_gameOutComes.Add(gameOutcome);
            m_numOfTasksToFinish--;
            if (m_numOfTasksToFinish == 0)
                m_finishAnalysis.Set();
        }
    }

    /// <summary>
    ///     Processes the training command.
    /// </summary>
    /// <param name="reader">The reader for the TCP stream.</param>
    /// <param name="writer">The writer for the TCP stream.</param>
    private void ProcessTrainingCommand(BinaryReader reader, BinaryWriter writer)
    {
        // Write out the command to read in episode.
        writer.Write(0);

        // Get the record from the client.
        EpisodicRecord record = new EpisodicRecord();
        record.ReadNetworkData(reader);

        lock (m_taskResultLock)
        {
            m_episodicList.Add(record);
            m_numOfTasksToFinish--;
            if (m_numOfTasksToFinish == 0)
                m_finishAnalysis.Set();
        }
    }
}