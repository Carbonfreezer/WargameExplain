using WargameExplainer.Explanation;
using WargameExplainer.Strategy;

namespace WargameExplainer.TrainingSystem;

/// <summary>
///     This class encapsulates an original training info provider and a sparrring partner info provider that encapsulates
///     the sparring information. The sparring info provider may be one with different features and observations to be able
///     to check, if we have increased performance.
/// </summary>
public class TrainingInfoProviderSparring : TrainingInfoProvider
{
    /// <summary>
    ///     The player that is used for the original training.
    /// </summary>
    private readonly int m_originalPlayer;

    /// <summary>
    ///     The info provider we use for the system to be tested.
    /// </summary>
    private readonly TrainingInfoProvider m_originalTrainingInfoProvider;

    /// <summary>
    ///     The info provider that uses the sparrings partner.
    /// </summary>
    private readonly TrainingInfoProvider m_sparringInfoProvider;

    /// <summary>
    /// Creates the training info provider for the sparrring.
    /// </summary>
    /// <param name="original">The training info provider that is used for the tested player.</param>
    /// <param name="sparring">The training info provider used for the rest.</param>
    /// <param name="originalPlayer">The tested player using the original info.</param>
    public TrainingInfoProviderSparring(TrainingInfoProvider original, TrainingInfoProvider sparring,
        int originalPlayer)
    {
        m_originalTrainingInfoProvider = original;
        m_sparringInfoProvider = sparring;
        m_originalPlayer = originalPlayer;
    }

    public override IList<GameStateObserver> Observers => m_originalTrainingInfoProvider.Observers;
    public override int BatchSize => m_originalTrainingInfoProvider.BatchSize;

    public override IGameOutcomeClassifier GetGameOutcomeClassifier()
    {
        return m_originalTrainingInfoProvider.GetGameOutcomeClassifier();
    }

    public override StrategicDecider GetFreshGame(IHashableGameState? baseInfo)
    {
        StrategicDecider baseDecider = m_originalTrainingInfoProvider.GetFreshGame(null);
        StrategicDecider sparringDecider = m_sparringInfoProvider.GetFreshGame(baseDecider.GameState);

        SparringsPartner sparring = new SparringsPartner(baseDecider.Manipulator, sparringDecider, m_originalPlayer);
        baseDecider.TrainingsPartner = sparring;
        return baseDecider;
    }
}