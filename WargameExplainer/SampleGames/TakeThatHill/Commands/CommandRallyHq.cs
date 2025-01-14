using WargameExplainer.Strategy;

namespace WargameExplainer.SampleGames.TakeThatHill.Commands;

/// <summary>
///     Command to rally the HQ.
/// </summary>
/// <param name="hq">HQ to rally.</param>
public readonly struct CommandRallyHq(BaseHq hq) : ICommand
{
    public void Execute()
    {
        hq.IsSpend = false;
    }

    public void Undo()
    {
        hq.IsSpend = true;
    }
}