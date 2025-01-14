using System.Drawing;
using System.Drawing.Imaging;

#pragma warning disable CA1416
namespace WargameExplainer.DrawingSystem;

/// <summary>
///     Helper class that is capable of creating images from a game board. Only functional on windows.
/// </summary>
public class DebugPainter : IDisposable
{
    /// <summary>
    ///     The bitmap we want to paint to.
    /// </summary>
    private readonly Bitmap m_bitmap;

    /// <summary>
    ///     The graphics image to paint to.
    /// </summary>
    private readonly Graphics m_graphics;

    /// <summary>
    ///     Generate serial filenames for debugging.
    /// </summary>
    private int m_serialNumber;

    /// <summary>
    ///     Creates the painter.
    /// </summary>
    /// <param name="pixelWidth">The pixel width of the final image.</param>
    /// <param name="pixelHeight">The pixel height of the final image.</param>
    public DebugPainter(int pixelWidth, int pixelHeight)
    {
        m_bitmap = new Bitmap(pixelWidth, pixelHeight);
        m_graphics = Graphics.FromImage(m_bitmap);
    }

    /// <summary>
    ///     Disposes the required resources.
    /// </summary>
    public void Dispose()
    {
        m_bitmap.Dispose();
        m_graphics.Dispose();
    }

    /// <summary>
    ///     Clears the image.
    /// </summary>
    public void ClearImage()
    {
        m_graphics.Clear(Color.White);
    }


    /// <summary>
    ///     Generates a complete snapshot and saves to the file if the painatble wants it.
    /// </summary>
    /// <param name="fileName">Filename to save to.</param>
    /// <param name="paintable">The paintable to generate a file for.</param>
    public void GenerateSnapShot(string fileName, IPaintable paintable)
    {
        ClearImage();
        paintable.PaintElement(m_graphics, m_bitmap.Width, m_bitmap.Height);
        m_bitmap.Save(fileName, ImageFormat.Png);
    }

    /// <summary>
    ///     Generates a snap shot with a serial number.
    /// </summary>
    /// <param name="paintable">The painting element we take.</param>
    public void GenerateSnapshotSerial(IPaintable paintable)
    {
        GenerateSnapShot($"Shot_{m_serialNumber++}.png", paintable);
    }
}

#pragma warning restore CA1416