using WargameExplainer.Board;

namespace WargameExplainer.Utils;

/// <summary>
///     Contains units that are moveable.
/// </summary>
public class MoveableUnit : IPositional
{
    /// <inheritdoc />

    public CoordinatesAxial Position { get; protected set; }


    /// <summary>
    ///     Moves the element to the new position.
    /// </summary>
    /// <param name="newPosition">Position where to move to.</param>
    /// <param name="board">PieceCollection to move on.</param>
    public CoordinatesAxial MoveToField(CoordinatesAxial newPosition, PieceCollection board)
    {
        CoordinatesAxial oldPosition = Position;
        board.RemoveElement(this);
        Position = newPosition;
        board.PlaceElement(this);
        return oldPosition;
    }
}