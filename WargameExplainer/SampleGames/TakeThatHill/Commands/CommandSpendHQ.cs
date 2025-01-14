using WargameExplainer.Strategy;

namespace WargameExplainer.SampleGames.TakeThatHill.Commands;

/// <summary>
///     Command that turns the HQ to spend.
/// </summary>
/// <param name="hq">HQ to spend</param>
public readonly struct CommandSpendHq(BaseHq hq) : ICommand
{
    public void Execute()
    {
        hq.IsSpend = true;
    }

    public void Undo()
    {
        hq.IsSpend = false;
    }
}