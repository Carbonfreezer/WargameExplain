using System.Drawing;
using WargameExplainer.Board;
using WargameExplainer.DrawingSystem;
using WargameExplainer.Strategy;
using WargameExplainer.Utils;

namespace WargameExplainer.SampleGames.TakeThatHill;


/// <summary>
/// The fighting unit of Take that hill.
/// </summary>
public class Unit : MoveableUnit, IPaintablePiece, IHashable
{
    /// <summary>
    /// Serial number of unit.
    /// </summary>
    private readonly int m_number;


    /// <summary>
    /// Generates the unit.
    /// </summary>
    /// <param name="startPosition">Where is it placed on the board.</param>
    /// <param name="layout">The layout to place it on.</param>
    /// <param name="number">The identifier of the unit.</param>
    public Unit(CoordinatesOffset startPosition, GameBoardLayout layout, int number)
    {
        Position = layout.GetAxialCoordinates(startPosition);
        m_number = number;
    }

    /// <summary>
    ///     Flags if the unit is spent.
    /// </summary>
    public bool IsSpend { get; set; }

    /// <summary>
    /// Flags, if we have fired. Relevant for the after state.
    /// </summary>
    public bool HasFired { get; set; }

    /// <summary>
    /// This is a transitory state just for debug painting.
    /// </summary>
    public bool HasBeenHit { get; set; }

    /// <inheritdoc />
    public void PaintElement(Graphics graphics, PointF position, float scale)
    {
#pragma warning disable CA1416

        Pen drawPen = new Pen(IsSpend ? Color.Red : Color.Green, HasFired ? 4 : 1);
        Font drawFont = new Font("Arial", 12);

        graphics.DrawRectangle(drawPen, position.X - 0.3f * scale, position.Y - 0.3f * scale, 0.6f * scale,
            0.6f * scale);
        graphics.DrawString($"{m_number}", drawFont, new SolidBrush(Color.Black), position);

        if (HasBeenHit)
        {
            HasBeenHit = false;
            graphics.DrawLine(drawPen, position.X - 0.4f * scale, position.Y - 0.4f * scale, position.X +0.4f * scale,
                position.Y + 0.4f * scale);
        }
#pragma warning restore CA1416
    }

    /// <inheritdoc />
    public void AppendData(BinaryWriter writer)
    {
        writer.Write(IsSpend);
        writer.Write(HasFired);
        writer.Write(Position.Q);
        writer.Write(Position.R);
    }
}