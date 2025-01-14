using WargameExplainer.TrainingSystem;

namespace WargameExplainer.SampleGames.BettingGame.Training;

/// <summary>
///     The internal trainable evaluator for the betting game.
/// </summary>
/// <param name="trainingInfo">The training info provider.</param>
/// <param name="gameState">The game state we refer to.</param>
public class TrainableBettingEvaluator(TrainingInfoProvider trainingInfo, BettingGameState gameState)
    : TrainableEvaluator(trainingInfo, gameState)
{
    public override int NumOfPlayerPerspectives => gameState.NumOfPlayerPerspectives;
    protected override bool InternallyReevaluate(in Span<float> afterStateValues)
    {
        return gameState.IsGameOverAndEvaluate(afterStateValues);
    }
}