namespace WargameExplainer.Board;

/// <summary>
///     Stores the cube coordinates.
/// </summary>
public struct CoordinatesCube(int q, int r, int s)
{
    /// <summary>
    ///     The Q component of the Cube coordinates.
    /// </summary>
    public int Q = q;

    /// <summary>
    ///     The R Component of the cube coordinates.
    /// </summary>
    public int R = r;

    /// <summary>
    ///     The s component of the cube coordinates.
    /// </summary>
    public int S = s;


    /// <summary>
    ///     Generates cube coordinates from the closest floating point.
    ///     Makes sure that Q+R+S = 0.
    /// </summary>
    /// <param name="q">q in floats</param>
    /// <param name="r">r in floats</param>
    /// <param name="s">s in floats.</param>
    public CoordinatesCube(float q, float r, float s) : this(0, 0, 0)
    {
        var qi = (int)Math.Round(q);
        var ri = (int)Math.Round(r);
        var si = (int)Math.Round(s);

        var qDiff = Math.Abs(q - qi);
        var rDiff = Math.Abs(r - ri);
        var sDiff = Math.Abs(s - si);


        if (qDiff > rDiff && qDiff > sDiff)
            qi = -ri - si;
        else if (rDiff > sDiff)
            ri = -qi - si;
        else
            si = -qi - ri;

        Q = qi;
        R = ri;
        S = si;
    }


    /// <summary>
    ///     The adding operator of q coordinates.
    /// </summary>
    /// <param name="a">First component to add.</param>
    /// <param name="b">Second component to add.</param>
    /// <returns>Sum of cube coordinates.</returns>
    public static CoordinatesCube operator +(CoordinatesCube a, CoordinatesCube b)
    {
        return new CoordinatesCube(a.Q + b.Q, a.R + b.R, a.S + b.S);
    }

    /// <summary>
    ///     Convert implicitly from coordinatesAxial.
    /// </summary>
    /// <param name="coordinatesAxial">Axial params.</param>
    public static implicit operator CoordinatesCube(CoordinatesAxial coordinatesAxial)
    {
        return new CoordinatesCube(coordinatesAxial.Q, coordinatesAxial.R, -coordinatesAxial.Q - coordinatesAxial.R);
    }
}