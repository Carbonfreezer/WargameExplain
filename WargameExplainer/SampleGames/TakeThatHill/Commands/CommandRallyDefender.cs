using WargameExplainer.Strategy;

namespace WargameExplainer.SampleGames.TakeThatHill.Commands;

/// <summary>
///     Command to rally the defender.
/// </summary>
/// <param name="enemy">Defender to rally.</param>
public readonly struct CommandRallyDefender(Defender enemy) : ICommand
{
    public void Execute()
    {
        enemy.IsSpend = false;
    }

    public void Undo()
    {
        enemy.IsSpend = true;
    }
}