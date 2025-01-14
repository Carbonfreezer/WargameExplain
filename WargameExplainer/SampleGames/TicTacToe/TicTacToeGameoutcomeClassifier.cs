﻿using System.Diagnostics;
using WargameExplainer.Explanation;

namespace WargameExplainer.SampleGames.TicTacToe;

public class TicTacToeGameoutcomeClassifier : IGameOutcomeClassifier
{
    public int NumOfCategories => 3;

    public int GetCategory(IList<float> evaluation)
    {
        if (evaluation[0] < -0.9f)
            return 0;
        if (evaluation[0] > 0.9f)
            return 2;

        return 1;
    }

    public string GetDescription(int category)
    {
        switch (category)
        {
            case 0:
                return "Lost";
            case 1:
                return "Draw";
            case 2:
                return "Won";
        }

        Debug.Assert(false, "Should not happen");
        return "";
    }
}