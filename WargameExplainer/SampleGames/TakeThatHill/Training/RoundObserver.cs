using WargameExplainer.Strategy;
using WargameExplainer.TrainingSystem;

namespace WargameExplainer.SampleGames.TakeThatHill.Training;

public class RoundObserver : GameStateObserver
{
    public override int HighestObservedValue => 20;
    public override ObserverCategory ObserverType => ObserverCategory.Discreet;
    public override string Interpretation => "The amount of rounds passed since the beginning out of 20";

    public override bool IsControlledByPlayer(int playerPerspective)
    {
        return true;
    }

    public override int GetDiscreetObservation(IHashableGameState gameState)
    {
        TakeThatHillGameState state = (TakeThatHillGameState)gameState;
        return Math.Min(state.GameRound, 20);
    }
}