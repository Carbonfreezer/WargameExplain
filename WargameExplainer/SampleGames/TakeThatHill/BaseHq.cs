using System.Drawing;
using WargameExplainer.Board;
using WargameExplainer.DrawingSystem;
using WargameExplainer.Strategy;
using WargameExplainer.Utils;

namespace WargameExplainer.SampleGames.TakeThatHill;

/// <summary>
///     The base Hq from take that hill.
/// </summary>
public class BaseHq : MoveableUnit, IPaintablePiece, IHashable
{
    /// <summary>
    ///     Gets initialized with a start position.
    /// </summary>
    /// <param name="startPosition">Where to start.</param>
    /// <param name="layout">Layout used.</param>
    public BaseHq(CoordinatesOffset startPosition, GameBoardLayout layout)
    {
        Position = layout.GetAxialCoordinates(startPosition); 
    }

    /// <summary>
    ///     Flags if the unit is spent.
    /// </summary>
    public bool IsSpend { get; set; }

    /// <inheritdoc />
    public void PaintElement(Graphics graphics, PointF position, float scale)
    {
#pragma warning disable CA1416
        Pen drawPen = new Pen(IsSpend ? Color.Red : Color.Green);
        graphics.DrawRectangle(drawPen, position.X - 0.1f * scale, position.Y - 0.1f * scale, 0.2f * scale,
            0.2f * scale);
        graphics.DrawLine(drawPen, position.X - 0.1f * scale, position.Y - 0.1f * scale, position.X + 0.1f * scale,
            position.Y + 0.1f * scale);
#pragma warning restore CA1416
    }


    /// <inheritdoc />
    public void AppendData(BinaryWriter writer)
    {
        writer.Write(Position.Q);
        writer.Write(Position.R);
        writer.Write(IsSpend);
    }
}