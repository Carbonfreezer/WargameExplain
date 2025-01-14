using System.Drawing;
using WargameExplainer.Board;
using WargameExplainer.DrawingSystem;
using WargameExplainer.Strategy;
using WargameExplainer.TrainingSystem;

namespace WargameExplainer.SampleGames.TakeThatHill;

/// <summary>
///     Contains the complete game board. With board and pieces.
/// </summary>
public class TakeThatHillGameState : IHashableGameState, IPaintable, IObserverPreparer
{
    /// <summary>
    ///     The array with the units we use.
    /// </summary>
    private readonly Unit[] m_arrayOfUnits = new Unit[3];

    /// <summary>
    ///     The headquarter.
    /// </summary>
    private readonly BaseHq m_baseHq;

    /// <summary>
    ///     The defender.
    /// </summary>
    private readonly Defender m_defender;


    /// <summary>
    ///     The painter for the main board.
    /// </summary>
    private readonly GameBoardPainter m_gameBoardPainter;

    /// <summary>
    ///     The layout structure of the board.
    /// </summary>
    private readonly GameBoardLayoutFree m_layout;

    /// <summary>
    ///     The game phase manager we use.
    /// </summary>
    private readonly GamePhaseManager m_phaseManager;


    /// <summary>
    ///     Buffer for calculating the squence of
    /// </summary>
    private int[] m_sequenceDistance = new int[3];

    /// <summary>
    ///     Generates the complete game state.
    /// </summary>
    /// <param name="isInNightMode">Flags that we are using night mode rules.</param>
    public TakeThatHillGameState(bool isInNightMode)
    {
        m_layout = new GameBoardLayoutFree(OrientationType.PointyTop, OffsetType.OddOffset, 10);
        for (int i = 1; i <= 5; ++i)
        {
            m_layout.AddTile(i, 1);
            m_layout.AddTile(i, 2);
            m_layout.AddTile(i, 3);
        }

        m_layout.AddTile(6, 2);

        PieceCollection = new PieceCollection(m_layout, 10);

        m_defender = new Defender(m_layout);
        PieceCollection.PlaceElement(m_defender);

        m_baseHq = new BaseHq(new CoordinatesOffset(1, 2), m_layout);
        PieceCollection.PlaceElement(m_baseHq);

        for (int i = 0; i < 3; ++i)
        {
            m_arrayOfUnits[i] = new Unit(new CoordinatesOffset(1, 1 + i), m_layout, i + 1);
            PieceCollection.PlaceElement(m_arrayOfUnits[i]);
        }


        ShotsTaken = 0;

        m_gameBoardPainter = new GameBoardPainter(m_layout, PieceCollection);

        m_phaseManager = new GamePhaseManager(isInNightMode);

        IsInNightMode = isInNightMode;
    }

    /// <summary>
    ///     Checks, if we are in night mode.
    /// </summary>
    public bool IsInNightMode { get; }

    /// <summary>
    ///     The game round when the offender as asked for illumination.
    /// </summary>
    public int IlluminationOffenderRound { get; set; } = -2;

    /// <summary>
    ///     The game round when the defender has asked for illumination.
    /// </summary>
    public int IlluminationDefenderRond { get; set; } = -2;

    /// <summary>
    ///     Checks if we are currently illuminated, because that influences the chance values on rally and on fire
    ///     probabilities.
    /// </summary>
    public bool IsIlluminated => (!IsInNightMode) || (IlluminationDefenderRond == GameRound) ||
                                 (IlluminationOffenderRound == GameRound);


    /// <summary>
    ///     The shots the attackers have already taken.
    /// </summary>
    public int ShotsTaken { get; private set; }

    /// <summary>
    ///     Asks for the current game round.
    /// </summary>
    public int GameRound => m_phaseManager.GameRound;

    /// <summary>
    ///     Gets the enemy.
    /// </summary>
    public Defender Enemy => m_defender;

    /// <summary>
    ///     Gets the Hq.
    /// </summary>
    public BaseHq Hq => m_baseHq;

    /// <summary>
    ///     The base board we use for the figure collection.
    /// </summary>
    public PieceCollection PieceCollection { get; }


    /// <summary>
    ///     Gets the layout.
    /// </summary>
    public GameBoardLayout Layout => m_layout;


    /// <summary>
    ///     Gets the phase manager.
    /// </summary>
    public GamePhaseManager PhaseManager => m_phaseManager;


    /// <inheritdoc />
    public void AppendData(BinaryWriter writer)
    {
        m_phaseManager.AppendData(writer);
        m_baseHq.AppendData(writer);
        foreach (Unit unit in m_arrayOfUnits)
            unit.AppendData(writer);
        m_defender.AppendData(writer);
        writer.Write(ShotsTaken);
    }

    /// <inheritdoc />
    public bool IsCurrentlyHashable => true;

    /// <summary>
    ///     Prepares the sequence of units.
    /// </summary>
    public void PrepareObservations()
    {
        m_sequenceDistance = Enumerable.Range(0, 3).Select(GetDistanceToUnit).Order().ToArray();
    }


    /// <summary>
    ///     Decides, if we want to get a screenshot.
    /// </summary>
    public bool WantsScreenshot => m_phaseManager.WantsScreenshot;


#pragma warning disable CA1416
    /// <inheritdoc />
    public void PaintElement(Graphics graphics, int imageWidth, int imageHeight)
    {
        m_gameBoardPainter.PaintElement(graphics, imageWidth, imageHeight);
        Font drawFont = new Font("Arial", 12);
        Brush drawBrush = new SolidBrush(Color.Black);

        graphics.DrawString(
            $"Round: {GameRound} Hits taken: {ShotsTaken} Beginning Of Phase: {PhaseManager.CurrentPhase.ToString()}",
            drawFont, drawBrush, 50, 500);
    }
#pragma warning restore CA1416


    /// <summary>
    ///     Gets the possible moves of the Hq.
    /// </summary>
    /// <returns>Where the hq can go.</returns>
    public IEnumerable<CoordinatesAxial> GetPossibleMovesForHq()
    {
        if (m_baseHq.IsSpend)
            return Enumerable.Empty<CoordinatesAxial>();

        int currentDistance = BoardCoder.GetDistanceCube(m_baseHq.Position, m_defender.Position);
        return BoardCoder.GetNeighbors(m_baseHq.Position).Where(pos =>
            (m_layout.IsInBoard(pos) && (BoardCoder.GetDistanceCube(pos, m_defender.Position) <= currentDistance)));
    }

    /// <summary>
    ///     Get all the legal moves for the unit. Not including none.
    /// </summary>
    /// <param name="unit">The unit to move.</param>
    /// <returns>Legal movement positions for the unit.</returns>
    /*
    public IEnumerable<CoordinatesAxial> GetPossibleMovesForUnit(int unit)
    {
        if (m_arrayOfUnits[unit].IsSpend)
            return Enumerable.Empty<CoordinatesAxial>();
        int currentDistance = BoardCoder.GetDistanceCube(m_arrayOfUnits[unit].Position, m_defender.Position);
        return BoardCoder.GetNeighbors(m_arrayOfUnits[unit].Position).Where(pos => m_layout.IsInBoard(pos) &&
            (BoardCoder.GetDistanceCube(pos, m_defender.Position) <= currentDistance) &&
            PieceCollection.GetElements(pos).All(cand => cand is not Unit));
    }
    */
    // Replacement hack to make things faster.
    public IEnumerable<CoordinatesAxial> GetPossibleMovesForUnit(int unit)
    {
        if (m_arrayOfUnits[unit].IsSpend)
            return Enumerable.Empty<CoordinatesAxial>();
        int currentDistance = BoardCoder.GetDistanceCube(m_arrayOfUnits[unit].Position, m_defender.Position);
        if (currentDistance == 1)
            return [m_defender.Position];

        // Simply go one to the right.
        CoordinatesOffset flatPosition =
            new CoordinatesOffset(m_arrayOfUnits[unit].Position, Layout.Orientation, Layout.Offset);
        flatPosition.Col += 1;
        return [flatPosition.GetAxial(Layout.Orientation, Layout.Offset)];
    }

    /// <summary>
    ///     Checks if a certain unit can fire.
    /// </summary>
    /// <param name="unit">The unit to test.</param>
    /// <returns>Returns if it can fire.</returns>
    public bool CanUnitFire(int unit)
    {
        if (m_arrayOfUnits[unit].IsSpend)
            return false;

        IEnumerable<CoordinatesAxial> lineOfSight =
            BoardCoder.LinePoints(m_arrayOfUnits[unit].Position, m_defender.Position, false);
        return lineOfSight.All(pos => PieceCollection.IsFree(pos));
    }

    /// <summary>
    ///     Gets called when successfully shot at the unit.
    /// </summary>
    /// <param name="unit">The unit we shoot at.</param>
    /// <returns>Whether the unit was spent.</returns>
    public bool ShootAtUnit(int unit)
    {
        bool wasSpend = m_arrayOfUnits[unit].IsSpend;
        m_arrayOfUnits[unit].IsSpend = true;
        m_arrayOfUnits[unit].HasBeenHit = true;
        ShotsTaken++;

        return wasSpend;
    }

    /// <summary>
    ///     Undos the shot operation.
    /// </summary>
    /// <param name="unit">Unit that was shot at.</param>
    /// <param name="wasSpend">Information, if unit was spend before.</param>
    public void UndoShotAtUnit(int unit, bool wasSpend)
    {
        m_arrayOfUnits[unit].IsSpend = wasSpend;
        m_arrayOfUnits[unit].HasBeenHit = false;
        ShotsTaken--;
    }

    /// <summary>
    ///     Gets the distance to the unit and the defender.
    /// </summary>
    /// <param name="unit">The unit we move.</param>
    /// <returns>Distance of unit.</returns>
    public int GetDistanceToUnit(int unit)
    {
        return BoardCoder.GetDistanceCube(m_arrayOfUnits[unit].Position, m_defender.Position);
    }


    /// <summary>
    ///     Asks for a unit.
    /// </summary>
    /// <param name="unit">Index of unit.</param>
    /// <returns>Unit.</returns>
    public Unit GetUnit(int unit)
    {
        return m_arrayOfUnits[unit];
    }

    /// <summary>
    ///     Gets the distance of the closest unit from rank number 0: closest 1: middle 2: closest unit.
    /// </summary>
    /// <param name="rankNumber">The rank with 0 beeing closest and 1 beeing furthest from Defender</param>
    /// <returns>Returns the distance</returns>
    public int GetDistanceOfClosestUnit(int rankNumber)
    {
        return m_sequenceDistance[rankNumber];
    }
}