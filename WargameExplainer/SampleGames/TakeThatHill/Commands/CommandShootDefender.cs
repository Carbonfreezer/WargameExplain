using WargameExplainer.Strategy;

namespace WargameExplainer.SampleGames.TakeThatHill.Commands;

/// <summary>
///     Command to shoot at the defender.
/// </summary>
/// <param name="enemy">Defender to shoot at.</param>
public struct CommandShootDefender(Defender enemy) : ICommand
{
    /// <summary>
    ///     Was the defender already depleted before shot at?
    /// </summary>
    private bool m_wasSpend;

    public void Execute()
    {
        m_wasSpend = enemy.IsSpend;
        enemy.IsSpend = true;
    }

    public void Undo()
    {
        enemy.IsSpend = m_wasSpend;
    }
}