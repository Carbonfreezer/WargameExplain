namespace WargameExplainer.Strategy;

/// <summary>
///     The manipulator is the instance, that can actually manipulate the game.
/// </summary>
public interface IManipulator
{
    /// <summary>
    ///     Checks if the current decision is random and based on probabilities.
    /// </summary>
    bool IsRandomManipulator { get; }


    /// <summary>
    /// Asks for the move options and if we are random also the selection probabilities are completed in the span.
    /// </summary>
    /// <param name="probabilities">The probability field that gets completed if it is not a null span.</param>
    /// <returns>The list of commands we can execute in the state.</returns>
    IList<ICommand> GetMoveOptionsWithProbabilities(in Span<float> probabilities);

    /// <summary>
    ///     The index of the player to read into the evaluation field, for result maximization.
    /// </summary>
    int PlayerPerspective { get; }

    /// <summary>
    /// Asks for the maximum number of probability choices we have in the random stage.
    /// This is needed to reserve the space for the span in the Method GetMoveOptionsWithProbabilities.
    /// </summary>
    /// <seealso cref="GetMoveOptionsWithProbabilities"/>
    int MaxNumOfProbabilityChoices { get; }

}