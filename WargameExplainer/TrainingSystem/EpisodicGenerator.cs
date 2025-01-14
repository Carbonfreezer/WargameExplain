using WargameExplainer.Strategy;

namespace WargameExplainer.TrainingSystem;

/// <summary>
///     Helper class that generates a long episodic buffer.
/// </summary>
public class EpisodicGenerator
{
  
    /// <summary>
    ///     The training info we generate the episodes with.
    /// </summary>
    private readonly TrainingInfoProvider m_trainingInfo;


    /// <summary>
    ///     Creates the training info from the training info provider.
    /// </summary>
    /// <param name="trainingInfo">The information provider we need for the </param>
    public EpisodicGenerator(TrainingInfoProvider trainingInfo)
    {
        m_trainingInfo = trainingInfo;
    }


    /// <summary>
    ///     Called to generate episodes, that may be used for learning.
    /// </summary>
    /// <param name="gamesToPlay">The amount of games we want to play.</param>
    /// <param name="epsilon">The epsilon we use in epsilon greedy.</param>
    /// <returns>List with episodes.</returns>
    public EpisodicRecord[] GenerateEpisodes(int gamesToPlay, float epsilon)
    {
        // The buffer with all the entries.
        EpisodicRecord[] result = new EpisodicRecord[gamesToPlay];

        for (int game = 0; game < gamesToPlay; ++game)
            result[game] = GenerateEpisode(epsilon);

        return result;
    }

    /// <summary>
    ///     Generates statistics for playing several games and returns for every game the afterstate value.
    /// </summary>
    /// <param name="gamesToPlay">How many games we want to play.</param>
    /// <returns>Array with afterstate values after the end of the game.</returns>
    public float[][] GetStatistics(int gamesToPlay)
    {
        float[][] result = new float[gamesToPlay] [];
        for (int game = 0; game < gamesToPlay; ++game)
            result[game] = GenerateResult();
      
        return result;
    }


    /// <summary>
    ///     Plays the game with an epsilon greedy strategy and returns the final afterstate.
    /// </summary>
    /// <returns>Aftertstate value.</returns>
    public float[] GenerateResult()
    {
        using StrategicDecider strategicDecider = m_trainingInfo.GetFreshGame(null);

        float[] afterStateValue = new float[strategicDecider.NumOfPlayerPerspectives];

        while (!strategicDecider.IsGameOverAndAfterStateValue(afterStateValue))
            strategicDecider.ExecuteNewMove();

        return afterStateValue;
    }


    /// <summary>
    ///     Helper method to generate the episodes for one single playthrough. Also used explicitely on the client side.
    /// </summary>
    /// <param name="epsilon">The epsilon in the epsilon greedy algorithm,</param>
    /// <returns>Accumulated episodes.</returns>
    public EpisodicRecord GenerateEpisode(float epsilon)
    {
        using StrategicDecider strategicDecider = m_trainingInfo.GetFreshGame(null);
        EpisodicRecord resultRecord = new EpisodicRecord();

        float[] afterStateValue = new float[strategicDecider.NumOfPlayerPerspectives];

        while (!strategicDecider.IsGameOverAndAfterStateValue(afterStateValue))
        {
            strategicDecider.ExecuteMoveEpsilonGreedy(epsilon);
            resultRecord.ObservedFeatureValues.Add(m_trainingInfo.GetObservedFeatureValuesAsArray(strategicDecider.GameState));
        }

        resultRecord.TargetValues = afterStateValue;

        return resultRecord;
    }
}