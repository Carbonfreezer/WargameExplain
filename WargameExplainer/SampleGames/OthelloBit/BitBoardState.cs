using System.Diagnostics;
using System.Drawing;
using System.Numerics;
using System.Text;
using WargameExplainer.DrawingSystem;
using WargameExplainer.Strategy;

namespace WargameExplainer.SampleGames.OthelloBit;

public class BitBoardState : IHashableGameState, IPaintable, IManipulator
{
    private readonly ulong m_boarderCutMask;

    /// UInt64 numbers describe every field on the board with one bit as follows:
    /// h8h7h6h5h4h3h2h1_g8g7g6g5g4g3g2g1_f8f7f6f5f4f3f2f1_e8e7e6e5e4e3e2e1_d8d7d6d5d4d3d2d1_c8c7c6c5c4c3c2c1_b8b7b6b5b4b3b2b1_a8a7a6a5a4a3a2a1
    private ulong m_blackStones;

    private ulong m_whiteStones;


    /// <summary>
    /// Gets a bit mask for a certain coordinates, that may be ored together for further analysis.
    /// </summary>
    /// <param name="xPos">x coordinate to test for.</param>
    /// <param name="yPos">y coordinate to test for.</param>
    /// <returns>resulting bit mask.</returns>
    public static ulong GetBitMaskForField(int xPos, int yPos)
    {
        Debug.Assert((xPos >= 0) && (yPos >= 0) && (xPos <= 7) && (yPos <= 7), "Illegal coordinate set.");
        return (1ul << (xPos * 8 + yPos));
    }


    /// <summary>
    /// Counts the amount of stones given for a certain mask.
    /// </summary>
    /// <param name="isBlack">Are we counting black stones?</param>
    /// <param name="mask">The mask we want to check for.</param>
    /// <returns>Amount of stones that are found in the masked field.</returns>
    public int GetStoneCountForMask(bool isBlack, ulong mask)
    {
        ulong candidate = mask & (isBlack ? m_blackStones : m_whiteStones);
        return BitOperations.PopCount(candidate);
    }

    public BitBoardState()
    {
        m_boarderCutMask = 0x7E7E7E7E7E7E7E7E; // cutoff A-line and H-line
        ResetBoard();
    }


    /// <summary>
    ///     Set board to initial state.
    /// </summary>
    public void ResetBoard()
    {
        m_blackStones = 0b00000000_00000000_00000000_00001000_00010000_00000000_00000000_00000000;
        m_whiteStones = 0b00000000_00000000_00000000_00010000_00001000_00000000_00000000_00000000;
        PlayerPerspective = 0;
    }


    /// <summary>
    ///     Get all possible lines of player disk - opponent disks next to player disk - new enlosing player disk
    /// </summary>
    /// <param name="player">Disks of current player</param>
    /// <param name="mask">
    ///     Disks of the opponent player except for his disks on A-line and h-line (so there won't be enclosing between
    ///     those two lines)
    /// </param>
    /// <param name="dir">Enclosing direction</param>
    /// <returns>Scanned line</returns>
    private ulong GetMovesForDirection(ulong player, ulong mask, int dir)
    {
        // check if opponent disk is next to player disk
        ulong flip = (((player << dir) | (player >> dir)) & mask);

        // shift seven times to find all opponent disks in line
        flip |= ((flip << dir) | (flip >> dir)) & mask;
        flip |= ((flip << dir) | (flip >> dir)) & mask;
        flip |= ((flip << dir) | (flip >> dir)) & mask;
        flip |= ((flip << dir) | (flip >> dir)) & mask;
        flip |= ((flip << dir) | (flip >> dir)) & mask;
        flip |= ((flip << dir) | (flip >> dir)) & mask;

        // do one more shift without masking to set the enlosing player disk
        return (flip << dir) | (flip >> dir);
    }


    /// <summary>
    ///     Get all possible moves in position coded as UInt64.
    /// </summary>
    /// <param name="player">Disks of current player.</param>
    /// <param name="opponent">Disks of the other player.</param>
    /// <returns>Moves</returns>
    private ulong GetMoves(ulong player, ulong opponent)
    {
        // empty fields to mask the enlosing player disks in scanned lins (= possible move)
        ulong empties = ~(player | opponent);
        ulong mask = opponent & m_boarderCutMask;
        return (GetMovesForDirection(player, mask, 1) | // vertical
                GetMovesForDirection(player, opponent, 8) | // horizontal
                GetMovesForDirection(player, mask, 7) | // diagonal (right top to left bottom)
                GetMovesForDirection(player, mask, 9)) & empties; // diagonal (left top to right bottom)
    }


    /// <summary>
    ///     print the board state in two dims, black stones: O, white stones: X, epmty field: -
    /// </summary>
    public void Dump()
    {
        ulong blackStones = m_blackStones;
        ulong whiteStones = m_whiteStones;

        StringBuilder sb = new StringBuilder();
        sb.Append("|");
        for (int i = 0; i < 64; i++)
        {
            if ((blackStones & 1) == 1)
                sb.Append("O");
            else if ((whiteStones & 1) == 1)
                sb.Append("X");
            else sb.Append("-");
            if ((i % 8 == 7) && (i < 63))
                sb.Append("|\n|");
            else if (i % 8 == 7)
                sb.Append("|\n");
            else
                sb.Append(" ");
            blackStones = blackStones >> 1;
            whiteStones = whiteStones >> 1;
        }

        Console.WriteLine(sb.ToString());
    }


    /// <summary>
    ///     Execte move: place new disk and flip opponent disks.
    /// </summary>
    /// <param name="move">Move position</param>
    /// <param name="flippedStones">Disks to be flipped</param>
    public void MakeMove(ulong move, ulong flippedStones)
    {
        if (PlayerPerspective == 0)
        {
            m_blackStones ^= (flippedStones | move);
            m_whiteStones ^= flippedStones;
        }
        else
        {
            m_blackStones ^= flippedStones;
            m_whiteStones |= (flippedStones | move);
        }

        PlayerPerspective = 1 - PlayerPerspective;
    }


    /// <summary>
    ///     Undo the last move.
    /// </summary>
    /// <param name="move">Move position</param>
    /// <param name="flippedStones">Disks to be flipped</param>
    public void UndoMove(ulong move, ulong flippedStones)
    {
        if (PlayerPerspective == 0)
        {
            m_blackStones ^= flippedStones;
            m_whiteStones ^= (flippedStones | move);
        }
        else
        {
            m_blackStones ^= (flippedStones | move);
            m_whiteStones ^= flippedStones;
        }

        PlayerPerspective = 1 - PlayerPerspective;
    }


    /// <summary>
    ///     Check, if there is a move possible for one side (if not: game over).
    /// </summary>
    /// <returns>True: game over, false: game not over</returns>
    public bool IsGameOver()
    {
        return (GetMoves(m_blackStones, m_whiteStones) | GetMoves(m_whiteStones, m_blackStones)) == 0;
    }

   

    /// <summary>
    ///     Return number of black disks and number of white disks in the momentary state.
    /// </summary>
    /// <returns></returns>
    public (int, int) GetStoneBalance()
    {
        return (BitOperations.PopCount(m_blackStones), BitOperations.PopCount(m_whiteStones));
    }


    /// <summary>
    ///     Gets the mobility of the indicated stone.
    /// </summary>
    /// <param name="isBlackStone">We look for black tone or not.</param>
    /// <returns>The movement options the stone has.</returns>
    public int GetMobility(bool isBlackStone)
    {
        return isBlackStone
            ? BitOperations.PopCount(GetMoves(m_blackStones, m_whiteStones))
            : BitOperations.PopCount(GetMoves(m_whiteStones, m_blackStones));
    }


    #region Manipulator

    public bool IsRandomManipulator => false;


    /// <summary>
    ///     Player perspective. 0: player black (starting), 1: player white
    /// </summary>
    public int PlayerPerspective { get; private set; }

    public int MaxNumOfProbabilityChoices => 0;


    /// <summary>
    ///     Get all possible moves.
    /// </summary>
    /// <param name="probabilities"></param>
    /// <returns></returns>
    public IList<ICommand> GetMoveOptionsWithProbabilities(in Span<float> probabilities)
    {
        ulong player;
        ulong opponent;

        if (PlayerPerspective == 0)
        {
            player = m_blackStones;
            opponent = m_whiteStones;
        }
        else
        {
            player = m_whiteStones;
            opponent = m_blackStones;
        }

        ulong moves = GetMoves(player, opponent);

        if (moves == 0ul)
            return [new BitMoveCommand(moves, 0ul, this)];

        List<ICommand> moveList = new List<ICommand>(BitOperations.PopCount(moves));

        int movePos;
        while (moves != 0)
        {
            movePos = BitOperations.TrailingZeroCount(moves); // position of the last 1
            ulong flippedDisks = FlipBit.flipOperations[movePos](player, opponent);
            moveList.Add(new BitMoveCommand(1UL << movePos, flippedDisks, this));
            moves = moves & (moves - 1); // set last one to zero
        }

        return moveList;
    }

    #endregion Manipulator


    #region HashableGameState

    public bool IsCurrentlyHashable => true;


    /// <summary>
    ///     Append to hash table.
    /// </summary>
    public void AppendData(BinaryWriter writer)
    {
        writer.Write(m_blackStones);
        writer.Write(m_whiteStones);
        writer.Write(PlayerPerspective);
    }

    #endregion HashableGameState


#pragma warning disable CA1416

    #region Paintable

    public bool WantsScreenshot => true;


    public void PaintElement(Graphics graphics, int imageWidth, int imageHeight)
    {
        using Pen drawingPen = new Pen(Color.Black, 1);
        using Font drawFont = new Font("Arial", 14);

        int boardSize = 8;

        // minimal margin to the edges of the image
        float margin = 20f;

        float canvasSize = Math.Min(imageHeight * 9 / 10, imageWidth) - margin * 2;
        float startWidth = Math.Max(margin, (imageWidth - canvasSize) / 2f);
        float cellSize = (canvasSize / boardSize);

        using Brush blackStoneBrush = new SolidBrush(Color.Black);
        using Brush whiteStoneBrush = new SolidBrush(Color.White);
        using Brush boardBrush = new SolidBrush(Color.FromArgb(120, 142, 211));

        // background color for the image
        graphics.Clear(Color.FromArgb(240, 240, 240));

        // draw background color of the board
        graphics.FillRectangle(boardBrush, startWidth, margin, canvasSize, canvasSize);

        // player
        ulong blackStones = m_blackStones;
        ulong whiteStones = m_whiteStones;

        var points = new PointF[2];
        for (int x = 0; x < boardSize; x++)
        {
            // draw horizontal lines
            points[0] = new PointF(startWidth, margin + x * cellSize);
            points[1] = new PointF(startWidth + canvasSize, margin + x * cellSize);
            graphics.DrawLines(drawingPen, points);

            // draw vertical lines
            points[0] = new PointF(startWidth + cellSize * x, margin);
            points[1] = new PointF(startWidth + cellSize * x, margin + canvasSize);
            graphics.DrawLines(drawingPen, points);

            // draw stones
            // swap axis (y, x) for ellipsis (graphic: x axis is horizontal, board: x-axis is vertical)
            for (int y = 0; y < boardSize; y++)
            {
                if ((blackStones & 1ul) == 1)
                    graphics.FillEllipse(blackStoneBrush,
                        startWidth + cellSize * y + 2,
                        margin + cellSize * x + 2,
                        cellSize - 4,
                        cellSize - 4);
                if ((whiteStones & 1) == 1)
                    graphics.FillEllipse(whiteStoneBrush,
                        startWidth + cellSize * y + 2,
                        margin + cellSize * x + 2,
                        cellSize - 4,
                        cellSize - 4);
                blackStones = blackStones >> 1;
                whiteStones = whiteStones >> 1;
            }
        }

        // draw last horizontal line
        points[0] = new PointF(startWidth, 20f + canvasSize);
        points[1] = new PointF(startWidth + canvasSize, 20f + canvasSize);
        graphics.DrawLines(drawingPen, points);

        // draw last vertical line
        points[0] = new PointF(startWidth + canvasSize, 20f);
        points[1] = new PointF(startWidth + canvasSize, 20f + canvasSize);
        graphics.DrawLines(drawingPen, points);

        // draw textual information
        string currentPlayer = PlayerPerspective == 0 ? "black" : "white";
        graphics.DrawString($"Current player: {currentPlayer}", drawFont, blackStoneBrush, startWidth,
            margin + canvasSize + 20f);
        (int numBlackStones, int numWhiteStones) = GetStoneBalance();
        graphics.DrawString($"Black: {numBlackStones} White: {numWhiteStones}", drawFont, blackStoneBrush, startWidth,
            margin + canvasSize + 40f);
    }

    #endregion Paintable

#pragma warning restore CA1416
}