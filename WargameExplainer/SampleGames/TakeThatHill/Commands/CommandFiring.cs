using WargameExplainer.Strategy;

namespace WargameExplainer.SampleGames.TakeThatHill.Commands;

/// <summary>
///     Command to reset the firing information of a unit.
/// </summary>
/// <param name="unit">Unit to reset firing information on.</param>
public readonly struct CommandResetFiring(Unit unit) : ICommand
{
    public void Execute()
    {
        unit.HasFired = false;
    }

    public void Undo()
    {
        unit.HasFired = true;
    }
}

/// <summary>
///     Command to set the firing information.
/// </summary>
/// <param name="unit">Unit to set firing information on.</param>
public readonly struct CommandSetFiring(Unit unit) : ICommand
{
    public void Execute()
    {
        unit.HasFired = true;
    }

    public void Undo()
    {
        unit.HasFired = false;
    }
}