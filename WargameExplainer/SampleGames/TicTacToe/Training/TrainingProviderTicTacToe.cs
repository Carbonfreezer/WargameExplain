using WargameExplainer.Explanation;
using WargameExplainer.SampleGames.TakeThatHill;
using WargameExplainer.Strategy;
using WargameExplainer.TrainingSystem;

namespace WargameExplainer.SampleGames.TicTacToe.Training;

/// <summary>
///     Generates the training info for Tic Tac Toe.
/// </summary>
public class TrainingProviderTicTacToe : TrainingInfoProvider
{
    public override IList<GameStateObserver> Observers { get; } =
    [
        new CenterPointCounter(0),
        new CenterPointCounter(1),
        new CornerPointCounter(0),
        new CornerPointCounter(1),
        new SidePointCounter(0),
        new SidePointCounter(1)
    ];

    public override int BatchSize => 10000;

    public override float L2RegularizationParameter => 0.0f;

    public override IGameOutcomeClassifier GetGameOutcomeClassifier()
    {
        return new TicTacToeGameoutcomeClassifier();
    }

    public override StrategicDecider GetFreshGame(IHashableGameState? baseInfo)
    {
        GameState newGame;
        if (baseInfo != null)
            newGame = (GameState)baseInfo;
        else
            newGame = new GameState();

        TrainableTicTacToeEvaluator evaluator = new TrainableTicTacToeEvaluator(this, newGame);
        if (LinearModel != null)
            evaluator.SetLinearModel(LinearModel);
        StrategicDecider strat = new StrategicDecider(newGame, 50, 3, evaluator, newGame);
        return strat;
    }
}