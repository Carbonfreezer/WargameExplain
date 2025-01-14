using System.Diagnostics;
using WargameExplainer.DrawingSystem;
using WargameExplainer.Strategy;
using WargameExplainer.TrainingSystem;

namespace WargameExplainer.Explanation;

/// <summary>
///     The example explainer generates examples from the game by playing the game several times and picking the most
///     extreme games according to the
///     TakeThatHillGameOutcomeClassifier handed over. In the end paintings of the most extreme games are generated.
/// </summary>
public class ExampleExplainer : IDisposable
{
    /// <summary>
    ///     The game selector interface to pick which games to keep.
    /// </summary>
    private readonly IGameOutcomeClassifier m_gameOutcomeClassifier;

    /// <summary>
    ///     The training information system to generate new games.
    /// </summary>
    private readonly TrainingInfoProvider m_infoSystem;

    /// <summary>
    ///     The painter to generate the images.
    /// </summary>
    private readonly DebugPainter m_painter;

    /// <summary>
    ///     The example record we generate (first dimension is the category, second is minimum and maximum of moves).
    /// </summary>
    private ExampleRecord[,]? m_storedExamples;

    /// <summary>
    ///     Generates the example explainer.
    /// </summary>
    /// <param name="infoSystem">Is used to generate new game setups.</param>
    /// <param name="gameOutcomeClassifier">The selector to determine which games should be kept as an explanation.</param>
    /// <param name="imageWidth">The width of the image we want to generate.</param>
    /// <param name="imageHeight">The height of the image we want to generate.</param>
    public ExampleExplainer(TrainingInfoProvider infoSystem, IGameOutcomeClassifier gameOutcomeClassifier, int imageWidth,
        int imageHeight)
    {
        m_infoSystem = infoSystem;
        m_gameOutcomeClassifier = gameOutcomeClassifier;
        m_painter = new DebugPainter(imageWidth, imageHeight);
    }

    /// <summary>
    ///     Removes the resources of the debug painter.
    /// </summary>
    public void Dispose()
    {
        m_painter.Dispose();
    }


    /// <summary>
    ///     Generates an example explanation for the used model.
    /// </summary>
    /// <param name="numOfGames">The number of games to play.</param>
    public void GenerateExampleExplanation(int numOfGames)
    {
        GenerateExampleRecords(numOfGames);
        PlayExampleRecords();
    }


    /// <summary>
    ///     Replays the finally stored examples and
    /// </summary>
    private void PlayExampleRecords()
    {
        Debug.Assert(m_storedExamples != null, "Should have been completed already");
        for (int category = 0; category < m_gameOutcomeClassifier.NumOfCategories; ++category)
        for (int barrier = 0; barrier < 2; ++barrier)
        {
            ExampleRecord usedRecord = m_storedExamples[category, barrier];

            // In this case, the constellation did not appear in any playthrough.
            if (usedRecord == null)
                continue;

            if (usedRecord.m_paintableGameState == null)
                continue;

            // AS the stored game in the IPaintable is still in the final stage, we first need to undo all operations.
            for (int i = usedRecord.m_excutedCommands.Count - 1; i >= 0; --i)
                usedRecord.m_excutedCommands[i].Undo();

            string header = $"{m_gameOutcomeClassifier.GetDescription(category)}_{(barrier == 0 ? "minMove" : "maxMove")}_";

            m_painter.GenerateSnapShot(header + "0.png", usedRecord.m_paintableGameState);
            int counter = 1;
            foreach (ICommand command in usedRecord.m_excutedCommands)
            {
                command.Execute();
                if (!usedRecord.m_paintableGameState.WantsScreenshot)
                    continue;
                m_painter.GenerateSnapShot(header + $"{counter}.png", usedRecord.m_paintableGameState);
                counter++;
            }

            m_painter.GenerateSnapShot(header + $"{counter}.png", usedRecord.m_paintableGameState);
        }

        // Can be garbage collected now.
        m_storedExamples = null;
    }


    /// <summary>
    ///     Generates example records, that can be played back later.
    /// </summary>
    /// <param name="numOfGames">The number of games we play for example generation.</param>
    private void GenerateExampleRecords(int numOfGames)
    {
        m_storedExamples = new ExampleRecord[m_gameOutcomeClassifier.NumOfCategories, 2];
        StrategicDecider dummy = m_infoSystem.GetFreshGame(null);
        Span<float> values = stackalloc float[dummy.NumOfPlayerPerspectives];
        dummy.Dispose();

        // Play the games and update the records.
        for (int game = 0; game < numOfGames; ++game)
        {
            using StrategicDecider decider = m_infoSystem.GetFreshGame(null);
            ExampleRecord record = new ExampleRecord();


            while (!decider.IsGameOverAndAfterStateValue(values))
                record.m_excutedCommands.Add(decider.ExecuteNewMove());

            record.m_finalEvaluation = values.ToArray();
            record.m_paintableGameState = (IPaintable)decider.GameState;

            int category = m_gameOutcomeClassifier.GetCategory(record.m_finalEvaluation);

            if (m_storedExamples[category, 0] == null)
            {
                // This is our first entry.
                m_storedExamples[category, 0] = record;
                m_storedExamples[category, 1] = record;
                continue;
            }

            if (record.m_excutedCommands.Count > m_storedExamples[category, 1].m_excutedCommands.Count)
                m_storedExamples[category, 1] = record;

            if (record.m_excutedCommands.Count < m_storedExamples[category, 0].m_excutedCommands.Count)
                m_storedExamples[category, 0] = record;
        }
    }
}