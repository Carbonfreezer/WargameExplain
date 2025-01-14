using WargameExplainer.Explanation;
using WargameExplainer.Strategy;
using WargameExplainer.TrainingSystem;

namespace WargameExplainer.SampleGames.TakeThatHill.Training;


/// <summary>
/// Training info provider for defensive interpretation of take that hill (= only shots taken matter).
/// </summary>
public class TrainingProviderTakeThatHillDefensive : TrainingInfoProvider
{
    public override IList<GameStateObserver> Observers { get; } =
    [
        new HitObserver(),
        // new RoundObserver(),
        new DistanceObserver(0),
        new DistanceObserver(1),
        new DistanceObserver(2)
    ];

    public override int BatchSize => 3000;

    public override float L2RegularizationParameter => 0.0f;

    public override IGameOutcomeClassifier GetGameOutcomeClassifier()
    {
        return new TakeThatHillGameOutcomeClassifierDefensive();
    }

    public override StrategicDecider GetFreshGame(IHashableGameState? baseInfo)
    {
        TakeThatHillGameState newGame;
        if (baseInfo != null)
            newGame = (TakeThatHillGameState)baseInfo;
        else
            newGame = new TakeThatHillGameState(false);


        TrainableDefensiveTakeThatHillEvaluator evaluator = new TrainableDefensiveTakeThatHillEvaluator(this, newGame);
        if (LinearModel != null)
            evaluator.SetLinearModel(LinearModel);
        ActionDecider decider = new ActionDecider(newGame);
        StrategicDecider strategicDecider = new StrategicDecider(newGame, 6000, 35, evaluator, decider);

        return strategicDecider;
    }
}