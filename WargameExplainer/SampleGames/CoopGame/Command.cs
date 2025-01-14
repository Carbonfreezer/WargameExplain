using WargameExplainer.Strategy;

namespace WargameExplainer.SampleGames.CoopGame;

/// <summary>
/// Command to or not return the money handed over.
/// </summary>
/// <param name="gameState">Game state we refer to.</param>
/// <param name="hasReturned">Indicates whether we want to return money or not.</param>
public readonly struct ReturnCommand(CoopGamestate gameState, bool hasReturned) : ICommand
{
    public void Execute()
    {
        if (hasReturned)
            gameState.m_moneyReturned[gameState.m_currentPlayer] = hasReturned;
        gameState.m_currentPlayer += 1;
    }

    public void Undo()
    {
        gameState.m_currentPlayer -= 1;
        gameState.m_moneyReturned[gameState.m_currentPlayer] = false;
    }
}


/// <summary>
/// Indicates if we want to vote for a common law to return the money.
/// </summary>
/// <param name="gameState">Game state we refer to.</param>
/// <param name="hasVoted">Indicates if we have voted for that.</param>
public readonly struct VotingCommand(CoopGamestate gameState, bool hasVoted) : ICommand
{
    public void Execute()
    {
        if (hasVoted)
            gameState.m_votesCollected += 1;
        gameState.m_currentPlayer += 1;

        if (gameState.m_currentPlayer >= CoopGamestate.NumOfPlayers)
        {
            gameState.m_currentPlayer = 0;
            gameState.m_isVoting = false;
        }
    }

    public void Undo()
    {
        if (hasVoted)
            gameState.m_votesCollected -= 1;

        if (!gameState.m_isVoting)
        {
            gameState.m_currentPlayer = CoopGamestate.NumOfPlayers - 1;
            gameState.m_isVoting = true;
        }
        else
        {
            gameState.m_currentPlayer -= 1;
        }
    }
}