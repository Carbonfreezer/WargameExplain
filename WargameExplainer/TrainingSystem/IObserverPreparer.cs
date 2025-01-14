namespace WargameExplainer.TrainingSystem;

/// <summary>
///     An interface that can be implemented by any game state to prepare in general observations for all observers.
/// </summary>
public interface IObserverPreparer
{
    /// <summary>
    ///     Gets invoked to prepare observers.
    /// </summary>
    void PrepareObservations();
}