using WargameExplainer.DrawingSystem;
using WargameExplainer.Explanation;
using WargameExplainer.SampleGames.TakeThatHill;
using WargameExplainer.Strategy;
using WargameExplainer.TrainingSystem;

namespace WargameExplainer.Utils;

/// <summary>
///     Contains a class for high level functions.
/// </summary>
public static class HighLevelFunctions
{
    /// <summary>
    ///     Generates the results statistics for the game locally un parallelized.
    /// </summary>
    /// <typeparam name="TInfoProvider">The type of the training info provider we like to use.</typeparam>
    /// <param name="modelName">The filename of the model we load. Null if no paramater is loaded.</param>
    public static void GetResultStatisticsLocally<TInfoProvider>(string? modelName)
        where TInfoProvider : TrainingInfoProvider, new()
    {
        TrainingInfoProvider provider = new TInfoProvider();
        if (modelName != null)
        {
            LinearModel usedModel = new LinearModel();
            usedModel.LoadFromFile(modelName);
            provider.LinearModel = usedModel;
        }

        IGameOutcomeClassifier interpreter = provider.GetGameOutcomeClassifier();

        EpisodicGenerator generator = new EpisodicGenerator(provider);
        var rawResult = generator.GetStatistics(provider.BatchSize);

        var mappedResult = rawResult.Select(vec => interpreter.GetCategory(vec));

        // Now we need to compute the outcome results.
        int[] accumulatedResults = Enumerable.Range(0, interpreter.NumOfCategories)
            .Select(cat => mappedResult.Count(i => i == cat)).ToArray();


        for (int i = 0; i < interpreter.NumOfCategories; ++i)
            Console.WriteLine($"{interpreter.GetDescription(i)}: {accumulatedResults[i]} ");
    }

    /// <summary>
    /// Runs a game and generates screenshots along the way.
    /// </summary>
    /// <typeparam name="TInfoProvider">The training info provider to use.</typeparam>
    /// <param name="modelName">The name of the model or null if not used.</param>
    public static void RunGameWithScreenshots<TInfoProvider>(string? modelName)
        where TInfoProvider : TrainingInfoProvider, new()
    {
        TrainingInfoProvider provider = new TInfoProvider();

        if (modelName != null)
        {
            LinearModel usedModel = new LinearModel();
            usedModel.LoadFromFile(modelName);
            provider.LinearModel = usedModel;
        }

        DebugPainter painter = new DebugPainter(1024, 1024);


        using StrategicDecider strategicDecider = provider.GetFreshGame(null);

        Span<float> values = stackalloc float[strategicDecider.NumOfPlayerPerspectives];
        painter.GenerateSnapshotSerial((IPaintable)strategicDecider.GameState);
        while (!strategicDecider.IsGameOverAndAfterStateValue(values))
        {
            strategicDecider.ExecuteNewMove();
            if (((IPaintable)strategicDecider.GameState).WantsScreenshot)
                painter.GenerateSnapshotSerial(((IPaintable)strategicDecider.GameState));
        }

        painter.GenerateSnapshotSerial(((IPaintable)strategicDecider.GameState));
    }

    public static void RunGameWithScreenshotsBoundary<TSingularProvider, TRemainingProvider>(string? singularModel,
        string? generalModel, int singularPlayer)
        where TSingularProvider : TrainingInfoProvider, new()
        where TRemainingProvider : TrainingInfoProvider, new()
    {
        TrainingInfoProvider provider = GetSparringProvider<TSingularProvider, TRemainingProvider>(singularModel, generalModel, singularPlayer);

       
        DebugPainter painter = new DebugPainter(1024, 1024);


        using StrategicDecider strategicDecider = provider.GetFreshGame(null);

        Span<float> values = stackalloc float[strategicDecider.NumOfPlayerPerspectives];
        painter.GenerateSnapshotSerial((IPaintable)strategicDecider.GameState);
        while (!strategicDecider.IsGameOverAndAfterStateValue(values))
        {
            strategicDecider.ExecuteNewMove();
            if (((IPaintable)strategicDecider.GameState).WantsScreenshot)
                painter.GenerateSnapshotSerial(((IPaintable)strategicDecider.GameState));
        }

        painter.GenerateSnapshotSerial(((IPaintable)strategicDecider.GameState));
    }

    /// <summary>
    ///     Generates a combined training info provider for sparring.
    /// </summary>
    /// <typeparam name="TNewProvider">The type of the new training info provider.</typeparam>
    /// <typeparam name="TBaselineProvider">The type of the training info provider we previously had.</typeparam>
    /// <param name="newParams">The parameters for the new model if existing.</param>
    /// <param name="oldParams">The parameters for the old model if existing.</param>
    /// <param name="candidatePlayer">The candidate player for the new model.</param>
    /// <returns>Finally constructed training info provider.</returns>
    public static TrainingInfoProvider GetSparringProvider<TNewProvider, TBaselineProvider>(string? newParams,
        string? oldParams, int candidatePlayer)
        where TNewProvider : TrainingInfoProvider, new()
        where TBaselineProvider : TrainingInfoProvider, new()
    {
        TrainingInfoProvider newProvider = new TNewProvider();

        if (newParams != null)
        {
            LinearModel model = new LinearModel();
            model.LoadFromFile(newParams);
            newProvider.LinearModel = model;
        }


        TrainingInfoProvider oldProvider = new TBaselineProvider();
        if (oldParams != null)
        {
            LinearModel model = new LinearModel();
            model.LoadFromFile(oldParams);
            oldProvider.LinearModel = model;
        }

        TrainingInfoProviderSparring result =
            new TrainingInfoProviderSparring(newProvider, oldProvider, candidatePlayer);
        return result;
    }


    /// <summary>
    ///     Does a local training (one thread only) intended for debugging purposes.
    /// </summary>
    /// <typeparam name="TInfoProvider">The type of the training info provider we like to use.</typeparam>
    /// <param name="modelName">The filename to save the model to.</param>
    public static void PerformTrainingLocally<TInfoProvider>(string modelName)
        where TInfoProvider : TrainingInfoProvider, new()
    {
        TrainingInfoProvider provider = new TInfoProvider();
        EpisodicGenerator generator = new EpisodicGenerator(provider);
        RidgeModelFitter fitter = new RidgeModelFitter(provider);

        IList<EpisodicRecord> episodes = generator.GenerateEpisodes(provider.BatchSize, provider.EpsilonTraining);
        LinearModel model = fitter.GetRegularizedModel(episodes);

        model.SaveToFile(modelName);
    }

    /// <summary>
    ///     Generates an example explanation with sample pictures of the game. Only works if the IHashableGameState also
    ///     implements IPainter.
    /// </summary>
    /// <typeparam name="TInfoProvider">The type of training info provider we like to use.</typeparam>
    /// <param name="modelName">The name of the model to interpret.</param>
    /// <param name="simulationRuns">The simulations runs to execute.</param>
    /// <param name="imageWidth">The resulting image width.</param>
    /// <param name="imageHeight">The resulting image height.</param>
    public static void GenerateExampleExplanation<TInfoProvider>(string modelName, int simulationRuns, int imageWidth,
        int imageHeight) where TInfoProvider : TrainingInfoProvider, new()
    {
        TInfoProvider trainer = new TInfoProvider();
        LinearModel usedModel = new LinearModel();
        usedModel.LoadFromFile(modelName);
        trainer.LinearModel = usedModel;

        IGameOutcomeClassifier outcomeClassifier = trainer.GetGameOutcomeClassifier();

        using ExampleExplainer explainer = new ExampleExplainer(trainer, outcomeClassifier, imageWidth, imageHeight);
        explainer.GenerateExampleExplanation(simulationRuns);
    }


    /// <summary>
    ///     Generates a verbal explanation of a specific model.
    /// </summary>
    /// <typeparam name="TInfoProvider">The type of the training info provider to use.</typeparam>
    /// <param name="modelName">The filename of the model to interpret.</param>
    public static void GenerateVerbalExplanation<TInfoProvider>(string modelName)
        where TInfoProvider : TrainingInfoProvider, new()
    {
        LinearModel model = new LinearModel();
        model.LoadFromFile(modelName);

        TInfoProvider trainer = new TInfoProvider();
        trainer.LinearModel = model;

        ModelInterpreter interpreter = new ModelInterpreter(trainer);
        Console.Write("======== Complete ==========");
        interpreter.FullDump(model);
        Console.Write("======== Specific ==========");
        interpreter.InterpretModel(model);
    }
}