using WargameExplainer.Strategy;
using WargameExplainer.TrainingSystem;

namespace WargameExplainer.SampleGames.OthelloBit.Training;

/// <summary>
///     Observes diagonal elements for the board.
/// </summary>
/// <param name="isBlack">Flags if we are looking for black elements.</param>
/// <param name="sideOffset">How far are we away from the outer corner.</param>
public class SidePointObserver(bool isBlack, int sideOffset) : GameStateObserver
{
    private readonly ulong m_mask = BitBoardState.GetBitMaskForField(0 + sideOffset, 0) |
                                    BitBoardState.GetBitMaskForField(0 + sideOffset, 7) |
                                    BitBoardState.GetBitMaskForField(7 - sideOffset, 0) |
                                    BitBoardState.GetBitMaskForField(7 - sideOffset, 7) |
                                    BitBoardState.GetBitMaskForField(0, 0 + sideOffset) |
                                    BitBoardState.GetBitMaskForField(7, 0 + sideOffset) |
                                    BitBoardState.GetBitMaskForField(0, 7 - sideOffset) |
                                    BitBoardState.GetBitMaskForField(7, 7 - sideOffset);


    public override ObserverCategory ObserverType => ObserverCategory.Discreet;

    public override int HighestObservedValue => 8;

    public override string Interpretation =>
        $"Number of side elements with offset {sideOffset} occupied by player " + (isBlack ? "black" : "white");

    public override bool IsControlledByPlayer(int playerPerspective)
    {
        return ((playerPerspective == 0) && isBlack) || ((playerPerspective == 1) && (!isBlack));
    }

    public override int GetDiscreetObservation(IHashableGameState gameState)
    {
        return ((BitBoardState)gameState).GetStoneCountForMask(isBlack, m_mask);
    }
}