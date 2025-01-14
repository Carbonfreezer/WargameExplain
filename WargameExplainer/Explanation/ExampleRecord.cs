using WargameExplainer.DrawingSystem;
using WargameExplainer.Strategy;

namespace WargameExplainer.Explanation;

/// <summary>
///     Keeps a list of executed moves in combination with the final evaluation. Used to determine the selected games.
/// </summary>
public class ExampleRecord
{
    /// <summary>
    ///     The list with the executed commands of the example game.
    /// </summary>
    public readonly List<ICommand> m_excutedCommands = new List<ICommand>();

    /// <summary>
    ///     The final evaluation of the game.
    /// </summary>
    public float[] m_finalEvaluation = Array.Empty<float>();

    /// <summary>
    ///     The paintable game state, where also all the commands are executed on.
    /// </summary>
    public IPaintable? m_paintableGameState;
}