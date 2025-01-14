using WargameExplainer.Strategy;

namespace WargameExplainer.TrainingSystem;

/// <summary>
///     This is an abstract that describes a single game state observer. Every observer observes a feature of the game
///     state and returns an integer value or a float in the range of 0..1.
///     The discreet value may be directly used in a linear combination to generate a value function normalized by the
///     maximum value  or may be used in one hot
///     encoded fashion for a linear combination.
/// </summary>
public abstract class GameStateObserver
{
    /// <summary>
    ///     The type of observer we have.
    /// </summary>
    public enum ObserverCategory
    {
        /// <summary>
        ///     The observer returns values in float in the 0..1 range.
        /// </summary>
        Continuous,

        /// <summary>
        ///     The observer returns values from 0 ... highest observed value.
        /// </summary>
        Discreet,

        /// <summary>
        ///     The observer also returns a discreet value, that gets one hot encoded.
        /// </summary>
        OneHotEncoded,

        /// <summary>
        /// The observer returns a discreet value that goes from - highestObservedValue to + highestObservedValue
        /// </summary>
        BalancedDiscreet,

        /// <summary>
        /// The observer returns values in the -1 1 range.
        /// </summary>
        BalancedContinuous
    }

    /// <summary>
    ///     Returns the amount of observation that can be returned. This is necessary later on to perform a one hot encoding
    ///     and for normalization. Te value range is then between 0 ... HighestObservedValue
    /// </summary>
    public virtual int HighestObservedValue =>
        throw new NotImplementedException("Call to highest observed value without implementation.");

    /// <summary>
    ///     Flags the observer type we use.
    /// </summary>
    public abstract ObserverCategory ObserverType { get; }

    /// <summary>
    ///     Asks for an interpretation string.
    /// </summary>
    public abstract string Interpretation { get; }

    /// <summary>
    ///     Flags if we are  directly influencable by a player perspective and therefore show the element
    ///     in the explanation.
    /// </summary>
    public abstract bool IsControlledByPlayer(int playerPerspective);

    /// <summary>
    ///     Gets the observation from the observable game state in discreet form.
    /// </summary>
    /// <param name="gameState">The game state we want to observe.</param>
    /// <returns>The observation as an integer value.</returns>
    public virtual int GetDiscreetObservation(IHashableGameState gameState)
    {
        throw new NotImplementedException("Call to discreet observation is not implemented.");
    }

    /// <summary>
    ///     Gets the observation from the observable game state in continuous form.
    /// </summary>
    /// <param name="gameState">The game state we want to observe.</param>
    /// <returns>The observation as an integer value.</returns>
    public virtual float GetContinuousObservation(IHashableGameState gameState)
    {
        throw new NotImplementedException("Call to continuous observation is not implemented.");
    }
}