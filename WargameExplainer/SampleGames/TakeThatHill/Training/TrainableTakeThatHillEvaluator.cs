using WargameExplainer.TrainingSystem;

namespace WargameExplainer.SampleGames.TakeThatHill.Training;

public class TrainableTakeThatHillEvaluator(TrainingInfoProvider trainingInfo, TakeThatHillGameState gameState)
    : TrainableEvaluator(trainingInfo, gameState)
{
    public override int NumOfPlayerPerspectives => 1;

    protected override bool InternallyReevaluate(in Span<float> afterStateValues)
    {
        int totalScore = gameState.GameRound + gameState.ShotsTaken;
        if (totalScore >= 16)
        {
            afterStateValues[0] = -1.0f;
            return true;
        }

        int minDistance = Math.Min(gameState.GetDistanceToUnit(0), gameState.GetDistanceToUnit(1));
        minDistance = Math.Min(minDistance, gameState.GetDistanceToUnit(2));

        if (minDistance == 0)
        {
            if (totalScore < 11)
                afterStateValues[0] = 1.0f;
            else
                afterStateValues[0] = 0.0f;
            return true;
        }


        float firstGuess = ((6.0f - minDistance) * 3.0f - totalScore) / 36.0f;
        if ((totalScore >= 10) && (firstGuess > 0.0f))
            firstGuess = 0.0f;
        afterStateValues[0] = firstGuess;

        return false;
    }
}