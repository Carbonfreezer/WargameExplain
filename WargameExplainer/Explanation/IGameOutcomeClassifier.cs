namespace WargameExplainer.Explanation;

/// <summary>
///     Interface for the explainer to classify the game outcome. This is used for the sample episodes and for classifying
///     the game outcome in the statistics.
/// </summary>
public interface IGameOutcomeClassifier
{
    /// <summary>
    ///     The amount of categories we support.
    /// </summary>
    int NumOfCategories { get; }

    /// <summary>
    ///     The category that belongs to a certain final game evaluation.
    /// </summary>
    /// <param name="evaluation">The evaluation parameter that was done at the end of the game.</param>
    /// <returns>Chosen category should correspond to num of categories.</returns>
    /// <seealso cref="NumOfCategories" />
    int GetCategory(IList<float> evaluation);

    /// <summary>
    ///     Gets a shot description for the category, that is used as a prefix of the filename.
    /// </summary>
    /// <param name="category">Category index</param>
    /// <returns>Description string.</returns>
    string GetDescription(int category);
}