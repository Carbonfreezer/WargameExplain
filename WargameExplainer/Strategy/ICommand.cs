namespace WargameExplainer.Strategy;

/// <summary>
///     Interface for a general command, that can influence the game state.
/// </summary>
public interface ICommand
{
    /// <summary>
    ///     The execution of the command.
    /// </summary>
    void Execute();

    /// <summary>
    ///     Call to undo the execution.
    /// </summary>
    void Undo();
}