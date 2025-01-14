using WargameExplainer.Strategy;

namespace WargameExplainer.SampleGames.TakeThatHill.Commands;

/// <summary>
///     Command to eventually set the defender illumination (if not previously set already).
/// </summary>
/// <param name="game">The game state we refer to.</param>
/// <param name="illuminationRound">The round information when to apply.</param>
public class CommandSetDefenderIllumination(TakeThatHillGameState game) : ICommand
{
    private bool m_wasDefenderIlluminationSet;

    public void Execute()
    {
        m_wasDefenderIlluminationSet = game.IlluminationDefenderRond != -2;
        if (m_wasDefenderIlluminationSet)
            return;

        game.IlluminationDefenderRond = game.GameRound + 1;
    }

    public void Undo()
    {
        if (!m_wasDefenderIlluminationSet)
            game.IlluminationDefenderRond = -2;
    }
}