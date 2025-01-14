using WargameExplainer.Board;

namespace WargameExplainer.TestElements;

/// <summary>
///     This layout represents a simple rectangular layout of the board game.
/// </summary>
public class GameBoardLayoutRectangular : GameBoardLayout
{
    /// <summary>
    ///     The height of the board.
    /// </summary>
    private readonly int m_height;

    /// <summary>
    ///     The width of the board.
    /// </summary>
    private readonly int m_width;

    /// <summary>
    ///     Generates the rectangular layout. To make the game more compatible, we start in the corner with coordinates (1,1).
    /// </summary>
    /// <param name="orientation">Flat / or pointy top.</param>
    /// <param name="offset">Are even or odd elements right shifted.</param>
    /// <param name="width">Width of the board.</param>
    /// <param name="height">Height of the board.</param>
    public GameBoardLayoutRectangular(OrientationType orientation, OffsetType offset, int width, int height) : base(
        orientation, offset)
    {
        m_width = width;
        m_height = height;
    }

    /// <inheritdoc/>
    public override bool IsInBoard(CoordinatesAxial position)
    {
        CoordinatesOffset offset = new CoordinatesOffset(position, Orientation, Offset);
        return offset.Col >= 1 && offset.Col <= m_width && offset.Row >= 1 && offset.Row <= m_height;
    }

    /// <inheritdoc/>
    public override IEnumerable<CoordinatesAxial> GetAllTiles()
    {
        for(int col = 1; col <= m_width; ++col)
        for (int row = 1; row <= m_height; ++row)
        {
            CoordinatesOffset offset = new CoordinatesOffset(col, row);
            yield return offset.GetAxial(Orientation, Offset);
        }
    }

    /// <inheritdoc/>
    public override (int width, int height) GetMaxExtension()
    {
        return (m_width, m_height);
    }
}