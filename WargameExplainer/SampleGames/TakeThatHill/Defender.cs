using System.Drawing;
using WargameExplainer.Board;
using WargameExplainer.DrawingSystem;
using WargameExplainer.Strategy;

namespace WargameExplainer.SampleGames.TakeThatHill;

/// <summary>
///     Contains the defender on top of the hill, that does not move.
/// </summary>
public class Defender : IPositional, IPaintablePiece, IHashable
{
    /// <summary>
    ///     Creates the defender on the game board.
    ///     It remains stationary.
    /// </summary>
    /// <param name="layout"></param>
    public Defender(GameBoardLayout layout)
    {
        Position = layout.GetAxialCoordinates( new CoordinatesOffset(6, 2)); 
    }

    /// <summary>
    ///     Flags if the unit is spent.
    /// </summary>
    public bool IsSpend { get; set; }

    /// <inheritdoc />
    public void PaintElement(Graphics graphics, PointF position, float scale)
    {
#pragma warning disable CA1416
        graphics.DrawEllipse(new Pen(IsSpend ? Color.Red : Color.Green), position.X - 0.5f * scale,
            position.Y - 0.5f * scale, scale, scale);
#pragma warning restore CA1416
    }

    /// <inheritdoc />
    public CoordinatesAxial Position { get; }

    public void AppendData(BinaryWriter writer)
    {
        writer.Write(IsSpend);
    }
}