namespace WargameExplainer.Strategy;

/// <summary>
///     Combines several commands into one.
/// </summary>
public class CommandCompound : ICommand
{
    /// <summary>
    ///     Internal list with commands.
    /// </summary>
    private readonly IList<ICommand> m_subCommands;


    /// <summary>
    ///     Empty constructor.
    /// </summary>
    public CommandCompound()
    {
        m_subCommands = new List<ICommand>();
    }

    /// <summary>
    ///     Constructor from a list of commands.
    /// </summary>
    /// <param name="listOfCommands">The list of commands we want to have.</param>
    public CommandCompound(IList<ICommand> listOfCommands)
    {
        m_subCommands = listOfCommands;
    }

    /// <inheritdoc />
    public void Execute()
    {
        for(int i = 0; i < m_subCommands.Count; ++i)
            m_subCommands[i].Execute();
    }

    /// <inheritdoc />
    public void Undo()
    {
        for(int i = m_subCommands.Count - 1; i >= 0; --i)
            m_subCommands[i].Undo();
    }
}