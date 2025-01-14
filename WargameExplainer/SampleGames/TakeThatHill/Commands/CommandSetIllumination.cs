using WargameExplainer.Strategy;

namespace WargameExplainer.SampleGames.TakeThatHill.Commands;

/// <summary>
///     The command we use for setting the illumination round for the offender right at the beginning.
/// </summary>
/// <param name="gameState">Take that hill gate state to use.</param>
/// <param name="illuminationRound">The round when the illumination happens. </param>
public readonly struct CommandSetIllumination(TakeThatHillGameState gameState, int illuminationRound) : ICommand
{
    public void Execute()
    {
        gameState.IlluminationOffenderRound = illuminationRound;
    }

    public void Undo()
    {
        gameState.IlluminationOffenderRound = -2;
    }
}