using WargameExplainer.Strategy;
using WargameExplainer.TrainingSystem;

namespace WargameExplainer.SampleGames.TicTacToe.Training;

/// <summary>
///     Detects if the center point is occupied by the player perspective.
/// </summary>
/// <param name="playerPerspective">The perspective of the player (0 or 1).</param>
public class CenterPointCounter(int playerPerspective) : GameStateObserver
{

    public override ObserverCategory ObserverType => ObserverCategory.Continuous;

    public override string Interpretation => $"center points of the board is occupied by player {playerPerspective + 1}";

    public override bool IsControlledByPlayer(int perspective)
    {
        return playerPerspective == perspective;
    }

    public override float GetContinuousObservation(IHashableGameState gameState)
    {
        GameState localState = (GameState)gameState;
        if (playerPerspective == 0)
            return (localState.m_gameBoard[1, 1] == 1) ? 1.0f : 0.0f;

        return (localState.m_gameBoard[1, 1] == 2) ? 1.0f : 0.0f;
    }
}