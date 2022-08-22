using Proto;
using Proto.Cluster;

namespace ProtoClusterTutorial;

public class GrainClient
{
    private readonly ActorSystem _actorSystem;

    public GrainClient(ActorSystem actorSystem)
    {
        _actorSystem = actorSystem;
    }

    public async Task TurnTheLightOnInTheKitchen(CancellationToken cancellationToken)
    {
        var smartBulbGrainClient = _actorSystem
            .Cluster()
            .GetSmartBulbGrain(identity: "kitchen");
        await smartBulbGrainClient.TurnOn(cancellationToken);
    }
}