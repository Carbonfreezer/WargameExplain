using WargameExplainer.Strategy;
using WargameExplainer.TrainingSystem;

namespace WargameExplainer.SampleGames.TakeThatHill.Training;


/// <summary>
/// Looks for the amount of hits taken. Maxes out at 10.
/// </summary>
public class HitObserver : GameStateObserver
{
    public override ObserverCategory ObserverType => ObserverCategory.Discreet;

    public override int HighestObservedValue => 10;

    public override string Interpretation => "The amount of hits taken with a maximum of 10.";

    public override bool IsControlledByPlayer(int playerPerspective)
    {
        return true;
    }

    public override int GetDiscreetObservation(IHashableGameState gameState)
    {
        TakeThatHillGameState state = (TakeThatHillGameState)gameState;
        return Math.Min(state.ShotsTaken, 10);
    }
}