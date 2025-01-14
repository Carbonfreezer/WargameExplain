using System.Diagnostics;
using WargameExplainer.Explanation;
using WargameExplainer.Strategy;

namespace WargameExplainer.TrainingSystem;

/// <summary>
///     General interface that has to be provided to generate the training data.
/// </summary>
public abstract class TrainingInfoProvider
{
    /// <summary>
    ///     The length of the feature vector we generate is stored here as a cache, as it never changes.
    /// </summary>
    private int m_cachedNumFeatures = -1;

    /// <summary>
    ///     The linear model that is iterated over the learning process.
    /// </summary>
    public LinearModel? LinearModel { get; set; }

    /// <summary>
    ///     Returns the list of observers used for the evaluation of the
    /// </summary>
    public abstract IList<GameStateObserver> Observers { get; }

    /// <summary>
    ///     Gets the number of input features the system generates.
    /// </summary>
    public int NumInputFeatures
    {
        get
        {
            if (m_cachedNumFeatures == -1)
                m_cachedNumFeatures = Observers.Select(observer =>
                    (observer.ObserverType == GameStateObserver.ObserverCategory.OneHotEncoded) ? observer.HighestObservedValue + 1 : 1).Sum();
            return m_cachedNumFeatures;
        }
    }

    /// <summary>
    ///     Asks for the required Batchsize;
    /// </summary>
    public abstract int BatchSize { get; }

    /// <summary>
    ///     Gets the regularization parameter.
    /// </summary>
    public virtual float L2RegularizationParameter => 0.02f;

    /// <summary>
    ///     The required epsilon value in epsilon greedy training.
    /// </summary>
    public virtual float EpsilonTraining => 0.3f;

    /// <summary>
    ///     Gets a new game selector, that is relevant for class
    /// </summary>
    /// <returns>Gets the game outcome classifier that belongs to that game.</returns>
    public abstract IGameOutcomeClassifier GetGameOutcomeClassifier();


    /// <summary>
    ///     Generates a strategy module for a fresh game for the given linear model if existant.
    /// </summary>
    /// <param name="baseInfo">This is eventual a common game state.</param>
    public abstract StrategicDecider GetFreshGame(IHashableGameState? baseInfo);


    /// <summary>
    ///     Generates and save the model.
    /// </summary>
    /// <param name="episodes">The list with the episodes we analyze.</param>
    /// <param name="modelName">The model name we save to.</param>
    public void GenerateAndSaveModel(IList<EpisodicRecord> episodes, string modelName)
    {
        RidgeModelFitter fitter = new RidgeModelFitter(this);
        LinearModel = fitter.GetRegularizedModel(episodes);
        LinearModel.SaveToFile(modelName);
    }


    /// <summary>
    ///     Given a game state and the weighting matrix (first dimension is player perspective, second is feature index), it
    ///     returns
    ///     an afterstate value vector being seen from each player.
    /// </summary>
    /// <param name="gameState">The gamestate we want to evaluate.</param>
    /// <param name="model">The linear model we use for evaluation.</param>
    /// <param name="situationEvaluation">The vector where we want to place the resulting elements in.</param>
    /// <returns>Evaluation vector to be seen from each player.</returns>
    public void GetSituationEvaluation(IHashableGameState gameState, LinearModel model,
        in Span<float> situationEvaluation)
    {
        Span<float> featureValues = stackalloc float[NumInputFeatures];
        GetObservedFeatureValues(gameState, featureValues);
        model.TransformInput(featureValues, situationEvaluation);
    }


    /// <summary>
    ///     Gets the float encoded vectors for the game state.
    /// </summary>
    /// <param name="gameState">Game state to analyze.</param>
    /// <param name="featureValues">The span into which we will add the input features.</param>
    /// <returns>The float encoded vector.</returns>
    public void GetObservedFeatureValues(IHashableGameState gameState, in Span<float> featureValues)
    {
        int arrayIndex = 0;

        (gameState as IObserverPreparer)?.PrepareObservations();
        foreach (GameStateObserver observer in Observers)
        {
            int gameObservation;
            float contObservation;
            switch (observer.ObserverType)
            {
                case GameStateObserver.ObserverCategory.Continuous:
                    contObservation = observer.GetContinuousObservation(gameState);
                    Debug.Assert((contObservation >= 0.0f) && (contObservation <= 1.0f), "Continuous observation not in [0.0, 1.0] range. ");
                    featureValues[arrayIndex++] = contObservation;
                    break;
                case GameStateObserver.ObserverCategory.BalancedContinuous:
                    contObservation = observer.GetContinuousObservation(gameState);
                    Debug.Assert((contObservation >= -1.0f) && (contObservation <= 1.0f), "Continuous observation not in [-1.0, 1.0] range. ");
                    featureValues[arrayIndex++] = contObservation * 0.5f; // Make deviation the same.
                    break;
                case GameStateObserver.ObserverCategory.Discreet:
                    gameObservation = observer.GetDiscreetObservation(gameState);
                    Debug.Assert((gameObservation >= 0) && (gameObservation <= observer.HighestObservedValue),
                        "Illegal observation");
                    featureValues[arrayIndex++] = ((float)gameObservation) / observer.HighestObservedValue;
                    break;
                case GameStateObserver.ObserverCategory.BalancedDiscreet:
                    gameObservation = observer.GetDiscreetObservation(gameState);
                    Debug.Assert((gameObservation >= -observer.HighestObservedValue) && (gameObservation <= observer.HighestObservedValue),
                        "Illegal observation");
                    // Make sure we have the same deviation then the discreet observations.
                    featureValues[arrayIndex++] = (0.5f * gameObservation) / observer.HighestObservedValue;
                    break;
                case GameStateObserver.ObserverCategory.OneHotEncoded:
                    gameObservation = observer.GetDiscreetObservation(gameState);
                    Debug.Assert((gameObservation >= 0) && (gameObservation <= observer.HighestObservedValue),
                        "Illegal observation");
                    for (int j = 0; j <= observer.HighestObservedValue; ++j)
                        featureValues[arrayIndex++] = (gameObservation == j ? 1.0f : 0.0f);
                    break;

            }
        }
    }

    /// <summary>
    ///     Gets the observed feature values as an array. Intended for the generation of an episodic entry.
    /// </summary>
    /// <param name="gameState">The game state to be analyzed.</param>
    /// <returns>The float array with the observed feature values.</returns>
    public float[] GetObservedFeatureValuesAsArray(IHashableGameState gameState)
    {
        Span<float> featureValues = stackalloc float[NumInputFeatures];
        GetObservedFeatureValues(gameState, featureValues);
        return featureValues.ToArray();
    }
}