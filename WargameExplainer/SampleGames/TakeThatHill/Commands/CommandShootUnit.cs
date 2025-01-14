using WargameExplainer.Strategy;

namespace WargameExplainer.SampleGames.TakeThatHill.Commands;

/// <summary>
///     The redo for the shooting operation.
/// </summary>
/// <param name="unit">Unit that shoots.</param>
/// <param name="state">The state that is used.</param>
public struct CommandShotUnit(int unit, TakeThatHillGameState state) : ICommand
{
    /// <summary>
    ///     If we were spend before. Needed to restore the state.
    /// </summary>
    private bool m_wasSpend;

    /// <inheritdoc />
    public void Execute()
    {
        m_wasSpend = state.ShootAtUnit(unit);
    }

    /// <inheritdoc />
    public void Undo()
    {
        state.UndoShotAtUnit(unit, m_wasSpend);
    }
}