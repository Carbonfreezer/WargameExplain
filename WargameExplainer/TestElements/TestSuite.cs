using WargameExplainer.Board;
using WargameExplainer.DrawingSystem;
using WargameExplainer.SampleGames.BettingGame;
using WargameExplainer.SampleGames.BettingGame.Training;
using WargameExplainer.SampleGames.CoopGame;
using WargameExplainer.SampleGames.TakeThatHill;
using WargameExplainer.SampleGames.TicTacToe;
using WargameExplainer.SampleGames.TicTacToe.Training;
using WargameExplainer.Strategy;
using WargameExplainer.TrainingSystem;
using WargameExplainer.Utils;

namespace WargameExplainer.TestElements;

public static class TestSuite
{
    /// <summary>
    ///     Tests the board game code.
    /// </summary>
    public static void TestBoardCoder()
    {
        // Section to test the board code.
        var startPoint = new CoordinatesOffset(1, 1).GetAxial(OrientationType.PointyTop, OffsetType.OddOffset);
        var endPoint = new CoordinatesOffset(6, 2).GetAxial(OrientationType.PointyTop, OffsetType.OddOffset);

        Console.WriteLine("Line check.");
        foreach (var linePoint in BoardCoder.LinePoints(startPoint, endPoint, true))
        {
            var
                boardPosition = new CoordinatesOffset(linePoint, OrientationType.PointyTop, OffsetType.OddOffset);
            Console.WriteLine($"Column: {boardPosition.Col}, Row: {boardPosition.Row}");
        }

        Console.WriteLine("Range check.");
        foreach (var rangePoint in BoardCoder.RangeFinder(startPoint, 1))
        {
            var
                boardPosition = new CoordinatesOffset(rangePoint, OrientationType.PointyTop, OffsetType.OddOffset);
            Console.WriteLine($"Column: {boardPosition.Col}, Row: {boardPosition.Row}");
        }
    }

    private static void TestGameBoard(GameBoardLayout layout)
    {
        PieceCollection board = new PieceCollection(layout, 20);

        DummyPawn pawnA = new DummyPawn(1, layout, new CoordinatesOffset(1, 1));
        DummyPawn pawnB = new DummyPawn(2, layout, new CoordinatesOffset(4, 2));
        DummyPawn pawnC = new DummyPawn(3, layout, new CoordinatesOffset(2, 1));

        board.PlaceElement(pawnA);
        board.PlaceElement(pawnB);
        board.PlaceElement(pawnC);

        foreach (IPositional element in board.GetElements(pawnA.Position))
            Console.WriteLine($"Pawn found: {((DummyPawn)element).Number}");

        board.RemoveElement(pawnA);
        foreach (IPositional element in board.GetElements(pawnA.Position))
            Console.WriteLine($"Pawn found: {((DummyPawn)element).Number}");

        foreach (CoordinatesCube cube in board.GetWalkingPositions(pawnA, 5, true))
        {
            CoordinatesOffset offset = new CoordinatesOffset(cube, layout.Orientation, layout.Offset);
            Console.WriteLine($"Walkable Position: Col: {offset.Col} Row: {offset.Row}");
        }

        var walkableArea = BoardCoder.GetNeighbors(pawnB.Position);
        var transformedArea =
            walkableArea.Select(area => new CoordinatesOffset(area, layout.Orientation, layout.Offset));
        foreach (CoordinatesOffset offset in transformedArea)
            Console.WriteLine($"Neigbour Field Col: {offset.Col} Row: {offset.Row}");
    }

    public static void TestGameBoardRectangular()
    {
        GameBoardLayoutRectangular layout =
            new GameBoardLayoutRectangular(OrientationType.PointyTop, OffsetType.OddOffset, 6, 3);
        TestGameBoard(layout);
    }

    public static void TestGameBoardFree()
    {
        GameBoardLayoutFree layout = new GameBoardLayoutFree(OrientationType.PointyTop, OffsetType.OddOffset, 10);
        for (int i = 1; i <= 5; ++i)
        {
            layout.AddTile(i, 1);
            layout.AddTile(i, 2);
            layout.AddTile(i, 3);
        }

        layout.AddTile(6, 2);
        TestGameBoard(layout);
    }

    public static void TestBoardPainter()
    {
        GameBoardLayoutFree layout = new GameBoardLayoutFree(OrientationType.PointyTop, OffsetType.OddOffset, 10);
        for (int i = 1; i <= 5; ++i)
        {
            layout.AddTile(i, 1);
            layout.AddTile(i, 2);
            layout.AddTile(i, 3);
        }

        layout.AddTile(6, 2);

        PieceCollection board = new PieceCollection(layout, 20);

        DummyPawn pawnA = new DummyPawn(1, layout, new CoordinatesOffset(1, 1));
        DummyPawn pawnB = new DummyPawn(2, layout, new CoordinatesOffset(4, 2));
        DummyPawn pawnC = new DummyPawn(3, layout, new CoordinatesOffset(2, 1));

        board.PlaceElement(pawnA);
        board.PlaceElement(pawnB);
        board.PlaceElement(pawnC);

        using DebugPainter painter = new DebugPainter(1024, 768);
        GameBoardPainter boardPainter = new GameBoardPainter(layout, board);
        painter.ClearImage();
        painter.GenerateSnapShot("Test.png", boardPainter);
    }

    public static void TestSetupTake()
    {
        TakeThatHillGameState allBoard = new TakeThatHillGameState(false);
        DebugPainter painter = new DebugPainter(1024, 768);
        painter.GenerateSnapShot("Test.png", allBoard);
    }


    /// <summary>
    ///     Static invocation to get the coop game running.
    /// </summary>
    public static void RunGameCoop()
    {
        Console.WriteLine("Without voting");
        CoopGamestate game = new CoopGamestate(false);
        game.Run();
        Console.WriteLine("With voting");
        game = new CoopGamestate(true);
        game.Run();
    }

    /// <summary>
    ///     Plays the betting game with the indicated model.
    /// </summary>
    /// <param name="modelName">model to play with.</param>
    public static void PlayBettingGame(string modelName)
    {
        TrainingProviderBetting trainingInfo = new TrainingProviderBetting();
        LinearModel model = new LinearModel();
        model.LoadFromFile(modelName);
        trainingInfo.LinearModel = model;

        using StrategicDecider strat = trainingInfo.GetFreshGame(null);
        Span<float> dummy = stackalloc float[strat.NumOfPlayerPerspectives];

        while (!strat.IsGameOverAndAfterStateValue(dummy))
        {
            strat.ExecuteNewMove();
            if (((BettingGameState)strat.GameState).m_waitingForEvaluation)
                Console.WriteLine($"Placed bet {((BettingGameState)strat.GameState).m_betPlaced}");
        }
    }

    /// <summary>
    ///     Plays the tic tac toe game with the indicated model.
    /// </summary>
    /// <param name="modelName">model to play with.</param>
    /// <exception cref="NotImplementedException"></exception>
    public static void PlayTicTacToe(string modelName)
    {
        TrainingProviderTicTacToe trainingInfo = new TrainingProviderTicTacToe();
        LinearModel model = new LinearModel();
        model.LoadFromFile(modelName);
        trainingInfo.LinearModel = model;

        using StrategicDecider strat = trainingInfo.GetFreshGame(null);
        Span<float> dummy = stackalloc float[strat.NumOfPlayerPerspectives];

        while (!strat.IsGameOverAndAfterStateValue(dummy))
        {
            strat.ExecuteNewMove();
            ((GameState)(strat.GameState)).Dump();
        }
    }

    public static void PlayTicTacToeAgainstDumpSystem(string modelName)
    {
        
        TrainingInfoProvider sparrer =
            HighLevelFunctions.GetSparringProvider<TrainingProviderTicTacToe, TrainingProviderTicTacToe>(modelName,
                null, 1);

        using StrategicDecider strat = sparrer.GetFreshGame(null);
        Span<float> dummy = stackalloc float[strat.NumOfPlayerPerspectives];

        while (!strat.IsGameOverAndAfterStateValue(dummy))
        {
            strat.ExecuteNewMove();
            ((GameState)(strat.GameState)).Dump();
        }

    }
}