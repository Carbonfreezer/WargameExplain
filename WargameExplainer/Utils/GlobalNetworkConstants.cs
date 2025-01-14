using WargameExplainer.SampleGames.OthelloBit.Training;
using WargameExplainer.SampleGames.TakeThatHill.Training;
using WargameExplainer.TrainingSystem;

namespace WargameExplainer.Utils;

/// <summary>
///     Contains some constants that are shared between projects.
/// </summary>
public static class GlobalNetworkConstants
{
    /// <summary>
    ///     The global port number of the server the clients send discovery packets to.
    /// </summary>
    public const int DiscoveryHostPort = 7775;

    /// <summary>
    ///     The address where the server resides on. You may use 127.0.0.1 for loopback or also 255.255.255.255 for broadcast
    ///     discovery if broadcast is not blocked on your network.
    /// </summary>
    public const string ServerAddress = "127.0.0.1";

    /// <summary>
    ///     Gets the training info for the current game.
    /// </summary>
    public static TrainingInfoProvider GenerateInfoProvider()
    {
        // return new TrainingProviderOthelloBitBalanced();
        // return new TrainingProviderOthelloBit();
        // return HighLevelFunctions.GetSparringProvider<TrainingProviderOthelloBit, TrainingProviderOthelloBit>(
        //    "OthelloModel.mdl", null, 1);

        // return HighLevelFunctions.GetSparringProvider<TrainingProviderOthelloBit, TrainingProviderOthelloBitBalanced>(
        //     "OthelloModel.mdl", "OthelloModelBalanced.mdl", 0);

        // return new TrainingProviderTakeThatHillDefensive();
        return new TrainingProviderTakeThatHillNight();
    }
}