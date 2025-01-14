using System.Diagnostics;
using WargameExplainer.DrawingSystem;
using WargameExplainer.Explanation;
using WargameExplainer.SampleGames.TakeThatHill.Training;
using WargameExplainer.Strategy;
using WargameExplainer.TrainingSystem;

namespace WargameExplainer.SampleGames.TakeThatHill;

/// <summary>
///     This is a test subsuite for take that hill, as it is more complex.
/// </summary>
public static class GameRunner
{
    /// <summary>
    ///     Execute the game once and takes snaphsots of the relevant stages.
    /// </summary>
    public static void RunGame()
    {
        TrainingProviderTakeThatHill trainer = new TrainingProviderTakeThatHill();
        LinearModel usedModel = new LinearModel();
        usedModel.LoadFromFile("DifferModel.mdl");
        trainer.LinearModel = usedModel;

        ModelInterpreter interpret = new ModelInterpreter(trainer);
        interpret.InterpretModel(usedModel);

        DebugPainter painter = new DebugPainter(1000, 600);


        using StrategicDecider strategicDecider = trainer.GetFreshGame(null);

        Span<float> values = stackalloc float[strategicDecider.NumOfPlayerPerspectives];
        painter.GenerateSnapshotSerial((IPaintable)strategicDecider.GameState);
        while (!strategicDecider.IsGameOverAndAfterStateValue(values))
        {
            strategicDecider.ExecuteNewMove();
            if (((IPaintable)strategicDecider.GameState).WantsScreenshot)
                painter.GenerateSnapshotSerial(((IPaintable)strategicDecider.GameState));
        }

        painter.GenerateSnapshotSerial(((IPaintable)strategicDecider.GameState));
        Console.WriteLine(values[0]);
    }

}