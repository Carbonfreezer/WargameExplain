using System.Diagnostics;
using static WargameExplainer.Board.BoardCoder;

namespace WargameExplainer.Board;

/// <summary>
///     Indicates which columns are right shifted in board game expansion
/// </summary>
public enum OffsetType
{
    /// <summary>
    ///     Odd columns are right shifted.
    /// </summary>
    OddOffset,

    /// <summary>
    ///     Even columns are right shifted.
    /// </summary>
    EvenOffset
}

/// <summary>
///     Hex fields can be orientated with the flat top or a pointy top on the board.
/// </summary>
public enum OrientationType
{
    /// <summary>
    ///     The flat side is on the top of the hex field.
    /// </summary>
    FlatTop,

    /// <summary>
    ///     The pointy side is on the top of the hex field.
    /// </summary>
    PointyTop
}

/// <summary>
///     Offset coordinates for direct representation used in the graphical layout.
/// </summary>
public struct CoordinatesOffset(int c, int r)
{
    /// <summary>
    ///     Column of the cell.
    /// </summary>
    public int Col = c;

    /// <summary>
    ///     Row of the cell.
    /// </summary>
    public int Row = r;


    /// <summary>
    ///     Transforms axial coordinates to plain offset coordinates. Plain coordinates are used in board drawing.
    /// </summary>
    /// <param name="position">The axial coordinates we want to transform</param>
    /// <param name="fieldOrientation">Is the game board float top or pointy top?</param>
    /// <param name="offset">Are the even or the odd rows right shifted.</param>
    public CoordinatesOffset(CoordinatesAxial position, OrientationType fieldOrientation, OffsetType offset) : this(0, 0)
    {
        switch (fieldOrientation)
        {
            case OrientationType.FlatTop:
                switch (offset)
                {
                    case OffsetType.EvenOffset:
                        Col = position.Q;
                        Row = position.R + (position.Q + (position.Q & 1)) / 2;
                        break;
                    case OffsetType.OddOffset:
                        Col = position.Q;
                        Row = position.R + (position.Q - (position.Q & 1)) / 2;
                        break;
                }

                break;
            case OrientationType.PointyTop:
                switch (offset)
                {
                    case OffsetType.EvenOffset:
                        Col = position.Q + (position.R + (position.R & 1)) / 2;
                        Row = position.R;
                        break;
                    case OffsetType.OddOffset:
                        Col = position.Q + (position.R - (position.R & 1)) / 2;
                        Row = position.R;
                        break;
                }

                break;
        }
    }


    /// <summary>
    ///     Transforms plain offset coordinates to axial coordinates. Plain coordinates are used in board drawing.
    /// </summary>
    /// <param name="fieldOrientation">Is the game board float top or pointy top?</param>
    /// <param name="offset">Are the even or the odd rows right shifted.</param>
    /// <returns>Axial coordinates of the </returns>
    public CoordinatesAxial GetAxial(OrientationType fieldOrientation, OffsetType offset)
    {
        switch (fieldOrientation)
        {
            case OrientationType.FlatTop:
                switch (offset)
                {
                    case OffsetType.EvenOffset:
                        return new CoordinatesAxial(Col, Row - (Col + (Col & 1)) / 2);
                    case OffsetType.OddOffset:
                        return new CoordinatesAxial(Col, Row - (Col - (Col & 1)) / 2);
                }

                break;
            case OrientationType.PointyTop:
                switch (offset)
                {
                    case OffsetType.EvenOffset:
                        return new CoordinatesAxial(Col - (Row + (Row & 1)) / 2, Row);
                    case OffsetType.OddOffset:
                        return new CoordinatesAxial(Col - (Row - (Row & 1)) / 2, Row);
                }

                break;
        }

        Debug.Assert(false, "Should never appear here.");
        return new CoordinatesAxial(0, 0);
    }
}