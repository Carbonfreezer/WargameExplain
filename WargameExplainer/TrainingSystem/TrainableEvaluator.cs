using WargameExplainer.Strategy;

namespace WargameExplainer.TrainingSystem;

/// <summary>
///     This is a general trainable evaluator once can derive from. Essentially it works like a directly programmable
///     evaluator,
///     where a provided linear model can be used to override the situation evaluation.
/// </summary>
/// <param name="trainingInfo">The training info struct to analyze the situation.</param>
/// <param name="gameState">The hashable game state to be analyzed.</param>
public abstract class TrainableEvaluator(TrainingInfoProvider trainingInfo, IHashableGameState gameState)
    : IEvaluator
{
    /// <summary>
    ///     Stores the internal hashable for situation evaluation.
    /// </summary>
    private readonly IHashableGameState m_gameState = gameState;

    /// <summary>
    ///     The linear model we use to compute the afterstate value.
    /// </summary>
    private LinearModel? m_containedModel;


    public abstract int NumOfPlayerPerspectives { get; }

    public bool IsGameOverAndEvaluate(in Span<float> afterStateValues)
    {
        bool isGameOver = InternallyReevaluate(afterStateValues);

        if (isGameOver || (m_containedModel == null))
            return isGameOver;

        trainingInfo.GetSituationEvaluation(m_gameState, m_containedModel, afterStateValues);
        return false;
    }


    /// <summary>
    ///     Provides the linear model from the outside.
    /// </summary>
    /// <param name="model">The model we use.</param>
    public void SetLinearModel(LinearModel model)
    {
        m_containedModel = model;
    }

    /// <summary>
    ///     Does the internal reevaluation. This is always called also for the game end detection.
    ///     The result of the internal evaluation will only be overriden, if a linear model is known and we are not in a game
    ///     end situation.
    /// </summary>
    protected abstract bool InternallyReevaluate(in Span<float> afterStateValues);
}