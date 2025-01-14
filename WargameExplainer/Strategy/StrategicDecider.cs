using System.Diagnostics;
using WargameExplainer.TrainingSystem;

namespace WargameExplainer.Strategy;

/// <summary>
///     Implements the expect minimax decider.
/// </summary>
public class StrategicDecider : IDisposable
{
    /// <summary>
    ///     The evaluator to evaluate the game state.
    /// </summary>
    private readonly IEvaluator m_evaluator;

    /// <summary>
    ///     The game state we operate on.
    /// </summary>
    private readonly IHashableGameState m_hashableGameState;

    /// <summary>
    ///     The hashmap we use.
    /// </summary>
    private readonly HashLookUp m_hashMap;

    /// <summary>
    ///     Random number generator.
    /// </summary>
    private readonly Random m_rnd = new();

    /// <summary>
    /// The depth wioth which we search.
    /// </summary>
    private readonly int m_searchDepth;


    /// <summary>
    ///     Generates the strategy
    /// </summary>
    /// <param name="hashableGameState">The hashable game state.</param>
    /// <param name="hashGranularity">Describes the granularity that should be used in the internal hashmap.</param>
    /// <param name="searchDepth">The depth with which we would like to search.</param>
    /// <param name="evaluator">The evaluator to determine the afterstate value.</param>
    /// <param name="manipulator">The manipulator to execute the moves.</param>
    public StrategicDecider(
        IHashableGameState hashableGameState,
        int hashGranularity,
        int searchDepth,
        IEvaluator evaluator,
        IManipulator manipulator)
    {
        m_hashMap = new HashLookUp(hashGranularity);
        m_hashableGameState = hashableGameState;
        m_evaluator = evaluator;
        Manipulator = manipulator;
        m_searchDepth = searchDepth;
    }

    /// <summary>
    ///     Asks for the number of player perspectives. Important for reserving enough memory in the span for asking for the
    ///     afterstate value.
    /// </summary>
    public int NumOfPlayerPerspectives => m_evaluator.NumOfPlayerPerspectives;


    /// <summary>
    ///     Asks for the current hashable game state, used for evaluation in episodic generation.
    /// </summary>
    public IHashableGameState GameState => m_hashableGameState;

    /// <summary>
    /// The manipulator is readable from the outside. This is relevant for  the sparrings partner.
    /// </summary>
    public IManipulator Manipulator { get; }

    /// <summary>
    /// The trainings partner if existant.
    /// </summary>
    public SparringsPartner? TrainingsPartner { get; set; } = null; 


    /// <summary>
    ///     Disposes the hash map.
    /// </summary>
    public void Dispose()
    {
        m_hashMap.Dispose();
        TrainingsPartner?.Dispose();
    }


    /// <summary>
    ///     Checks from the outside if we have a game over situation and returns the afterstate value.
    /// </summary>
    /// <param name="afterStateValue">The aftertstate value we have.</param>
    /// <returns>If the game is over.</returns>
    public bool IsGameOverAndAfterStateValue(in Span<float> afterStateValue)
    {
        return m_evaluator.IsGameOverAndEvaluate(afterStateValue);
    }


    /// <summary>
    ///     Gets a random selection from a weight array.
    /// </summary>
    /// <param name="weightArray">Array of probabilities.</param>
    /// <returns>Selected option</returns>
    public int GetRandomChoiceIndex(Span<float> weightArray)
    {
        // Trick to make all reproducable.
        // return 0;

        float testValue = m_rnd.NextSingle();
        float acc = 0.0f;
        for (int i = 0; i < weightArray.Length; ++i)
        {
            acc += weightArray[i];
            if (acc > testValue)
                return i;
        }

        Debug.Assert(false, "Should never happen.");
        return -1;
    }


    /// <summary>
    ///     Does a complete move on the current situation.
    /// </summary>
    /// <returns>The command that was executed.</returns>
    public ICommand ExecuteNewMove()
    {
        if ((TrainingsPartner != null) && (TrainingsPartner.IsSparring))
            return TrainingsPartner.ExecuteSparringMove();

        Span<float> randArray = stackalloc float[Manipulator.MaxNumOfProbabilityChoices];
        IList<ICommand> actions = Manipulator.GetMoveOptionsWithProbabilities(in randArray);
        ICommand decision;

        // Check for the random case.
        if (Manipulator.IsRandomManipulator)
        {
            decision = actions[GetRandomChoiceIndex(randArray)];
            decision.Execute();
            return decision;
        }

        // We do not need to calculate if we only have one choice anyway.
        if (actions.Count == 1)
        {
            decision = actions[0];
            decision.Execute();
            return decision;
        }
#if DEBUG
        ulong testBefore = m_hashMap.GetHashCode(m_hashableGameState);
#endif

        decision = GetSmartMove();

#if DEBUG
        ulong testAfter = m_hashMap.GetHashCode(m_hashableGameState);
        Debug.Assert(testBefore == testAfter, "Mismatch in Command / Undo operations.");
#endif

        decision.Execute();
        return decision;
    }


    /// <summary>
    ///     Makes an epsilon greedy move.
    /// </summary>
    /// <param name="epsilon">The epsilon factor in epsilon greedy strategy.</param>
    public ICommand ExecuteMoveEpsilonGreedy(float epsilon)
    {
        if ((!Manipulator.IsRandomManipulator) && (m_rnd.NextSingle() < epsilon))
        {
            // In this case we do a random choice.
            IList<ICommand> actions = Manipulator.GetMoveOptionsWithProbabilities(Span<float>.Empty);
            int choice = (int)(m_rnd.NextSingle() * actions.Count);
            actions[choice].Execute();
            return actions[choice];
        }

        return ExecuteNewMove();
    }


    /// <summary>
    ///     Gets the best move for the corresponding player and the corresponding evaluation.
    ///     If this is a random move or a terminal game state the returned move is -1.
    /// </summary>
    /// <param name="remainingDepth">The amount of levels we still have to evaluate.</param>
    /// <param name="momentaryEvaluation">Span that gets filled with values for the afterstate.</param>
    /// <returns>Returns move index.</returns>
    private int GetMomentaryEvaluation(int remainingDepth, in Span<float> momentaryEvaluation)
    {
        int totalPerspectives = m_evaluator.NumOfPlayerPerspectives;
        bool isGameOver = m_evaluator.IsGameOverAndEvaluate(momentaryEvaluation);
        if ((isGameOver) || (remainingDepth == 0))
            return -1;

        bool wantsHashing = m_hashableGameState.IsCurrentlyHashable;
        ulong hashCode = 0;
        if (wantsHashing)
        {
            hashCode = m_hashMap.GetHashCode(m_hashableGameState);
            if (m_hashMap.TryGet(hashCode, momentaryEvaluation))
                return - 1;
        }

        // Here we need to choose. Traverse over all the children.
        Span<float> probs = stackalloc float[Manipulator.MaxNumOfProbabilityChoices];
        IList<ICommand> moves = Manipulator.GetMoveOptionsWithProbabilities(in probs);
        int numOfMoves = moves.Count;

        // Copy the data to avoid recalculation.
        bool isRandom = Manipulator.IsRandomManipulator;
        int playerPerspective = Manipulator.PlayerPerspective;

        Span<float> flatValueArray = stackalloc float[numOfMoves * NumOfPlayerPerspectives];

        for (int i = 0; i < numOfMoves; ++i)
        {
            moves[i].Execute();
            GetMomentaryEvaluation(remainingDepth - 1,
                flatValueArray.Slice(i * NumOfPlayerPerspectives, NumOfPlayerPerspectives));
            moves[i].Undo();
        }

        int bestMove = -1;
        // In the case of random, we return the weighted value vectors.
        if (isRandom)
        {
            for (int perspective = 0; perspective < totalPerspectives; ++perspective)
            {
                momentaryEvaluation[perspective] = 0.0f;
                for (int move = 0; move < numOfMoves; ++move)
                    momentaryEvaluation[perspective] +=
                        flatValueArray[move * NumOfPlayerPerspectives + perspective] * probs[move];
            }

        }
        else
        {
            // In this case we pick the best move from the players perspective.
            float bestValue = float.NegativeInfinity;
            for (int move = 0; move < numOfMoves; ++move)
                // Add a bit of noise to not make always the same move on evaluation tie.
                if (flatValueArray[move * NumOfPlayerPerspectives + playerPerspective] > bestValue + (m_rnd.NextSingle() - 0.5f) * 0.00001f)
                {
                    bestValue = flatValueArray[move * NumOfPlayerPerspectives + playerPerspective];
                    bestMove = move;
                }

            // Now we copy over the result to the destination
            for (int i = 0; i < NumOfPlayerPerspectives; ++i)
                momentaryEvaluation[i] = flatValueArray[bestMove * NumOfPlayerPerspectives + i];

        }

        if (wantsHashing)
            m_hashMap.InsertIntoHashmap(hashCode, momentaryEvaluation);

        return bestMove;
    }

    /// <summary>
    ///     Searches for a move according to expect minimax strategy.
    /// </summary>
    /// <returns>The command with which we search.</returns>
    private ICommand GetSmartMove()
    {
        // Helper for calibrating the size of the hashmap.
        // Console.WriteLine($"Occupancy: {m_hashMap.AverageFilling}");
        m_hashMap.Flush();

        Span<float> dummy = stackalloc float[NumOfPlayerPerspectives];
        int move = GetMomentaryEvaluation(m_searchDepth, dummy);

        Debug.Assert(move != -1, "We should have a valid move here.");
        IList<ICommand> moves = Manipulator.GetMoveOptionsWithProbabilities(Span<float>.Empty);
        return moves[move];
    }
}