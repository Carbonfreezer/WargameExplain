using WargameExplainer.Strategy;

namespace WargameExplainer.SampleGames.CoopGame;

/// <summary>
///     This is a simple game to test non-zero sum capabilities. It is played with three players.
///     All players get one coin in the beginning. Every player can decide to hand back his coin. All coins handed back
///     are doubled and equally distributed amongst player. The nash equilibrium is, that nobody hands back his coin.
///     Additionally players can vote upfront, if collaboration should be enforced. This is only done, if all players vote for it. 
/// </summary>
public class CoopGamestate : IHashableGameState, IManipulator, IEvaluator
{
    /// <summary>
    ///     The number of players we have.
    /// </summary>
    public const int NumOfPlayers = 3;

    /// <summary>
    ///     The player who is nect to decide.
    /// </summary>
    public int m_currentPlayer;

    /// <summary>
    ///     Flags whether we are currently voting.
    /// </summary>
    public bool m_isVoting;

    /// <summary>
    ///     The flag whether a player returned the money.
    /// </summary>
    public bool[] m_moneyReturned = new bool[NumOfPlayers];

    /// <summary>
    ///     The amount of votes we have collected.
    /// </summary>
    public int m_votesCollected = 0;

    /// <summary>
    ///     Voting means we do an upfront voting and if it is uniformly agreed upon everybody has to return his money.
    /// </summary>
    /// <param name="hasVoting">Do we do a voting upfront.</param>
    public CoopGamestate(bool hasVoting)
    {
        m_isVoting = hasVoting;
    }


    /// <inheritdoc />
    public int NumOfPlayerPerspectives => NumOfPlayers;

    public bool IsGameOverAndEvaluate(in Span<float> afterStateValues)
    {
        bool isGameOver = (m_currentPlayer == NumOfPlayers) && (!m_isVoting);

        if (!isGameOver)
        {
            for (int i = 0; i < NumOfPlayers; ++i)
                afterStateValues[i]=0.0f;
        }
        else
        {
            int returnedMoney = m_moneyReturned.Count(x => x);
            float moneyAdded = returnedMoney * 2.0f / NumOfPlayers;

            for (int i = 0; i < NumOfPlayers; ++i)
                if (m_moneyReturned[i])
                    afterStateValues[i] = moneyAdded;
                else
                    afterStateValues[i] = (1.0f + moneyAdded);
        }

        return isGameOver;
    }

  
    /// <inheritdoc />
    public bool IsCurrentlyHashable => false;

 
    /// <inheritdoc />
    public void AppendData(BinaryWriter writer)
    {
    }


    /// <inheritdoc />
    public bool IsRandomManipulator => false;


    public IList<ICommand> GetMoveOptionsWithProbabilities(in Span<float> probabilities)
    {
        List<ICommand> result = new List<ICommand>(2);
        if (m_isVoting)
            result.AddRange([new VotingCommand(this, false), new VotingCommand(this, true)]);
        else if (m_votesCollected == NumOfPlayers)
            result.Add(new ReturnCommand(this, true));
        else
            result.AddRange([new ReturnCommand(this, true), new ReturnCommand(this, false)]);

        return result;
    }

    /// <inheritdoc />
    public int PlayerPerspective => m_currentPlayer;

    public int MaxNumOfProbabilityChoices => 0;


    /// <summary>
    /// Executes the game.
    /// </summary>
    public void Run()
    { 
        using StrategicDecider strategicDecider = new StrategicDecider(this, 20, 300, this, this);
        Span<float> values = stackalloc float[NumOfPlayerPerspectives];

        
        while (!IsGameOverAndEvaluate(in values))
        {
            strategicDecider.ExecuteNewMove();
        }


        Console.WriteLine($"Number of votes : {m_votesCollected}");

        for (int i = 0; i < NumOfPlayers; ++i)
            Console.WriteLine($"Social behaviour of player {i} :  {m_moneyReturned[i]} Value: {values[i]}");
    }

}