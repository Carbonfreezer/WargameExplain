using WargameExplainer.Strategy;
using WargameExplainer.TrainingSystem;

namespace WargameExplainer.SampleGames.OthelloBit.Training;


/// <summary>
/// Diagonal observer that expresses the advantage of black.
/// </summary>
/// <see cref="DiagonalObserver"/>
/// <param name="diagonalOffset">Offset into the diagonal line.</param>
public class DiagonalObserverBalanced(int diagonalOffset) : GameStateObserver
{
    private readonly ulong m_mask = BitBoardState.GetBitMaskForField(0 + diagonalOffset, 0 + diagonalOffset) |
                                    BitBoardState.GetBitMaskForField(7 - diagonalOffset, 0 + diagonalOffset) |
                                    BitBoardState.GetBitMaskForField(0 + diagonalOffset, 7 - diagonalOffset) |
                                    BitBoardState.GetBitMaskForField(7 - diagonalOffset, 7 - diagonalOffset);

    public override ObserverCategory ObserverType => ObserverCategory.BalancedDiscreet;
    public override int HighestObservedValue => 4;

    public override string Interpretation =>
        $"Number of diagonal elements with offset {diagonalOffset} occupied seen from black";

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