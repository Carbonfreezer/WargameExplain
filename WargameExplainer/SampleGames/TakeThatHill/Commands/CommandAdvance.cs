using WargameExplainer.Strategy;

namespace WargameExplainer.SampleGames.TakeThatHill.Commands;

/// <summary>
///     Command to advance the current gaming phase.
/// </summary>
/// <param name="phaseManager">The manager that takes cre of the gaming phase.</param>
public readonly struct CommandAdvanceOperation(GamePhaseManager phaseManager) : ICommand
{
    public void Execute()
    {
        phaseManager.AdvanceStage();
    }

    public void Undo()
    {
        phaseManager.ReverseStage();
    }
}