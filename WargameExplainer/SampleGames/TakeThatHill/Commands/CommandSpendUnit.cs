using WargameExplainer.Strategy;

namespace WargameExplainer.SampleGames.TakeThatHill.Commands;

/// <summary>
///     Command to turn a unit into spend.
/// </summary>
/// <param name="unit">Unit to spend.</param>
public readonly struct CommandSpendUnit(Unit unit) : ICommand
{
    public void Execute()
    {
        unit.IsSpend = true;
    }

    public void Undo()
    {
        unit.IsSpend = false;
    }
}