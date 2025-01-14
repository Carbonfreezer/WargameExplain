using WargameExplainer.Explanation;
using WargameExplainer.Strategy;
using WargameExplainer.TrainingSystem;

namespace WargameExplainer.SampleGames.OthelloBit.Training;

public class TrainingProviderOthelloBit : TrainingInfoProvider
{
    public override IList<GameStateObserver> Observers => 
        [
            new DiagonalObserver(true, 0),
            new DiagonalObserver(true, 1),
            new DiagonalObserver(false,0),
            new DiagonalObserver(false, 1),
            new SidePointObserver(true, 1),
            new SidePointObserver(true, 2),
            new SidePointObserver(true, 3),
            new SidePointObserver(false, 1),
            new SidePointObserver(false, 2),
            new SidePointObserver(false, 3),
            new InnerPointObserver(true),
            new InnerPointObserver(false),
            new MobilityObserver()
        ];
    public override int BatchSize => 1000; // 10000; //  100000;
    public override float L2RegularizationParameter => 0.0f;

    public override IGameOutcomeClassifier GetGameOutcomeClassifier()
    {
        // We can use the same game outcome classifier as for the othello game.
        return new OthelloGameOutcomeClassifier();
    }

    public override StrategicDecider GetFreshGame(IHashableGameState? baseInfo)
    {
        BitBoardState gameState;
        if (baseInfo != null)
            gameState = (BitBoardState)baseInfo;
        else
            gameState = new BitBoardState();
        TrainableOthelloBitEvaluator evaluator = new TrainableOthelloBitEvaluator(this, gameState);
        if (LinearModel != null)
            evaluator.SetLinearModel(LinearModel);
        StrategicDecider decider = new StrategicDecider(gameState, 10000, 5, evaluator, gameState);
        return decider;
    }
}