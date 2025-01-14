using WargameExplainer.Strategy;

namespace WargameExplainer.SampleGames.OthelloBit;

public readonly struct BitMoveCommand(ulong move, ulong flipped, BitBoardState state) : ICommand
{
    public void Execute()
    {
        state.MakeMove(move, flipped);
    }

    public void Undo()
    {
        state.UndoMove(move, flipped);
    }
}