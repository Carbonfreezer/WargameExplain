using WargameExplainer.TrainingSystem;

namespace WargameExplainer.SampleGames.TicTacToe.Training;

/// <summary>
///     Trainable tic tac toe wrapper.
/// </summary>
/// <param name="trainingInfo">The training info.</param>
/// <param name="gameState">The game state we use.</param>
public class TrainableTicTacToeEvaluator(TrainingInfoProvider trainingInfo, GameState gameState)
    : TrainableEvaluator(trainingInfo, gameState)
{
    public override int NumOfPlayerPerspectives => gameState.NumOfPlayerPerspectives;
    protected override bool InternallyReevaluate(in Span<float> afterStateValues)
    {
        return gameState.IsGameOverAndEvaluate(afterStateValues);
    }

}