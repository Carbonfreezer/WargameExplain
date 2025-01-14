// See https://aka.ms/new-console-template for more information

using Microsoft.VisualBasic.CompilerServices;
using WargameExplainer.SampleGames.BettingGame;
using WargameExplainer.SampleGames.BettingGame.Training;
using WargameExplainer.SampleGames.CoopGame;
using WargameExplainer.SampleGames.OthelloBit.Training;
using WargameExplainer.SampleGames.TakeThatHill;
using WargameExplainer.SampleGames.TakeThatHill.Training;
using WargameExplainer.SampleGames.TicTacToe;
using WargameExplainer.SampleGames.TicTacToe.Training;
using WargameExplainer.TestElements;
using WargameExplainer.Utils;

// TestSuite.TestBoardCoder();
// TestSuite.TestGameBoardRectangular();
// TestSuite.TestGameBoardFree();
// TestSuite.TestBoardPainter();
// TestSuite.TestSetupTake();

// GameRunner.RunGame();




// HighLevelFunctions.GenerateVerbalExplanation<TrainingProviderTakeThatHill>("DifferModel.mdl");
// HighLevelFunctions.GenerateExampleExplanation<TrainingProviderTakeThatHill>("DifferModel.mdl", 10, 1000, 600);
// HighLevelFunctions.GetResultStatisticsLocally<TrainingProviderTakeThatHill>("DifferModel.mdl");

// TestSuite.RunGameCoop();

// HighLevelFunctions.PerformTrainingLocally<TrainingProviderBetting>("Betting.mdl");
// HighLevelFunctions.GenerateVerbalExplanation<TrainingProviderBetting>("Betting.mdl");
// HighLevelFunctions.GetResultStatisticsLocally<TrainingProviderBetting>("Betting.mdl");
// TestSuite.PlayBettingGame("Betting.mdl");

// HighLevelFunctions.PerformTrainingLocally<TrainingProviderTicTacToe>("TicTacToe.mdl");
// HighLevelFunctions.GenerateVerbalExplanation<TrainingProviderTicTacToe>("TicTacToe.mdl");
// TestSuite.PlayTicTacToe("TicTacToe.mdl");
// TestSuite.PlayTicTacToeAgainstDumpSystem("TicTacToe.mdl");


// HighLevelFunctions.RunGameWithScreenshots<TrainingProviderOthelloBit>("OthelloModel.mdl");
// HighLevelFunctions.PerformTrainingLocally<TrainingProviderOthelloBit>("OthelloModel.mdl");
// HighLevelFunctions.GenerateVerbalExplanation<TrainingProviderOthelloBit>("OthelloModel.mdl");

// HighLevelFunctions.PerformTrainingLocally<TrainingProviderOthelloBitBalanced>("OthelloModelBalanced.mdl");
// HighLevelFunctions.GenerateVerbalExplanation<TrainingProviderOthelloBitBalanced>("OthelloModelBalanced.mdl");
// HighLevelFunctions.RunGameWithScreenshotsBoundary<TrainingProviderOthelloBitBalanced, TrainingProviderOthelloBit>("OthelloModelBalanced.mdl", "OthelloModel.mdl", 1);

// HighLevelFunctions.GetResultStatisticsLocally<TrainingProviderTakeThatHillDefensive>("Test.mdl");
// HighLevelFunctions.PerformTrainingLocally<TrainingProviderTakeThatHillDefensive>("Test.mdl");
// HighLevelFunctions.GenerateVerbalExplanation<TrainingProviderTakeThatHillDefensive>("DefensiveSimple.mdl");
// HighLevelFunctions.GenerateExampleExplanation<TrainingProviderTakeThatHillDefensive>("Defensive.mdl", 200, 1000, 600);

// HighLevelFunctions.PerformTrainingLocally<TrainingProviderTakeThatHillNight>("Night.mdl");
HighLevelFunctions.GenerateVerbalExplanation<TrainingProviderTakeThatHillNight>("Night.mdl");

Console.WriteLine("Finished");
Console.ReadLine();