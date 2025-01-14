using WargameExplainer.Strategy;
using WargameExplainer.TrainingSystem;

namespace WargameExplainer.SampleGames.TakeThatHill.Training;


/// <summary>
/// Observer element for the illumination of the game round.
/// </summary>
public class LightRoundObserver : GameStateObserver
{
    public override ObserverCategory ObserverType => ObserverCategory.OneHotEncoded;

    public override int HighestObservedValue => 10;

    public override string Interpretation => "Chosen game round for offender illumination";

    public override int GetDiscreetObservation(IHashableGameState gameState)
    {
        int round = ((TakeThatHillGameState)gameState).IlluminationOffenderRound;
        if (round < 0)
            round = 0;
        return round;
    }

    public override bool IsControlledByPlayer(int playerPerspective)
    {
        return true;
    }
}