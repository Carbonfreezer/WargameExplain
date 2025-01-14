using WargameExplainer.Strategy;
using WargameExplainer.TrainingSystem;

namespace WargameExplainer.SampleGames.OthelloBit.Training;


/// <summary>
/// Observes diagonal elements for the board.
/// </summary>
/// <param name="isBlack">Flags if we are looking for black elements.</param>
/// <param name="diagonalOffset">How far are we away from the outer corner.</param>
public class DiagonalObserver(bool isBlack, int diagonalOffset) : GameStateObserver
{
    private readonly ulong m_mask = BitBoardState.GetBitMaskForField(0 + diagonalOffset, 0 + diagonalOffset) |
                                    BitBoardState.GetBitMaskForField(7 - diagonalOffset, 0 + diagonalOffset) |
                                    BitBoardState.GetBitMaskForField(0 + diagonalOffset, 7 - diagonalOffset) |
                                    BitBoardState.GetBitMaskForField(7 - diagonalOffset, 7 - diagonalOffset);

    public override ObserverCategory ObserverType => ObserverCategory.Discreet;

    public override int HighestObservedValue => 4;

    public override string Interpretation =>
        $"Number of diagonal elements with offset {diagonalOffset} occupied by player " + (isBlack ? "black" : "white");

    public override bool IsControlledByPlayer(int playerPerspective)
    {
        return ((playerPerspective == 0) && isBlack) || ((playerPerspective == 1) && (!isBlack));
    }

    public override int GetDiscreetObservation(IHashableGameState gameState)
    {
        return ((BitBoardState)gameState).GetStoneCountForMask(isBlack, m_mask);
    }
}