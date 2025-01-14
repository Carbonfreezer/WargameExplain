using System.Diagnostics;

namespace WargameExplainer.Board;

/// <summary>
///     Administrates the pieces on the board. One tile may have several pieces being placed on it.
/// </summary>
public class PieceCollection
{
    /// <summary>
    ///     Indicates the board layout we have.
    /// </summary>
    private readonly GameBoardLayout m_boardLayout;

    /// <summary>
    ///     The amount of hash fields we use.
    /// </summary>
    private readonly int m_hashSize;

    /// <summary>
    ///     Contains the piece collection.
    /// </summary>
    private readonly List<IPositional>[] m_hashTable;

    /// <summary>
    ///     The collection of the  pieces.
    /// </summary>
    /// <param name="layout">The board layout to check, if something is on board.</param>
    /// <param name="hashSize">
    ///     The hash size for the table we use. The more pieces we expect to be in game, the larger the hash
    ///     size.
    /// </param>
    public PieceCollection(GameBoardLayout layout, int hashSize)
    {
        m_boardLayout = layout;
        m_hashSize = hashSize;

        m_hashTable = new List<IPositional>[hashSize];
        for (var i = 0; i < hashSize; ++i)
            m_hashTable[i] = new List<IPositional>();
    }


    /// <summary>
    ///     Gets all the elements where we can walk to. Elements must be in range and on board.
    ///     WARNING: This method only works for convex layouts, otherwise the A* method has to be used.
    /// </summary>
    /// <param name="element">The element we would like to move.</param>
    /// <param name="range">The maximum range we can move.</param>
    /// <param name="destinationFree">Indicator, if the destination must be free.</param>
    /// <returns>Enumerator with legal positions.</returns>
    public IEnumerable<CoordinatesCube> GetWalkingPositions(IPositional element, int range,
        bool destinationFree)
    {
        var rawElements = BoardCoder.RangeFinder(element.Position, range);
        var onBoard = rawElements.Where(param => m_boardLayout.IsInBoard(param));
        if (destinationFree)
            onBoard = onBoard.Where(candidate => IsFree(candidate));
        return onBoard;
    }

    /// <summary>
    ///     Inserts a new element into the board.
    /// </summary>
    /// <param name="element">Element to place.</param>
    public void PlaceElement(IPositional element)
    {
        m_hashTable[element.Position.GetHashCode() % m_hashSize].Add(element);
    }

    /// <summary>
    ///     Removes an element from the board.
    /// </summary>
    /// <param name="element">Element to remove.</param>
    public void RemoveElement(IPositional element)
    {
        bool success = m_hashTable[element.Position.GetHashCode() % m_hashSize].Remove(element);
        Debug.Assert(success, "Element not stored in board.");
    }


    /// <summary>
    ///     Checks if the indicated position is free.
    /// </summary>
    /// <param name="position">Position to check.</param>
    /// <returns>Info if place is free.</returns>
    public bool IsFree(CoordinatesAxial position)
    {
        return m_hashTable[position.GetHashCode() % m_hashSize].All(element => element.Position != position);
    }

    /// <summary>
    ///     Gets all the board elements at a specific position.
    /// </summary>
    /// <param name="position">The position we query.</param>
    /// <returns>List with the elements stored there.</returns>
    public IEnumerable<IPositional> GetElements(CoordinatesAxial position)
    {
        return m_hashTable[position.GetHashCode() % m_hashSize].Where(element => element.Position == position);
    }

    /// <summary>
    ///     Gets all the elements that are on the game board.
    /// </summary>
    /// <returns>List of all elements.</returns>
    public IEnumerable<IPositional> GetAllElements()
    {
        return m_hashTable.SelectMany(x => x);
    }
}