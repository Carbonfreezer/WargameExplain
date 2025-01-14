using WargameExplainer.Board;
using WargameExplainer.Strategy;

namespace WargameExplainer.Utils;

/// <summary>
///     The movement command for the unit to move to a different position.
/// </summary>
/// <param name="unit">The unit to move.</param>
/// <param name="destination">The point where we want to go to.</param>
/// <param name="pieces">The collection of pieces we have.</param>
public struct CommandMovement(MoveableUnit unit, CoordinatesAxial destination, PieceCollection pieces)
    : ICommand
{
    /// <summary>
    ///     The position, where we originally were.
    /// </summary>
    private CoordinatesAxial m_oldPosition;


    /// <inheritdoc />
    public void Execute()
    {
        m_oldPosition = unit.MoveToField(destination, pieces);
    }

    /// <inheritdoc />
    public void Undo()
    {
        unit.MoveToField(m_oldPosition, pieces);
    }
}