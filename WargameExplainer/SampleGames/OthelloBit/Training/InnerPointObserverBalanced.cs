using WargameExplainer.Strategy;
using WargameExplainer.TrainingSystem;

namespace WargameExplainer.SampleGames.OthelloBit.Training;

/// <summary>
///     Observes the stone values on the inner points in balanced way.
/// </summary>
/// <seealso cref="InnerPointObserver" />
public class InnerPointObserverBalanced : GameStateObserver
{
    private readonly ulong m_mask;

    private readonly int m_positions;

    public InnerPointObserverBalanced()
    {
        m_positions = 0;
        m_mask = 0ul;

        for (int x = 0; x < 8; ++x)
        for (int y = 0; y < 8; ++y)
        {
            if ((x == 0) || (x == 7))
                continue;

            if ((y == 0) || (y == 7))
                continue;

            if ((x == 1) && (y == 1))
                continue;

            if ((x == 1) && (y == 6))
                continue;

            if ((x == 6) && (y == 1))
                continue;

            if ((x == 6) && (y == 6))
                continue;

            ++m_positions;
            m_mask |= BitBoardState.GetBitMaskForField(x, y);

        }
    }

    public override ObserverCategory ObserverType => ObserverCategory.BalancedDiscreet;

    public override int HighestObservedValue => m_positions;
    public override string Interpretation =>
        "Number of inner points occupied by player seen from black";

    public override bool IsControlledByPlayer(int playerPerspective)
    {
        return true;
    }

    public override int GetDiscreetObservation(IHashableGameState gameState)
    {
        BitBoardState board = (BitBoardState)gameState;
        return board.GetStoneCountForMask(true, m_mask) - board.GetStoneCountForMask(false, m_mask);
    }
}