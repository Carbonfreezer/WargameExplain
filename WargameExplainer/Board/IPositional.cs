namespace WargameExplainer.Board;

/// <summary>
///     Anything that can be placed on the board.
/// </summary>
public interface IPositional
{
    /// <summary>
    ///     We have to be able to ask for the positional coordinates.
    /// </summary>
    public CoordinatesAxial Position { get; }
}