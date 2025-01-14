using System.Diagnostics;
using WargameExplainer.Strategy;

namespace WargameExplainer.SampleGames.TicTacToe;

/// <summary>
///     Very simple tic tac toe game for tesing.
/// </summary>
public class GameState : IHashableGameState, IEvaluator, IManipulator
{
    /// <summary>
    ///     0 : Empty, 1 : Stone of player 0 2: STone of player 1
    /// </summary>
    public readonly int[,] m_gameBoard = new int[3, 3];

   
    /// <summary>
    ///     Puts a stone on the board and switches the player.
    /// </summary>
    /// <param name="player">Player index that places stone.</param>
    /// <param name="x">x coordinate of stone.</param>
    /// <param name="y">y coordinate of stone.</param>
    public void PlaceStone(int player, int x, int y)
    {
        Debug.Assert(player == PlayerPerspective, "Illegal game state.");
        m_gameBoard[x, y] = player + 1;
        PlayerPerspective = 1 - PlayerPerspective;
    }

    /// <summary>
    ///     Removes a stone from the board.
    /// </summary>
    /// <param name="x">x coordinate of stone to remove.</param>
    /// <param name="y">y coordinate of stone to remove.</param>
    public void RevokeStone(int x, int y)
    {
        m_gameBoard[x, y] = 0;
        PlayerPerspective = 1 - PlayerPerspective;
    }


    /// <summary>
    ///     Checks if a certain stone has a triple line.
    /// </summary>
    /// <param name="stone">Stone to look for.</param>
    /// <returns>If we have three stones in a row.</returns>
    private bool CheckForLines(int stone)
    {
        // check rows
        if (m_gameBoard[0, 0] == stone && m_gameBoard[0, 1] == stone && m_gameBoard[0, 2] == stone) return true;
        if (m_gameBoard[1, 0] == stone && m_gameBoard[1, 1] == stone && m_gameBoard[1, 2] == stone) return true;
        if (m_gameBoard[2, 0] == stone && m_gameBoard[2, 1] == stone && m_gameBoard[2, 2] == stone) return true;

        // check columns
        if (m_gameBoard[0, 0] == stone && m_gameBoard[1, 0] == stone && m_gameBoard[2, 0] == stone) return true;
        if (m_gameBoard[0, 1] == stone && m_gameBoard[1, 1] == stone && m_gameBoard[2, 1] == stone) return true;
        if (m_gameBoard[0, 2] == stone && m_gameBoard[1, 2] == stone && m_gameBoard[2, 2] == stone) return true;

        // check diags
        if (m_gameBoard[0, 0] == stone && m_gameBoard[1, 1] == stone && m_gameBoard[2, 2] == stone) return true;
        if (m_gameBoard[0, 2] == stone && m_gameBoard[1, 1] == stone && m_gameBoard[2, 0] == stone) return true;

        return false;
    }


    public void Dump()
    {
        Console.WriteLine("=================");
        for (int j = 0; j < 3; ++j)
        {
            for (int i = 0; i < 3; ++i)
                Console.Write(m_gameBoard[i, j] switch { 1 => "+", 2 => "-", _ => "o" });
            Console.WriteLine("");
        }
    }


    #region IHashableGameState

    /// <summary>
    ///     We are always hashable.
    /// </summary>
    public bool IsCurrentlyHashable => true;

    public void AppendData(BinaryWriter writer)
    {
        foreach (int element in m_gameBoard.Cast<int>()) writer.Write(element);
        writer.Write(PlayerPerspective);
    }

    #endregion

    #region Evaluator
    
    public int NumOfPlayerPerspectives => 2;
    public bool IsGameOverAndEvaluate(in Span<float> afterStateValues)
    {
        // Here we check for the winning condition.
        bool isWinning0 = CheckForLines(1);
        bool isWinning1 = CheckForLines(2);

        if (isWinning0)
        {
            afterStateValues[0] = 1.0f;
            afterStateValues[1] = -1.0f;
        }
        else if (isWinning1)
        {
            afterStateValues[0] = -1.0f;
            afterStateValues[1] = 1.0f;
        }
        else
        {
            afterStateValues[0] = 0.0f;
            afterStateValues[1] = 0.0f;
        }

        return isWinning0 || isWinning1 || (m_gameBoard.Cast<int>().Count(x => x == 0) == 0);
    }

    #endregion

    #region Manipulator

    public bool IsRandomManipulator => false;

    public IList<ICommand> GetMoveOptionsWithProbabilities(in Span<float> probabilities)
    {
        List<ICommand> result = new List<ICommand>(9);
        for (int i = 0; i < 3; ++i)
        for (int j = 0; j < 3; ++j)
            if (m_gameBoard[i, j] == 0)
                result.Add(new CommandPlaceStone(i, j, PlayerPerspective, this));

        return result;
    }

    /// <summary>
    ///     The player who is nect to play.
    /// </summary>
    public int PlayerPerspective { get; private set; }

    public int MaxNumOfProbabilityChoices => 0;

    #endregion
}