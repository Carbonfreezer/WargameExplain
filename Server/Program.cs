// See https://aka.ms/new-console-template for more information

using Server;

Console.WriteLine("Server");

// This project is the server module for training models and  for analyzing a bulk check.
// This program dispatches play command to the diverse clients, that may reside on the same or a different computer.

// BaseRunner runner = new BaseRunner(true, "OthelloModel.mdl");
// BaseRunner runner = new BaseRunner(false, null );

// BaseRunner runner = new BaseRunner(true, "OthelloModelBalanced.mdl");
// BaseRunner runner = new BaseRunner(false, "DefensiveSimple.mdl");

BaseRunner runner = new BaseRunner(false, "Night.mdl");

runner.Run();
