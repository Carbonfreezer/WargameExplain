using System.Diagnostics;
using WargameExplainer.Strategy;

namespace WargameExplainer.TrainingSystem;

/// <summary>
///     Implements a sparring partner as a specific manipulator, that refers for certain players to a fixed strategy that
///     is not trained. This construct is used for hyperparameter tuning to keep one player fixed.
/// </summary>
public class SparringsPartner : IDisposable
{
    /// <summary>
    /// The original manipulator.
    /// </summary>
    private readonly IManipulator m_coreManipulator;

    /// <summary>
    /// The sparrings partner that does eventually own decisions.
    /// </summary>
    private readonly StrategicDecider m_sparringsPartner;


    /// <summary>
    /// The player with which we do not use sparrings partner.
    /// </summary>
    private readonly int m_notSparredPlayer;

    /// <summary>
    /// Generates the sparrings partner with a core manipulator that we really use and a stratetgic decider, that should be used as a sparringspartner. 
    /// </summary>
    /// <param name="coreManipulator">The core manipulator that is used for one group of players.</param>
    /// <param name="partner">A strategic decider that models the sparrings partner.</param>
    /// <param name="notSparredPlayer">The player perspective for which we do not use the sparrings partner.</param>
    public SparringsPartner(IManipulator coreManipulator, StrategicDecider partner, int notSparredPlayer)
    {
        m_coreManipulator = coreManipulator;
        m_sparringsPartner = partner;
        m_notSparredPlayer = notSparredPlayer;
    }


    /// <summary>
    /// Flags if the current decision should get to sparring mode.
    /// </summary>
    public bool IsSparring => (m_coreManipulator.PlayerPerspective != m_notSparredPlayer);

    /// <summary>
    /// Executes new move with the sparrings partner.
    /// </summary>
    /// <returns>Executed move.</returns>

    public ICommand ExecuteSparringMove()
    {
        Debug.Assert(IsSparring, "We are nor responsible here.");
        return m_sparringsPartner.ExecuteNewMove();
    }

    
    public void Dispose()
    {
        m_sparringsPartner.Dispose();
    }
}