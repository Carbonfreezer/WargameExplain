using WargameExplainer.Strategy;
using WargameExplainer.TrainingSystem;

namespace WargameExplainer.SampleGames.OthelloBit.Training;


/// <summary>
/// Observes all positions that are not on the boundary and the inner corner points.
/// </summary>
public class InnerPointObserver : GameStateObserver
{
    private readonly bool m_isBlack;

    private readonly ulong m_mask;

    private readonly int m_positions;

    public InnerPointObserver(bool isBlack)
    {
        m_isBlack = isBlack;

        m_positions = 0;
        m_mask = 0ul;

        for(int x = 0; x < 8; ++x)
        for (int y = 0; y < 8; ++y)
        {
            if ((x == 0) || (x== 7))
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

    public override ObserverCategory ObserverType => ObserverCategory.Discreet;

    public override int HighestObservedValue => m_positions;
    public override string Interpretation =>
        "Number of inner points occupied by player " + (m_isBlack ? "black" : "white");

    public override bool IsControlledByPlayer(int playerPerspective)
    {
        return ((playerPerspective == 0) && m_isBlack) || ((playerPerspective == 1) && (!m_isBlack));
    }

    public override int GetDiscreetObservation(IHashableGameState gameState)
    {
        return ((BitBoardState)gameState).GetStoneCountForMask(m_isBlack, m_mask);
    }
}