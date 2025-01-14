namespace WargameExplainer.Board;

/// <summary>
///     This is a general call one has to derive from, that describes the layout of a board.
/// </summary>
public abstract class GameBoardLayout
{
    /// <summary>
    ///     The offset we use for encountering the elements.
    /// </summary>
    protected readonly OffsetType m_offset;

    /// <summary>
    ///     The orientation of the elements.
    /// </summary>
    protected readonly OrientationType m_orientation;

    /// <summary>
    ///     Creates the base information of the board layout.
    /// </summary>
    /// <param name="orientation">Flat / or pointy top.</param>
    /// <param name="offset">Are even or odd elements right shifted.</param>
    protected GameBoardLayout(OrientationType orientation, OffsetType offset)
    {
        m_orientation = orientation;
        m_offset = offset;
    }

    /// <summary>
    ///     Returns the offset type.
    /// </summary>
    public OffsetType Offset => m_offset;

    /// <summary>
    ///     Gets the orientation.
    /// </summary>
    public OrientationType Orientation => m_orientation;


    /// <summary>
    ///     A method to check, if the indicated position is on the board.
    /// </summary>
    /// <param name="position">Position to check.</param>
    /// <returns>Indicator, if we are on the board.</returns>
    public abstract bool IsInBoard(CoordinatesAxial position);

    /// <summary>
    ///     Asks for all the tiles in axial coordinates.
    /// </summary>
    /// <returns>All tiles in axial coordinates.</returns>
    public abstract IEnumerable<CoordinatesAxial> GetAllTiles();

    /// <summary>
    ///     Gets the maximum extenstion in tiles.
    /// </summary>
    /// <returns>Pair of maximum extension.</returns>
    public abstract (int width, int height) GetMaxExtension();

    /// <summary>
    ///     Gets the axial coordinates for a given offset coord.
    /// </summary>
    /// <param name="offsetCoords">The offset coord we have.</param>
    /// <returns>Corresponding axial coordinate.</returns>
    public CoordinatesAxial GetAxialCoordinates(CoordinatesOffset offsetCoords)
    {
        return offsetCoords.GetAxial(m_orientation, m_offset);
    }
}