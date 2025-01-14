using WargameExplainer.Strategy;

namespace WargameExplainer.SampleGames.TakeThatHill.Commands;

/// <summary>
///     Command to rally a unit.
/// </summary>
/// <param name="unit">Unit to rally.</param>
public readonly struct CommandRallyUnit(Unit unit) : ICommand
{
    public void Execute()
    {
        unit.IsSpend = false;
    }

    public void Undo()
    {
        unit.IsSpend = true;
    }
}