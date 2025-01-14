using WargameExplainer.Strategy;
using WargameExplainer.TrainingSystem;

namespace WargameExplainer.SampleGames.TakeThatHill.Training;

/// <summary>
///     Observes the total score shots taken and game round passed.
/// </summary>
public class TotalScoreObserver : GameStateObserver
{
    public override int HighestObservedValue => 17;
    public override ObserverCategory ObserverType => ObserverCategory.Discreet;
    public override string Interpretation => "Sum of shots taken and game round passed.";

    public override bool IsControlledByPlayer(int playerPerspective)
    {
        return true;
    }

    public override int GetDiscreetObservation(IHashableGameState gameState)
    {
        TakeThatHillGameState state = (TakeThatHillGameState)gameState;
        return state.ShotsTaken + state.GameRound;
    }
}