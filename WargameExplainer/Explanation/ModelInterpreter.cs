using System.Diagnostics;
using WargameExplainer.TrainingSystem;

namespace WargameExplainer.Explanation;

public class ModelInterpreter
{
    /// <summary>
    ///     Contains the dictionary, where each game state observer starts in the list of corresponding feature values.
    /// </summary>
    private readonly List<(GameStateObserver observer, int startingPoint)> m_indexedObservers = new();

    /// <summary>
    ///     Generates the model interpreter fom the information of the training provider.
    /// </summary>
    /// <param name="trainingInfo">The trainin info provider we use.</param>
    public ModelInterpreter(TrainingInfoProvider trainingInfo)
    {
        int startCounter = 0;
        foreach (GameStateObserver observer in trainingInfo.Observers)
        {
            m_indexedObservers.Add((observer, startCounter));
            startCounter += (observer.ObserverType == GameStateObserver.ObserverCategory.OneHotEncoded) ? 1 + observer.HighestObservedValue : 1;
        }
    }


    /// <summary>
    ///     Generates a full dump of the model.
    /// </summary>
    /// <param name="model">Model we like to dump.</param>
    public void FullDump(LinearModel model)
    {
        int numPlayers = model.Intercept.Count;
        for (int player = 0; player < numPlayers; ++player)
        {
            Console.WriteLine();
            Console.WriteLine($"Interpretation of player {player + 1}");
            Console.WriteLine("========================================================");
            Console.WriteLine($"Intercept: {model.Intercept[player]}");
            foreach (var x in m_indexedObservers) InterpretObservationPair(model, x, player);
        }
    }

    /// <summary>
    ///     Interprets the model and only shows the perspectives of the player.
    /// </summary>
    /// <param name="model">Linear model to interpret.</param>
    public void InterpretModel(LinearModel model)
    {
        int numPlayers = model.Intercept.Count;
        for (int player = 0; player < numPlayers; ++player)
        {
            Console.WriteLine();
            Console.WriteLine($"Interpretation of player {player + 1}");
            Console.WriteLine("========================================================");
            Console.WriteLine($"Intercept: {model.Intercept[player]}");
            foreach (var x in m_indexedObservers)
            {
                if (!x.observer.IsControlledByPlayer(player))
                    continue;

                InterpretObservationPair(model, x, player);
            }
        }
    }


    /// <summary>
    ///     Generates a verbal interpretation for a specific observation.
    /// </summary>
    /// <param name="model">The model we interpret.</param>
    /// <param name="observationPair">The pair of the observation we would like</param>
    /// <param name="player">The player to interpret.</param>
    private static void InterpretObservationPair(LinearModel model,
        (GameStateObserver observer, int startingPoint) observationPair,
        int player)
    {

        switch (observationPair.observer.ObserverType)
        {
            case GameStateObserver.ObserverCategory.Continuous:
            case GameStateObserver.ObserverCategory.BalancedContinuous:
                Console.WriteLine(
                    $"Impact of number of {observationPair.observer.Interpretation} has Value {model.Weights[player][observationPair.startingPoint]}.");
                break;
            case GameStateObserver.ObserverCategory.Discreet:
            case GameStateObserver.ObserverCategory.BalancedDiscreet:
                Console.WriteLine(
                    $"Impact of number of {observationPair.observer.Interpretation} has Value {model.Weights[player][observationPair.startingPoint] / observationPair.observer.HighestObservedValue} per step.");
                break;
            case GameStateObserver.ObserverCategory.OneHotEncoded:

                for (int oneHot = 0; oneHot <= observationPair.observer.HighestObservedValue; ++oneHot)
                    Console.WriteLine(
                        $"Impact of number of {observationPair.observer.Interpretation} is exactly {oneHot} has Value {model.Weights[player][observationPair.startingPoint + oneHot]}.");
                break;

            default:
                Debug.Assert(false, "Case not implemented.");
                break;
        }
    
    }
}