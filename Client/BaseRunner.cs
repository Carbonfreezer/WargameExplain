using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using WargameExplainer.Explanation;
using WargameExplainer.TrainingSystem;
using WargameExplainer.Utils;

namespace Client;

/// <summary>
///     The client system that can provide information for the server.
/// </summary>
public class BaseRunner
{
    /// <summary>
    ///     The game outcome classifier.
    /// </summary>
    private readonly IGameOutcomeClassifier m_classifier;

    /// <summary>
    ///     The episodic generator to generate the episodes.
    /// </summary>
    private readonly EpisodicGenerator m_episodicGenerator;

    /// <summary>
    ///     The lstener over which we communicate with the central servers.
    /// </summary>
    private readonly TcpListener m_listener;

    /// <summary>
    ///     The info provider for the training.
    /// </summary>
    private readonly TrainingInfoProvider m_trainingProvider;

    /// <summary>
    ///     Flag to see, if we want to continue broadcasting udp packages.
    /// </summary>
    private bool m_wantsToBroadCastUdpPackages;


    /// <summary>
    ///     Constructor
    /// </summary>
    public BaseRunner()
    {
        m_trainingProvider = GlobalNetworkConstants.GenerateInfoProvider();
        m_episodicGenerator = new EpisodicGenerator(m_trainingProvider);
        m_listener = new TcpListener(IPAddress.Any, 0);
        m_classifier = m_trainingProvider.GetGameOutcomeClassifier();
    }


    /// <summary>
    ///     Run call starts the program and the listener waits for commands from the server. Once the server has done the job,
    ///     the client also terminates.
    /// </summary>
    public void Run()
    {
        m_listener.Start();

        m_wantsToBroadCastUdpPackages = true;
        Thread broadCaster = new Thread(UdpBroadCaster);
        broadCaster.Name = "Broadcaster";
        broadCaster.Start();

        using TcpClient client = m_listener.AcceptTcpClient();
        m_wantsToBroadCastUdpPackages = false;
        using NetworkStream stream = client.GetStream();
        using BinaryReader reader = new BinaryReader(stream);
        using BinaryWriter writer = new BinaryWriter(stream);
        try
        {
            while (true)
            {
                int command = reader.ReadInt32();
                switch (command)
                {
                    case 0:
                        ProcessTrainingCommand(writer);
                        break;
                    case 1:
                        ReadLinearModel(reader);
                        break;
                    case 2:
                        GenerateGameoutcome(writer);
                        break;
                    default:
                        Debug.Assert(false, "Unimplemented case.");
                        break;
                }
            }
        }
        // Happens, when the other side has closed the command.
        catch (EndOfStreamException)
        {
        }
    }


    /// <summary>
    ///     Plays a game and sets the result over to the server.
    /// </summary>
    /// <param name="writer">Writer to write the result to.</param>
    private void GenerateGameoutcome(BinaryWriter writer)
    {
        float[] evaluation = m_episodicGenerator.GenerateResult();
        int result = m_classifier.GetCategory(evaluation);
        writer.Write(result);
    }

    /// <summary>
    ///     Reads in the linear model from the stream.
    /// </summary>
    /// <param name="reader">Reader to read model from.</param>
    private void ReadLinearModel(BinaryReader reader)
    {
        LinearModel model = new LinearModel();
        model.ReadFromStream(reader);
        m_trainingProvider.LinearModel = model;
    }

    /// <summary>
    ///     Processes the training command for episode generation.
    /// </summary>
    /// <param name="writer">The writer to write the result to.</param>
    private void ProcessTrainingCommand(BinaryWriter writer)
    {
        EpisodicRecord result = m_episodicGenerator.GenerateEpisode(m_trainingProvider.EpsilonTraining);
        result.WriteNetworkData(writer);
    }


    /// <summary>
    ///     Udp broadcasting routine to find a servers.
    /// </summary>
    private void UdpBroadCaster()
    {
        using UdpClient udpClient = new UdpClient { EnableBroadcast = true, MulticastLoopback = false };

        int portNumber = ((IPEndPoint)m_listener.LocalEndpoint).Port;
        byte[] sendBytes =
            Encoding.ASCII.GetBytes(portNumber.ToString());

        IPEndPoint destination = new IPEndPoint(IPAddress.Parse(GlobalNetworkConstants.ServerAddress), GlobalNetworkConstants.DiscoveryHostPort);

        while (m_wantsToBroadCastUdpPackages)
        {
            udpClient.Send(sendBytes, destination);
            Thread.Sleep(2000);
        }
    }
}