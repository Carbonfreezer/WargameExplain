using WargameExplainer.Explanation;

namespace WargameExplainer.SampleGames.TakeThatHill;

/// <summary>
///     The interface to generate the example based explanations for take that hill.
/// </summary>
public class TakeThatHillGameOutcomeClassifierDefensive : IGameOutcomeClassifier
{
    /// <summary>
    ///     Game outcome
    /// </summary>
    public int NumOfCategories => 11;

    public int GetCategory(IList<float> evaluation)
    {
        return (int)MathF.Round(-evaluation[0] * 10);
    }

    public string GetDescription(int category)
    {
        return $"Num of shots taken around {category}";
    }
}