using WargameExplainer.Strategy;

namespace WargameExplainer.SampleGames.BettingGame;

/// <summary>
///     Command for the random decider that treats the effect that a bet has been lost / won.
/// </summary>
/// <param name="game">The game to manipulate</param>
/// <param name="hasWon">Flag if we have won the game.</param>
public readonly struct WonCommand(BettingGameState game, bool hasWon) : ICommand
{
    /// <inheritdoc />
    public void Execute()
    {
        if (hasWon)
            game.m_successesWon += 1;
        game.m_bettingRound += 1;
        game.m_waitingForEvaluation = false;
    }

    /// <inheritdoc />
    public void Undo()
    {
        if (hasWon)
            game.m_successesWon -= 1;
        game.m_bettingRound -= 1;
        game.m_waitingForEvaluation = true;
    }
}

/// <summary>
///     The command to place a bet.
/// </summary>
/// <param name="game">The game we place a bet on.</param>
/// <param name="slot">The slot we bet</param>
public readonly struct BetCommand(BettingGameState game, int slot) : ICommand
{
    /// <inheritdoc />
    public void Execute()
    {
        game.m_betPlaced = slot;
        game.m_waitingForEvaluation = true;
    }

    /// <inheritdoc />
    public void Undo()
    {
        game.m_waitingForEvaluation = false;
    }
}