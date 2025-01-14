// See https://aka.ms/new-console-template for more information

using Client;

Console.WriteLine("Client");

// This is a client that simply executes a game and sends the result
// back to the server.

BaseRunner baseRunner = new BaseRunner();
baseRunner.Run();
