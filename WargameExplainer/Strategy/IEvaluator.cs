namespace WargameExplainer.Strategy;

/// <summary>
///     The evaluator interface performs the evaluation of the afterstate value.
/// </summary>
public interface IEvaluator
{
    /// <summary>
    ///     Gets the amount of player perspectives in the game.
    /// </summary>
    int NumOfPlayerPerspectives { get; }

    /// <summary>
    /// Asks for the game evaluation and if the game ends.
    /// </summary>
    /// <param name="afterStateValues">Span with aftertstate values to fill.</param>
    /// <returns>Indicates, if the game has ended here.</returns>
    bool IsGameOverAndEvaluate(in Span<float> afterStateValues);
}