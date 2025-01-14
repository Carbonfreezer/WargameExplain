using WargameExplainer.Strategy;

namespace WargameExplainer.SampleGames.TakeThatHill;

/// <summary>
///     Adiministrates the different movement phases.
/// </summary>
public class GamePhaseManager : IHashable
{
    /// <summary>
    ///     The general handling phase we are in.
    /// </summary>
    public enum HandlingPhase
    {
        PreviousRound,
        Movement,
        FireDecision,
        FireExecution,
        Rallying,
        Defender,
        NextRound
    }


    /// <summary>
    ///     Contains the subphases of the main handling phase.
    /// </summary>
    public static int[] SubPhases = [0, 4, 3, 3, 4, 2];

    /// <summary>
    ///     Flags that stage is random
    /// </summary>
    public static bool[] RandomStage =
    [
        false,
        false, false, true, true,
        true
    ];

    /// <summary>
    ///     Flags that we use illumination.
    /// </summary>
    private readonly bool m_usesIllumination;


    /// <summary>
    ///     Generates the game phase manager depending for the mode if we use illumination or not.
    /// </summary>
    /// <param name="usesIllumination">Do we use illumination?</param>
    public GamePhaseManager(bool usesIllumination)
    {
        m_usesIllumination = usesIllumination;
        if (usesIllumination)
        {
            CurrentPhase = HandlingPhase.Defender;
            CurrentSubphase = SubPhases[(int)CurrentPhase] - 1;
            GameRound = 0;
        }
        else
        {
            GameRound = 1;
            CurrentPhase = HandlingPhase.Movement;
            CurrentSubphase = 0;
        }
    }

    /// <summary>
    ///     Flags that we are waiting for an illumination decision of the offender.
    /// </summary>
    public bool OffenderIlluminationDecision => m_usesIllumination && (GameRound == 0);

    /// <summary>
    ///     The game round we are currently in.
    /// </summary>
    public int GameRound { get; private set; }

    /// <summary>
    ///     The current phase we are in for handling.
    /// </summary>
    public HandlingPhase CurrentPhase { get; private set; }


    /// <summary>
    ///     The current subphase of the character.
    /// </summary>
    public int CurrentSubphase { get; private set; }

    /// <summary>
    ///     Flags, if we are in random stage.
    /// </summary>
    public bool IsInRandomStage { get; private set; }


    /// <summary>
    ///     Decides, if we want to get a screenhsot.
    /// </summary>
    public bool WantsScreenshot => (CurrentSubphase == 0);

    public void AppendData(BinaryWriter writer)
    {
        writer.Write((int)CurrentPhase);
        writer.Write(CurrentSubphase);
        writer.Write(GameRound);
    }


    /// <summary>
    ///     Moves over to the next stage in the round phase.
    /// </summary>
    public void AdvanceStage()
    {
        CurrentSubphase += 1;
        if (CurrentSubphase >= SubPhases[(int)CurrentPhase])
        {
            CurrentSubphase = 0;
            CurrentPhase++;
        }

        if (CurrentPhase == HandlingPhase.NextRound)
        {
            GameRound++;
            CurrentSubphase = 0;
            CurrentPhase = HandlingPhase.Movement;
        }

        IsInRandomStage = RandomStage[(int)CurrentPhase];
    }


    /// <summary>
    ///     Same as above just in reverse order.
    /// </summary>
    public void ReverseStage()
    {
        CurrentSubphase -= 1;
        if (CurrentSubphase < 0)
        {
            CurrentPhase--;
            CurrentSubphase = SubPhases[(int)CurrentPhase] - 1;
        }

        if (CurrentPhase == HandlingPhase.PreviousRound)
        {
            GameRound--;
            CurrentPhase = HandlingPhase.Defender;
            CurrentSubphase = SubPhases[(int)CurrentPhase] - 1;
        }

        IsInRandomStage = RandomStage[(int)CurrentPhase];
    }
}