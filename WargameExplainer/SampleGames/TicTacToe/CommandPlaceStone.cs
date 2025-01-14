using WargameExplainer.Strategy;

namespace WargameExplainer.SampleGames.TicTacToe;

/// <summary>
///     Command to place a stone on the tic tact toe board.
/// </summary>
/// <param name="x">x coordinate of the stone.</param>
/// <param name="y">y coordinate of the stone.</param>
/// <param name="player">The player who places the stone.</param>
/// <param name="game">The game to place a stone on.</param>
public readonly struct CommandPlaceStone(int x, int y, int player, GameState game) : ICommand
{
    public void Execute()
    {
        game.PlaceStone(player, x, y);
    }

    public void Undo()
    {
        game.RevokeStone(x, y);
    }
}