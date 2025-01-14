using WargameExplainer.Strategy;
using WargameExplainer.TrainingSystem;

namespace WargameExplainer.SampleGames.BettingGame.Training;

/// <summary>
///     Observer for how many rounds are already passed.
/// </summary>
public class BettingRoundCounter : GameStateObserver
{
    public override int HighestObservedValue => 10;
    public override ObserverCategory ObserverType => ObserverCategory.Discreet;

    public override string Interpretation => "rounds already passed in the game";

    public override bool IsControlledByPlayer(int playerPerspective)
    {
        return false;
    }

    public override int GetDiscreetObservation(IHashableGameState gameState)
    {
        return ((BettingGameState)gameState).m_bettingRound;
    }
}