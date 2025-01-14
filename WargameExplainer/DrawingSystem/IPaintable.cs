using System.Drawing;

namespace WargameExplainer.DrawingSystem;

/// <summary>
///     Anything that can paint itself.
/// </summary>
public interface IPaintable
{
    /// <summary>
    ///     Indicates if the element would like to deliver a screenshot in the current situation.
    /// </summary>
    bool WantsScreenshot { get; }

    /// <summary>
    ///     Interface to be able to paint something.
    /// </summary>
    /// <param name="graphics">The graphics context we use.</param>
    /// <param name="imageWidth">The image width.</param>
    /// <param name="imageHeight">The image height.</param>
    void PaintElement(Graphics graphics, int imageWidth, int imageHeight);
}