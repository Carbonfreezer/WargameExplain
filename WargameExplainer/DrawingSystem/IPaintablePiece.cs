using System.Drawing;

namespace WargameExplainer.DrawingSystem;

/// <summary>
///     Interface to paint elements into the debug draw.
/// </summary>
public interface IPaintablePiece
{
    /// <summary>
    ///     Paints the element on the bitmap at the indicated position.
    /// </summary>
    /// <param name="graphics">Graphics Context to paint with.</param>
    /// <param name="position">Position to paint element to.</param>
    /// <param name="scale">Scale to apply.</param>
    void PaintElement(Graphics graphics, PointF position, float scale);
}