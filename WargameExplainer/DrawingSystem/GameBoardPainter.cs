using System.Drawing;
using WargameExplainer.Board;

namespace WargameExplainer.DrawingSystem;

#pragma warning disable CA1416

/// <summary>
///     General call to paint a game pieces and the entire hex game board.
/// </summary>
public class GameBoardPainter : IPaintable
{
    /// <summary>
    ///     The layout we paint.
    /// </summary>
    private readonly GameBoardLayout m_layout;

    /// <summary>
    ///     The collection of the pieces we may paint.
    /// </summary>
    private readonly PieceCollection m_pieceCollection;

    /// <summary>
    ///     Contains the height of the current image we paint to.
    /// </summary>
    private int m_imageHeight;

    /// <summary>
    ///     Contains the widht of the current image we paint to.
    /// </summary>
    private int m_imageWidth;


    /// <summary>
    ///     The painter for the game pieces.
    /// </summary>
    /// <param name="layout">The layout we use for the pieces.</param>
    /// <param name="pieceCollection">The pieces on the pieces.</param>
    public GameBoardPainter(GameBoardLayout layout, PieceCollection pieceCollection)
    {
        m_layout = layout;
        m_pieceCollection = pieceCollection;
    }

    /// <summary>
    /// Currently not really used, just to satisfy the interface.
    /// </summary>
    public bool WantsScreenshot => true;

    /// <inheritdoc />
    public void PaintElement(Graphics graphics, int imageWidth, int imageHeight)
    {
        m_imageWidth = imageWidth;
        m_imageHeight = imageHeight;

        float cellSize = GetCellSize(m_layout);
        PaintGameBoard(graphics, m_layout, cellSize);
        PaintPieces(graphics, m_pieceCollection, m_layout, cellSize);
    }

    /// <summary>
    ///     Gets the maximum cell size for the indicated gameboard layout.
    /// </summary>
    /// <param name="layout">Used layout</param>
    /// <returns>Indicated cell size fitting for the image.</returns>
    private float GetCellSize(GameBoardLayout layout)
    {
        var extension = layout.GetMaxExtension();
        var width = 0.5f * m_imageWidth / extension.width;
        var height = 0.5f * m_imageHeight / extension.height;
        return MathF.Min(width, height);
    }

    /// <summary>
    ///     Draws the cell structure.
    /// </summary>
    /// <param name="graphics">The graphics where to paint information to.</param>
    /// <param name="layout">The layout we use for drawing.</param>
    /// <param name="cellSize">The cell size.</param>
    private void PaintGameBoard(Graphics graphics, GameBoardLayout layout, float cellSize)
    {
        var drawingPen = new Pen(Color.Black, 1);
        var pointList = layout.Orientation == OrientationType.PointyTop
            ? BoardCoder.CornerPointyTop
            : BoardCoder.CornerFlatTop;
        var points = new PointF[7];
        foreach (var center in layout.GetAllTiles())
        {
            var (xBase, yBase) = center.GetPixelPosition(cellSize, layout.Orientation);
            for (var i = 0; i < 7; ++i)
            {
                var (x, y) = pointList[i % 6];
                points[i] = new PointF(xBase + x * cellSize, yBase + y * cellSize);
            }

            graphics.DrawLines(drawingPen, points);
        }
    }

    /// <summary>
    ///     Paints all the paintable elements of the pieces.
    /// </summary>
    /// <param name="graphics">The graphics where to paint information to.</param>
    /// <param name="pieces">Game pieces to paint.</param>
    /// <param name="layout">Layout to use for the pieces.</param>
    /// <param name="cellSize">Cell size we use to scale the content.</param>
    private void PaintPieces(Graphics graphics, PieceCollection pieces, GameBoardLayout layout, float cellSize)
    {
        var paintables = pieces.GetAllElements().OfType<IPaintablePiece>();
        foreach (IPaintablePiece paintable in paintables)
        {
            var pixelCenter = ((IPositional)paintable).Position.GetPixelPosition(cellSize, layout.Orientation);
            paintable.PaintElement(graphics, new PointF(pixelCenter.x, pixelCenter.y), cellSize);
        }
    }
}

#pragma warning restore CA1416