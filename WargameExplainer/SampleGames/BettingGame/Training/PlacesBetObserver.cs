using WargameExplainer.Strategy;
using WargameExplainer.TrainingSystem;

namespace WargameExplainer.SampleGames.BettingGame.Training;

/// <summary>
///     Observes, which bet we have placed.
/// </summary>
public class PlacesBetObserver : GameStateObserver
{
    public override int HighestObservedValue => 2;
    public override ObserverCategory ObserverType => ObserverCategory.OneHotEncoded;

    public override string Interpretation => "placed betting type";

    public override bool IsControlledByPlayer(int playerPerspective)
    {
        return true;
    }

    public override int GetDiscreetObservation(IHashableGameState gameState)
    {
        return ((BettingGameState)gameState).m_betPlaced;
    }
}