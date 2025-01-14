namespace WargameExplainer.Board;

/// <summary>
///     Helper class to encode board games
/// </summary>
public static class BoardCoder
{
    /// <summary>
    ///     The neighbors we have for a given cube coordinates.
    /// </summary>
    private static readonly CoordinatesCube[] m_cubeNeigbors =
    [
        new CoordinatesCube(1, 0, -1), new CoordinatesCube(1, -1, 0), new CoordinatesCube(0, -1, 1),
        new CoordinatesCube(-1, 0, 1), new CoordinatesCube(-1, 1, 0), new CoordinatesCube(0, 1, -1)
    ];


    /// <summary>
    ///     The corner points of a pointy top hex for radius 1.
    /// </summary>
    public static List<(float x, float y)> CornerPointyTop;


    /// <summary>
    ///     Contains the flat top corners for radius 1.
    /// </summary>
    public static List<(float x, float y)> CornerFlatTop;


    /// <summary>
    ///     Generates the corner lists.
    /// </summary>
    static BoardCoder()
    {
        CornerFlatTop = GeneratePointList([30.0f, 90.0f, 150.0f, 210.0f, 270.0f, 330.0f]);
        CornerPointyTop = GeneratePointList([0.0f, 60.0f, 120.0f, 180.0f, 240.0f, 300.0f]);
    }


    /// <summary>
    ///     Gets the amount of steps one has to go on a board between two positions.
    /// </summary>
    /// <param name="positionA">Start position in Cube coordinates.</param>
    /// <param name="positionB">End position in Cube coordinates.</param>
    /// <returns>Amount of steps to go.</returns>
    public static int GetDistanceCube(CoordinatesCube positionA, CoordinatesCube positionB)
    {
        return (Math.Abs(positionA.Q - positionB.Q) + Math.Abs(positionA.R - positionB.R) +
                Math.Abs(positionA.S - positionB.S)) / 2;
    }


    /// <summary>
    ///     Gets all hexes along a line for a given set of points.
    /// </summary>
    /// <param name="start">Start position in cube coordinates.</param>
    /// <param name="end">End position in cube coordinates.</param>
    /// <param name="startEndInclusive">Flags if we want to include the start and endpoint.</param>
    /// <returns>The line points.</returns>
    public static IEnumerable<CoordinatesAxial> LinePoints(CoordinatesCube start,
        CoordinatesCube end, bool startEndInclusive)
    {
        var steps = GetDistanceCube(start, end);
        var delta = 1.0f / steps;
        if (startEndInclusive)
            yield return start;
        for (var i = 1; i < steps; ++i)
        {
            var t = i * delta;
            var q = start.Q + (end.Q - start.Q) * t;
            var r = start.R + (end.R - start.R) * t;
            var s = start.S + (end.S - start.S) * t;
            yield return new CoordinatesCube(q, r, s);
        }

        if (startEndInclusive)
            yield return end;
    }


    /// <summary>
    ///     Gets all cells that are within a certain range around the center point
    /// </summary>
    /// <param name="center">Center point for range.</param>
    /// <param name="range">Distance to get.</param>
    /// <returns>Enumerable for points to get for the range.</returns>
    public static IEnumerable<CoordinatesCube> RangeFinder(CoordinatesCube center, int range)
    {
        for (var q = -range; q <= range; ++q)
        for (var r = Math.Max(-range, -q - range); r <= Math.Min(range, -q + range); ++r)
            yield return new CoordinatesCube(q + center.Q, r + center.R, -q - r + center.S);
    }

    /// <summary>
    ///     Gets all the cube neighbors of an indicated position.
    /// </summary>
    /// <param name="center">Position to check for neighbors.</param>
    /// <returns>Neighborhood position.</returns>
    public static IEnumerable<CoordinatesAxial> GetNeighbors(CoordinatesCube center)
    {
        return m_cubeNeigbors.Select(cubeNeighbor => (CoordinatesAxial)(cubeNeighbor + center));
    }


    /// <summary>
    ///     Helper method to build corners for the hexagon structure.
    /// </summary>
    /// <param name="inArray"></param>
    /// <returns></returns>
    private static List<(float x, float y)> GeneratePointList(float[] inArray)
    {
        var result = new List<(float x, float y)>(6);
        foreach (var degree in inArray)
        {
            var rad = degree / 180.0f * MathF.PI;
            result.Add((MathF.Sin(rad), MathF.Cos(rad)));
        }

        return result;
    }
}