namespace WargameExplainer.Board;

/// <summary>
///     Axial coords.
/// </summary>
/// <param name="q">q parameter.</param>
/// <param name="r">r parameter</param>
public struct CoordinatesAxial(int q, int r)
{
    /// <summary>
    ///     The q coordinate of the element.
    /// </summary>
    public int Q = q;

    /// <summary>
    ///     The r coordinate of the element.
    /// </summary>
    public int R = r;

    /// <summary>
    ///     Store it once.
    /// </summary>
    private static readonly float ThreeSqrt = MathF.Sqrt(3.0f);

    public bool Equals(CoordinatesAxial other)
    {
        return Q == other.Q && R == other.R;
    }

    public override bool Equals(object? obj)
    {
        return obj is CoordinatesAxial other && Equals(other);
    }

    /// <summary>
    ///     Gets a hash code dor the coordinates for further processing.
    /// </summary>
    /// <returns>Hash code.</returns>
    public override int GetHashCode()
    {
        int test = 83 * Q + 89 * R;
        return test > 0 ? test : -test;
    }


    /// <summary>
    ///     Gets the pixel coordinates for drawing an element on the map.
    /// </summary>
    /// <param name="radius">Radius of the hex cell.</param>
    /// <param name="orientation">Orientation type of the hex cell..</param>
    /// <returns>Drawing coordinates of the element.</returns>
    public (float x, float y) GetPixelPosition(float radius, OrientationType orientation)
    {
        return orientation == OrientationType.PointyTop
            ? (radius * ThreeSqrt * (Q + 0.5f * R), radius * (3.0f / 2 * R))
            : (radius * (3.0f / 2 * Q), radius * ThreeSqrt * (R + 0.5f * Q));
    }


    /// <summary>
    ///     Convert implicitely from axial.
    /// </summary>
    /// <param name="coordinatesCube">Axial params.</param>
    public static implicit operator CoordinatesAxial(CoordinatesCube coordinatesCube)
    {
        return new CoordinatesAxial(coordinatesCube.Q, coordinatesCube.R);
    }

    public static bool operator ==(CoordinatesAxial a, CoordinatesAxial b)
    {
        return a.Q == b.Q && a.R == b.R;
    }

    public static bool operator !=(CoordinatesAxial a, CoordinatesAxial b)
    {
        return !(a == b);
    }
}