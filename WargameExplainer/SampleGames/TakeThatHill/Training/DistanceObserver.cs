using WargameExplainer.Strategy;
using WargameExplainer.TrainingSystem;

namespace WargameExplainer.SampleGames.TakeThatHill.Training;

/// <summary>
///     The observer on how far we are away from the defender.
/// </summary>
/// <param name="rankNumber">Number of the unit from 0: closest unit 1: second closest and 2 last..</param>
public class DistanceObserver(int rankNumber) : GameStateObserver
{
    public override int HighestObservedValue => 5;
    public override ObserverCategory ObserverType => ObserverCategory.Discreet;
    public override string Interpretation => $"Unit beeing closest {rankNumber + 1}. distance to the defender";

    public override bool IsControlledByPlayer(int playerPerspective)
    {
        return true;
    }

    public override int GetDiscreetObservation(IHashableGameState gameState)
    {
        TakeThatHillGameState state = (TakeThatHillGameState)gameState;
        return state.GetDistanceOfClosestUnit(rankNumber);
    }
}