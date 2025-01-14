using WargameExplainer.Strategy;
using WargameExplainer.TrainingSystem;

namespace WargameExplainer.SampleGames.BettingGame.Training;

/// <summary>
///     Observer to count the successes in betting.
/// </summary>
public class SuccessfullBetsCounter : GameStateObserver
{
    public  override int HighestObservedValue => 3;
    public override ObserverCategory ObserverType => ObserverCategory.Discreet;
    public bool WantsOneHotEncoding => false;
    public override string Interpretation => "successfully placed bets";

    public override bool IsControlledByPlayer(int playerPerspective)
    {
        return false;
    }

    public override int GetDiscreetObservation(IHashableGameState gameState)
    {
        return ((BettingGameState)gameState).m_successesWon;
    }
}