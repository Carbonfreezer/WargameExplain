using WargameExplainer.Strategy;
using WargameExplainer.TrainingSystem;

namespace WargameExplainer.SampleGames.OthelloBit.Training;

/// <summary>
///     Observes diagonal elements for the board as balanced observer.
/// </summary>
/// <seealso cref="SidePointObserver" />
/// <param name="sideOffset">How far are we away from the outer corner.</param>
public class SidePointObserverBalanced(int sideOffset) : GameStateObserver
{
    private readonly ulong m_mask = BitBoardState.GetBitMaskForField(0 + sideOffset, 0) |
                                    BitBoardState.GetBitMaskForField(0 + sideOffset, 7) |
                                    BitBoardState.GetBitMaskForField(7 - sideOffset, 0) |
                                    BitBoardState.GetBitMaskForField(7 - sideOffset, 7) |
                                    BitBoardState.GetBitMaskForField(0, 0 + sideOffset) |
                                    BitBoardState.GetBitMaskForField(7, 0 + sideOffset) |
                                    BitBoardState.GetBitMaskForField(0, 7 - sideOffset) |
                                    BitBoardState.GetBitMaskForField(7, 7 - sideOffset);


    public override ObserverCategory ObserverType => ObserverCategory.BalancedDiscreet;

    public override int HighestObservedValue => 8;

    public override string Interpretation =>
        $"Number of side elements with offset {sideOffset} seen from black ";

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