using WargameExplainer.Strategy;

namespace WargameExplainer.SampleGames.BettingGame;

/// <summary>
///     Very simplistic game where the player can place three different betting types.
///     He can place 10 bets and must have 3 wins in the end.
/// </summary>
public class BettingGameState : IHashableGameState, IManipulator, IEvaluator
{
    /// <summary>
    ///     The chosen bet to place.
    /// </summary>
    public int m_betPlaced;

    /// <summary>
    ///     The betting round we are in.
    /// </summary>
    public int m_bettingRound;

    /// <summary>
    ///     The number of successes we have.
    /// </summary>
    public int m_successesWon;

    /// <summary>
    ///     Flags, that we have placed a bet and are waiting for the random player.
    /// </summary>
    public bool m_waitingForEvaluation;

  
    /// <inheritdoc />
    public int NumOfPlayerPerspectives => 1;

    public bool IsGameOverAndEvaluate(in Span<float> afterStateValues)
    {
        bool isGameOver = (m_bettingRound >= 10) || (m_successesWon >= 3);

        if (m_successesWon >= 3)
            afterStateValues[0]= 1.0f;
        else if (m_bettingRound >= 10)
            afterStateValues[0] = -1.0f;
        else
            afterStateValues[0] = 0.0f;

        return isGameOver;

    }

   
    /// <inheritdoc />
    public bool IsCurrentlyHashable => !m_waitingForEvaluation;

   
    /// <inheritdoc />
    public void AppendData(BinaryWriter writer)
    {
        writer.Write(m_bettingRound);
        writer.Write(m_successesWon);
    }

    
    /// <inheritdoc />
    public bool IsRandomManipulator => m_waitingForEvaluation;


    public IList<ICommand> GetMoveOptionsWithProbabilities(in Span<float> probabilities)
    {
        List<ICommand> result = new List<ICommand>(3);
        // The random part.
        if (m_waitingForEvaluation)
        {
            float successProb = m_betPlaced switch
            {
                0 => 0.1f,
                1 => 0.2f,
                2 => 0.3f,
                _ => 0.0f
            };

            probabilities[0] = successProb;
            probabilities[1] = 1.0f - successProb;
            result.AddRange([new WonCommand(this, true), new WonCommand(this, false)]);
        }
        else
        {
            result.AddRange([new BetCommand(this, 0), new BetCommand(this, 1), new BetCommand(this, 2)]);
        }

        return result;
    }

    /// <inheritdoc />
    public int PlayerPerspective => 0;

    public int MaxNumOfProbabilityChoices => 2;
}