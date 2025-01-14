using WargameExplainer.Explanation;
using WargameExplainer.Strategy;
using WargameExplainer.TrainingSystem;

namespace WargameExplainer.SampleGames.BettingGame.Training;

/// <summary>
///     The information for the training of the betting game.
/// </summary>
public class TrainingProviderBetting : TrainingInfoProvider
{
    public override IList<GameStateObserver> Observers { get; } =
    [
        new BettingRoundCounter(),
        new PlacesBetObserver(),
        new SuccessfullBetsCounter()
    ];

    public override int BatchSize => 1000;

    public override IGameOutcomeClassifier GetGameOutcomeClassifier()
    {
        return new BettingGameOutcomeClassifier();
    }

    public override StrategicDecider GetFreshGame(IHashableGameState? baseInfo)
    {
        BettingGameState newGame;
        if (baseInfo != null)
            newGame = (BettingGameState) baseInfo;
        else
            newGame = new BettingGameState();
        TrainableBettingEvaluator evaluator = new TrainableBettingEvaluator(this, newGame);
        if (LinearModel != null)
            evaluator.SetLinearModel(LinearModel);
        StrategicDecider strat = new StrategicDecider(newGame, 10, 3, evaluator, newGame);
        return strat;
    }
}