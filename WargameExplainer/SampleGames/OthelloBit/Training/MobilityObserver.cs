using WargameExplainer.Strategy;
using WargameExplainer.TrainingSystem;

namespace WargameExplainer.SampleGames.OthelloBit.Training;


/// <summary>
/// The observer for the mobility of the players.
/// </summary>
public class MobilityObserver : GameStateObserver
{
    public override ObserverCategory ObserverType => ObserverCategory.Continuous;

    public override string Interpretation => "Mobility difference of both players higher favors black";

    public override bool IsControlledByPlayer(int playerPerspective)
    {
        return true;
    }

    public override float GetContinuousObservation(IHashableGameState gameState)
    {
        BitBoardState localState = (BitBoardState)gameState;
        int numMovesBlack = localState.GetMobility(true);
        int numMovesWhite = localState.GetMobility(false);

        if (numMovesBlack + numMovesWhite == 0)
            return 0.5f;

        return ((numMovesBlack - numMovesWhite) * 0.5f / (numMovesBlack + numMovesWhite)) + 0.5f;
    }
}