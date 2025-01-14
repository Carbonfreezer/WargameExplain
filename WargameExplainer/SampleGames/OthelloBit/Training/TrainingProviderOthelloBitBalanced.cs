using WargameExplainer.TrainingSystem;

namespace WargameExplainer.SampleGames.OthelloBit.Training;

public class TrainingProviderOthelloBitBalanced : TrainingProviderOthelloBit
{
    public override IList<GameStateObserver> Observers =>
    [
        new DiagonalObserverBalanced( 0),
        new DiagonalObserverBalanced( 1),
        new SidePointObserverBalanced( 1),
        new SidePointObserverBalanced( 2),
        new SidePointObserverBalanced( 3),
        new InnerPointObserverBalanced(),
        new MobilityObserverBalanced()
    ];
}