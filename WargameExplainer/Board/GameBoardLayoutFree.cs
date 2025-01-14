namespace WargameExplainer.Board;

/// <summary>
///     Contains a free gameboard layout, that can have dedicated tiles added.
/// </summary>
public class GameBoardLayoutFree : GameBoardLayout
{
    /// <summary>
    ///     The amount of hash fields we use.
    /// </summary>
    private readonly int m_hashSize;

    /// <summary>
    ///     Contains the board summary.
    /// </summary>
    private readonly List<CoordinatesAxial>[] m_hashTable;


    /// <summary>
    ///     Generates the board layout. The board is empty at the beginning.
    /// </summary>
    /// <param name="orientation">Flat / or pointy top.</param>
    /// <param name="offset">Are even or odd elements right shifted.</param>
    /// <param name="hashSize"> Hash Size we use for the board.</param>
    public GameBoardLayoutFree(OrientationType orientation, OffsetType offset, int hashSize) : base(orientation, offset)
    {
        m_hashSize = hashSize;
        m_hashTable = new List<CoordinatesAxial>[m_hashSize];
        for (var i = 0; i < m_hashSize; ++i)
            m_hashTable[i] = new List<CoordinatesAxial>();
    }


    /// <summary>
    ///     Adds a tile to the game board.
    /// </summary>
    /// <param name="column">Column index of tile.</param>
    /// <param name="row">Row index of tile.</param>
    public void AddTile(int column, int row)
    {
        var element = new CoordinatesOffset(column, row).GetAxial(Orientation, Offset);
        m_hashTable[element.GetHashCode() % m_hashSize].Add(element);
    }

    /// <inheritdoc />
    public override bool IsInBoard(CoordinatesAxial position)
    {
        return m_hashTable[position.GetHashCode() % m_hashSize].Contains(position);
    }

    /// <inheritdoc />
    public override IEnumerable<CoordinatesAxial> GetAllTiles()
    {
        return m_hashTable.SelectMany(x => x);
    }

    /// <inheritdoc />
    public override (int width, int height) GetMaxExtension()
    {
        var flattened = GetAllTiles();
        var offsetTiles = flattened.Select(position => new CoordinatesOffset(position, Orientation, Offset)).ToList();
        int maxWidth = offsetTiles.Max(element => element.Col);
        int maxHeight = offsetTiles.Max(element => element.Row);

        return (maxWidth, maxHeight);
    }
}