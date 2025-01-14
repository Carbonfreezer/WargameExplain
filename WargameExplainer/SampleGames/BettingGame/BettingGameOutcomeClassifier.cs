using System.Diagnostics;
using WargameExplainer.Explanation;

namespace WargameExplainer.SampleGames.BettingGame;

/// <summary>
///     Outcome classifier for the betting game.
/// </summary>
public class BettingGameOutcomeClassifier : IGameOutcomeClassifier
{
    public int NumOfCategories => 2;

    public int GetCategory(IList<float> evaluation)
    {
        if (evaluation[0] < -0.9f)
            return 0;

        return 1;
    }

    public string GetDescription(int category)
    {
        switch (category)
        {
            case 0:
                return "Lost";
            case 1:
                return "Won";
        }

        Debug.Assert(false, "Should not happen");
        return "";
    }
}