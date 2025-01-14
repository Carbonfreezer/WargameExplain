using System.Diagnostics;
using WargameExplainer.Explanation;

namespace WargameExplainer.SampleGames.OthelloBit;

public class OthelloGameOutcomeClassifier : IGameOutcomeClassifier
{
    /// <summary>
    ///     win, draw, loss
    /// </summary>
    public int NumOfCategories => 3;


    /// <summary>
    ///     Return Olettho specific result from final afterstate values.
    /// </summary>
    /// <param name="evaluation">Final afterstate values</param>
    /// <returns>Olettho specific result</returns>
    public int GetCategory(IList<float> evaluation)
    {
        if (evaluation[0] < -0.9f) // white player wins
            return 0;
        if (evaluation[0] > 0.9f) // black player wins
            return 1;

        return 2; // draw
    }


    /// <summary>
    ///     Give a description for the game outcome.
    /// </summary>
    /// <param name="category">Game result.</param>
    /// <returns>Description.</returns>
    public string GetDescription(int category)
    {
        switch (category)
        {
            case 0:
                return "Player with white stones wins";
            case 2:
                return "Draw";
            case 1:
                return "Player with black stones wins";
        }

        Debug.Assert(false, "Should not  happen");
        return "";
    }
}