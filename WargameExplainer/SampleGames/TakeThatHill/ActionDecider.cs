using System.Diagnostics;
using WargameExplainer.Board;
using WargameExplainer.SampleGames.TakeThatHill.Commands;
using WargameExplainer.Strategy;
using WargameExplainer.Utils;

namespace WargameExplainer.SampleGames.TakeThatHill;

/// <summary>
///     The action decider provides the manipulation information for Take That Hill basic version.
/// </summary>
public class ActionDecider : IManipulator
{
    /// <summary>
    ///     The game state of Take That Hill.
    /// </summary>
    private readonly TakeThatHillGameState m_gameState;


    /// <summary>
    ///     Gets constructed from the gameState.
    /// </summary>
    /// <param name="gameState">The game state the action decider refers to.</param>
    public ActionDecider(TakeThatHillGameState gameState)
    {
        m_gameState = gameState;
    }


    /// <inheritdoc />
    public bool IsRandomManipulator => m_gameState.PhaseManager.IsInRandomStage;


    public IList<ICommand> GetMoveOptionsWithProbabilities(in Span<float> probabilities)
    {
        // Deal with the preselction of lights.
        if (m_gameState.PhaseManager.OffenderIlluminationDecision)
            return IlluminationDecisions();

        if (IsRandomManipulator)
            return HandleRandomMoves(m_gameState.PhaseManager, probabilities);
        return HandleSystematicMoves(m_gameState.PhaseManager);
    }

    /// <inheritdoc />
    public int PlayerPerspective => 0;

    public int MaxNumOfProbabilityChoices => 2;


    /// <summary>
    ///     Asks for the list with illumination decisions.
    /// </summary>
    /// <returns>The list with the different options we can use for illumination.</returns>
    private IList<ICommand> IlluminationDecisions()
    {
        List<ICommand> result = new List<ICommand>(10);
        for (int i = 1; i <= 10; ++i)
        {
            ICommand comm = new CommandCompound([
                new CommandSetIllumination(m_gameState, i),
                new CommandAdvanceOperation(m_gameState.PhaseManager)
            ]);
            result.Add(comm);
        }
        return result;
    }


    /// <summary>
    ///     Does all the movement parts, where there is a real choice by an agent.
    /// </summary>
    /// <param name="mngr">game phase manager.</param>
    /// <returns>The list with executable commands in that position.</returns>
    private IList<ICommand> HandleSystematicMoves(GamePhaseManager mngr)
    {
        Unit unit;
        List<ICommand> result = new List<ICommand>(7);
        switch (mngr.CurrentPhase)
        {
            case GamePhaseManager.HandlingPhase.Movement:
                if (mngr.CurrentSubphase == 3)
                {
                    // Hq is alawys rallied.
                    // We can always remain standing.
                    result.Add(new CommandAdvanceOperation(mngr));
                    BaseHq hq = m_gameState.Hq;
                    // This is the case of Hq movement.
                    foreach (CoordinatesAxial coords in m_gameState.GetPossibleMovesForHq())
                    {
                        CommandCompound moveHq = new CommandCompound([
                            new CommandMovement(hq, coords, m_gameState.PieceCollection),
                            new CommandSpendHq(hq),
                            new CommandAdvanceOperation(mngr)
                        ]);
                        result.Add(moveHq);
                    }

                    return result;
                }

                // Now we deal with the other units.
                result.Add(new CommandAdvanceOperation(mngr));

                unit = m_gameState.GetUnit(mngr.CurrentSubphase);

                // When we are spend we can not do anything.
                if (unit.IsSpend)
                    return result;

                foreach (CoordinatesAxial coords in m_gameState.GetPossibleMovesForUnit(mngr.CurrentSubphase))
                {
                    CommandCompound moveUnit = new CommandCompound([
                        new CommandMovement(unit, coords, m_gameState.PieceCollection),
                        new CommandSpendUnit(unit),
                        new CommandAdvanceOperation(mngr)
                    ]);
                    result.Add(moveUnit);
                }

                return result;
            case GamePhaseManager.HandlingPhase.FireDecision:
                result.Add(new CommandAdvanceOperation(mngr));
                if (!m_gameState.CanUnitFire(mngr.CurrentSubphase))
                    return result;
                unit = m_gameState.GetUnit(mngr.CurrentSubphase);
                CommandCompound fireOp = new CommandCompound([
                    new CommandSetFiring(m_gameState.GetUnit(mngr.CurrentSubphase)),
                    new CommandSpendUnit(unit),
                    new CommandAdvanceOperation(mngr)
                ]);
                result.Add(fireOp);


                return result;
        }

        Debug.Assert(false, "We should never be here.");
        return null;
    }

    /// <summary>
    ///     Handles all the actions, where there is a random move involved.
    /// </summary>
    /// <param name="mngr">Game phase manager.</param>
    /// <param name="probabilities">The span with the probabilities we have on random choice.</param>
    private IList<ICommand> HandleRandomMoves(GamePhaseManager mngr, in Span<float> probabilities)
    {
        CommandCompound success;
        CommandCompound failiure;
        Unit unit;
        int dist;
        List<ICommand> result = new List<ICommand>(2);

        switch (mngr.CurrentPhase)
        {
            case GamePhaseManager.HandlingPhase.FireExecution:
                unit = m_gameState.GetUnit(mngr.CurrentSubphase);
                // If we have not fired there is nothing to do here.
                if (!unit.HasFired)
                {
                    result.Add(new CommandAdvanceOperation(mngr));
                    probabilities[0] = 1.0f;
                    return result;
                }

                dist = m_gameState.GetDistanceToUnit(mngr.CurrentSubphase);
                float hitProb = m_gameState.IsIlluminated ?  (6 - dist) / 6.0f : (4 - dist) / 6.0f;
                hitProb = Math.Clamp(hitProb, 0.0f, 1.0f);

                success = new CommandCompound([
                    new CommandResetFiring(unit),
                    new CommandShootDefender(m_gameState.Enemy),
                    new CommandAdvanceOperation(mngr)
                ]);

                failiure = new CommandCompound([
                    new CommandResetFiring(unit),
                    new CommandAdvanceOperation(mngr)
                ]);

                result.Add(success);
                probabilities[0] = hitProb;
                result.Add(failiure);
                probabilities[1] = 1.0f - hitProb;
                return result;
            case GamePhaseManager.HandlingPhase.Rallying:
                if (mngr.CurrentSubphase == 3)
                {
                    // We want to rally the Hq.
                    probabilities[0] = 1.0f;

                    if (!m_gameState.Hq.IsSpend)
                    {
                        result.Add(new CommandAdvanceOperation(mngr));
                        return result;
                    }

                    success = new CommandCompound([
                        new CommandRallyHq(m_gameState.Hq),
                        new CommandAdvanceOperation(mngr)
                    ]);
                    result.Add(success);
                    return result;
                }

                unit = m_gameState.GetUnit(mngr.CurrentSubphase);
                if (!unit.IsSpend)
                {
                    probabilities[0] = 1.0f;
                    result.Add(new CommandAdvanceOperation(mngr));
                    return result;
                }

                if (unit.Position == m_gameState.Hq.Position)
                {
                    probabilities[0] = 1.0f;
                    success = new CommandCompound([
                        new CommandRallyUnit(unit),
                        new CommandAdvanceOperation(mngr)
                    ]);
                    result.Add(success);
                    return result;
                }

                if (!m_gameState.Enemy.IsSpend && m_gameState.GetDistanceToUnit(mngr.CurrentSubphase) == 1)
                {
                    probabilities[0] = 1.0f;
                    result.Add(new CommandAdvanceOperation(mngr));
                    return result;
                }

                success = new CommandCompound([
                    new CommandRallyUnit(unit),
                    new CommandAdvanceOperation(mngr)
                ]);

                dist = BoardCoder.GetDistanceCube(unit.Position, m_gameState.Hq.Position);

                float rallProb = m_gameState.IsIlluminated ? ( dist > 1 ? 4.0f / 6.0f : 5.0f / 6.0f) :
                    (dist > 1 ? 2.0f / 6.0f : 3.0f / 6.0f);

                result.Add(success);
                probabilities[0] = rallProb;
                result.Add(new CommandAdvanceOperation(mngr));
                probabilities[1] = 1.0f - rallProb;
                return result;
            case GamePhaseManager.HandlingPhase.Defender:
                // If we are spend we continue.
                if (m_gameState.Enemy.IsSpend)
                {
                    probabilities[0] = 1.0f;

                    // Simply continue.
                    if (mngr.CurrentSubphase == 0)
                    {
                        result.Add(new CommandAdvanceOperation(mngr));
                        return result;
                    }

                    success = new CommandCompound([
                        new CommandRallyDefender(m_gameState.Enemy),
                        new CommandAdvanceOperation(mngr)
                    ]);
                    result.Add(success);
                    return result;
                }

                // Now we can actually shoot. Get the closes enemy.
                int bestCand = -1;
                int bestDist = 100;
                for (int i = 0; i < 3; ++i)
                {
                    Unit candidate = m_gameState.GetUnit(i);
                    int locDist = BoardCoder.GetDistanceCube(candidate.Position, m_gameState.Enemy.Position);
                    if (locDist < bestDist)
                    {
                        bestDist = locDist;
                        bestCand = i;
                    }
                    else if (locDist == bestDist && !candidate.IsSpend)
                    {
                        bestCand = i;
                    }
                }

                // Now we have the best candidate and the best distance.
                if (mngr.CurrentSubphase == 0)
                {
                    float prob = m_gameState.IsIlluminated?  (7 - bestDist) / 6.0f : (5 - bestDist) / 6.0f;
                    prob = Math.Clamp(prob, 0.0f, 1.0f);
                    success = new CommandCompound([
                        new CommandShotUnit(bestCand, m_gameState),
                        new CommandAdvanceOperation(mngr)
                    ]);

                    result.Add(success);
                    probabilities[0] = prob;
                    failiure = new CommandCompound([
                        new CommandSetDefenderIllumination(m_gameState),
                        new CommandAdvanceOperation(mngr)
                    ]);
                    result.Add(failiure);
                  
                    probabilities[1] = 1.0f - prob;
                    return result;
                }

                // We are now in the second phase. We now need to grab a unit that is neighboring our best unit.
                int secondBestCand = -1;
                int secondBestDist = 100;
                for (int i = 0; i < 3; ++i)
                {
                    if (BoardCoder.GetDistanceCube(m_gameState.GetUnit(i).Position,
                            m_gameState.GetUnit(bestCand).Position) != 1) continue;
                    int locDist =
                        BoardCoder.GetDistanceCube(m_gameState.GetUnit(i).Position, m_gameState.Enemy.Position);

                    if (locDist < secondBestDist)
                    {
                        secondBestDist = locDist;
                        secondBestCand = i;
                    }
                    else if (locDist == secondBestDist && !m_gameState.GetUnit(i).IsSpend)
                    {
                        secondBestCand = i;
                    }
                }

                if (secondBestCand == -1)
                {
                    // Nothing further to shoot at.
                    probabilities[0] = 1.0f;
                    result.Add(new CommandAdvanceOperation(mngr));
                    return result;
                }

                // We fire a shot at the second unit.
                float prb = m_gameState.IsIlluminated ? (7 - secondBestDist) / 6.0f : (5 - secondBestDist) / 6.0f;
                prb = Math.Clamp(prb, 0.0f, 1.0f);
                success = new CommandCompound([
                    new CommandShotUnit(secondBestCand, m_gameState),
                    new CommandAdvanceOperation(mngr)
                ]);

                result.Add(success);
                probabilities[0] = prb;
                failiure = new CommandCompound([
                    new CommandSetDefenderIllumination(m_gameState),
                    new CommandAdvanceOperation(mngr)
                ]);
                result.Add(failiure);

                probabilities[1] = 1.0f - prb;
                return result;
        }

        Debug.Assert(false, "We should never be here.");
        return null;
    }
}