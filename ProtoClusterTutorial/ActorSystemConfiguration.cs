using Proto;
using Proto.Cluster;
using Proto.Cluster.Partition;
using Proto.Cluster.Testing;
using Proto.DependencyInjection;
using Proto.Remote;
using Proto.Remote.GrpcNet;

namespace ProtoClusterTutorial.Configurations;

public static class ActorSystemConfiguration
{
    public static void AddActorSystem(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton(provider =>
        {
            var actorSystemConfig = ActorSystemConfig
                .Setup();

            var remoteConfig = GrpcNetRemoteConfig
                .BindToLocalhost()
                .WithProtoMessages(MessagesReflection.Descriptor);

            var clusterConfig = ClusterConfig
                .Setup(
                    "ProtoClusterToturial",
                    new TestProvider(new TestProviderOptions(), new InMemAgent()),
                    new PartitionIdentityLookup()
                )
                .WithClusterKind(
                    SmartBulbGrainActor.Kind,
                    Props.FromProducer(() => new SmartBulbGrainActor((context, identity) =>
                        new SmartBulbGrain(context, identity)))
                )
                .WithClusterKind(
                    SmartHouseGrainActor.Kind,
                    Props.FromProducer(() => new SmartHouseGrainActor((context, identity) =>
                        new SmartHouseGrain(context, identity)))
                );

            return new ActorSystem(actorSystemConfig)
                .WithServiceProvider(provider)
                .WithRemote(remoteConfig)
                .WithCluster(clusterConfig);
        });
    }
}