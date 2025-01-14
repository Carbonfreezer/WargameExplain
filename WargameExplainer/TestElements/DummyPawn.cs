using System.Drawing;
using WargameExplainer.Board;
using WargameExplainer.DrawingSystem;

namespace WargameExplainer.TestElements;

#pragma warning disable CA1416

/// <summary>
/// Dummy pawn for testing.
/// </summary>
public class DummyPawn : IPositional, IPaintablePiece
{
    public DummyPawn(int number, GameBoardLayout layout, CoordinatesOffset boardPosition)
    {
        Number = number;
        Position = boardPosition.GetAxial(layout.Orientation, layout.Offset);
    }


    public int Number { get; }

    public CoordinatesAxial Position { get; }
    public void PaintElement(Graphics graphics, PointF position, float scale)
    {
        graphics.DrawEllipse(new Pen(Color.Green), position.X - 0.5f * scale, position.Y - 0.5f * scale, scale, scale);
    }
}

#pragma warning restore CA1416