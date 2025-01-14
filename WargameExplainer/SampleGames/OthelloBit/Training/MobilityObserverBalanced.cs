using WargameExplainer.Strategy;
using WargameExplainer.TrainingSystem;

namespace WargameExplainer.SampleGames.OthelloBit.Training;

/// <summary>
///     The observer for the mobility in a balanced fashion.
/// </summary>
/// <seealso cref="MobilityObserver" />
public class MobilityObserverBalanced : GameStateObserver
{
    public override ObserverCategory ObserverType => ObserverCategory.BalancedContinuous;

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
            return 0.0f;

        return ((float)(numMovesBlack - numMovesWhite)  / (numMovesBlack + numMovesWhite));
    }
}