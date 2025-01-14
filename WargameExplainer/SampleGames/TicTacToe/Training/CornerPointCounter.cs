using WargameExplainer.Strategy;
using WargameExplainer.TrainingSystem;

namespace WargameExplainer.SampleGames.TicTacToe.Training;

/// <summary>
///     Detects the number of corner points occupied by player perspective
/// </summary>
/// <param name="playerPerspective">The perspective of the player (0 or 1).</param>
public class CornerPointCounter(int playerPerspective) : GameStateObserver
{
    /// <summary>
    ///     None or 4 corner points.
    /// </summary>
    public override int HighestObservedValue => 4;

    public override ObserverCategory ObserverType => ObserverCategory.Discreet;

    public override string Interpretation => $"corner points of board are occupied by player {playerPerspective + 1}";

    public override bool IsControlledByPlayer(int perspective)
    {
        return playerPerspective == perspective;
    }

    public override int GetDiscreetObservation(IHashableGameState gameState)
    {
        GameState localState = (GameState)gameState;
        int detectField = playerPerspective + 1;
        int sum = 0;

        if (localState.m_gameBoard[0, 0] == detectField)
            sum += 1;
        if (localState.m_gameBoard[0, 2] == detectField)
            sum += 1;
        if (localState.m_gameBoard[2, 0] == detectField)
            sum += 1;
        if (localState.m_gameBoard[2, 2] == detectField)
            sum += 1;

        return sum;
    }
}