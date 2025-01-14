using WargameExplainer.Explanation;
using WargameExplainer.Strategy;
using WargameExplainer.TrainingSystem;

namespace WargameExplainer.SampleGames.TakeThatHill.Training;

/// <summary>
///     The training info provider for the night mode.
/// </summary>
public class TrainingProviderTakeThatHillNight : TrainingInfoProvider
{
    public override IList<GameStateObserver> Observers { get; } =
    [
        new LightRoundObserver(),
        new TotalScoreObserver(),
        new DistanceObserver(0),
        new DistanceObserver(1),
        new DistanceObserver(2)
    ];

    public override int BatchSize => 3000;

    public override float L2RegularizationParameter => 0.0f;

    public override IGameOutcomeClassifier GetGameOutcomeClassifier()
    {
        return new TakeThatHillGameOutcomeClassifier();
    }

    public override StrategicDecider GetFreshGame(IHashableGameState? baseInfo)
    {
        TakeThatHillGameState newGame;
        if (baseInfo != null)
            newGame = (TakeThatHillGameState)baseInfo;
        else
            newGame = new TakeThatHillGameState(true);


        TrainableTakeThatHillEvaluator evaluator = new TrainableTakeThatHillEvaluator(this, newGame);
        if (LinearModel != null)
            evaluator.SetLinearModel(LinearModel);
        ActionDecider decider = new ActionDecider(newGame);
        StrategicDecider strategicDecider = new StrategicDecider(newGame, 6000, 35, evaluator, decider);

        return strategicDecider;
    }
}