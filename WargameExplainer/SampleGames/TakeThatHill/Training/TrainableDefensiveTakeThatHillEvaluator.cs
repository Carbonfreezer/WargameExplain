using WargameExplainer.TrainingSystem;

namespace WargameExplainer.SampleGames.TakeThatHill.Training;


/// <summary>
/// Evaluator for the defensive version of take that hill, where the storming group wants to take as few hits as possible.
/// </summary>
/// <param name="trainingInfo">The training info provider we use.</param>
/// <param name="gameState">The game state we are in.</param>
public class TrainableDefensiveTakeThatHillEvaluator(TrainingInfoProvider trainingInfo, TakeThatHillGameState gameState)
    : TrainableEvaluator(trainingInfo, gameState)
{
    public override int NumOfPlayerPerspectives => 1;

    protected override bool InternallyReevaluate(in Span<float> afterStateValues)
    {
        if (gameState.ShotsTaken >= 10)
        {
            afterStateValues[0] = -1.0f;
            return true;
        }

        int minDistance = Math.Min(gameState.GetDistanceToUnit(0), gameState.GetDistanceToUnit(1));
        minDistance = Math.Min(minDistance, gameState.GetDistanceToUnit(2));

        if (minDistance == 0)
        {
            afterStateValues[0] = -gameState.ShotsTaken / 10.0f;
            return true;
        }

        afterStateValues[0] = -gameState.ShotsTaken / 10.0f - minDistance / 6.0f;
        return false;
    }
}
