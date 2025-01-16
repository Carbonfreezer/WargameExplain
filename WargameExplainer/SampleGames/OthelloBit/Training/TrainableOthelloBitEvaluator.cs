using WargameExplainer.TrainingSystem;

namespace WargameExplainer.SampleGames.OthelloBit.Training;


/// <summary>
/// The trainable evaluator for the othello game. Only uses stone balance as heuristic.
/// </summary>
public class TrainableOthelloBitEvaluator : TrainableEvaluator
{
    /// <summary>
    ///     Game interface of Othello
    /// </summary>
    private readonly BitBoardState m_gameState;

    /// <summary>
    /// Contains the masking for the corners.
    /// </summary>
    private readonly ulong m_cornerMask;

    /// <summary>
    ///     Initialization.
    /// </summary>
    /// <param name="gameState">Olettho game interface</param>
    public TrainableOthelloBitEvaluator(TrainingProviderOthelloBit provider, BitBoardState gameState) : base(provider,
        gameState)
    {
        m_gameState = gameState;
        m_cornerMask = BitBoardState.GetBitMaskForField(0 , 0 ) |
                       BitBoardState.GetBitMaskForField(7, 0 ) |
                       BitBoardState.GetBitMaskForField(0 , 7 ) |
                       BitBoardState.GetBitMaskForField(7 , 7 );
    }

    /// <summary>
    ///     Number of players.
    /// </summary>
    public override int NumOfPlayerPerspectives => 2;

    protected override bool InternallyReevaluate(in Span<float> afterStateValues)
    {
        bool isGameOver = m_gameState.IsGameOver();
        (int numBlackStones, int numWhiteStones) = m_gameState.GetStoneBalance();

        if (isGameOver)
        {
            if (numBlackStones > numWhiteStones)
            {
                afterStateValues[0] = 1f;
                afterStateValues[1] = -1f;
            }
            else if (numBlackStones < numWhiteStones)
            {
                afterStateValues[0] = -1f;
                afterStateValues[1] = 1f;
            }
            else
            {
                afterStateValues[0] = 0f;
                afterStateValues[1] = 0f;
            }

            return true;
        }

        int blackAdvantageCorner = m_gameState.GetStoneCountForMask(true, m_cornerMask) -
                                   m_gameState.GetStoneCountForMask(false, m_cornerMask);

        int stoneAdvantage = numBlackStones - numWhiteStones;

        int blackMoves = m_gameState.GetMobility(true);
        int whiteMoves = m_gameState.GetMobility(false);

        afterStateValues[0] = (blackAdvantageCorner / 4.0f) * 0.6f + stoneAdvantage / 64.0f * 0.1f + 0.3f * ((float)(blackMoves - whiteMoves) / (blackMoves + whiteMoves));
        afterStateValues[1] = -afterStateValues[0];

        return false;
    }
}
