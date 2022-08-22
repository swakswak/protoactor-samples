using Proto;
using Proto.Cluster;
using ProtoClusterTutorial;
using ProtoClusterTutorial.Configurations;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddActorSystem();
builder.Services.AddHostedService<ActorSystemClusterHostedService>();
builder.Services.AddHostedService<SmartBulbSimulator>();

var app = builder.Build();
var loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();
Log.SetLoggerFactory(loggerFactory);
app.MapGet("/", () => Task.FromResult("Hello, Proto.Cluster!"));
app.MapGet("/smart-bulbs/{identity}", async (ActorSystem actorSystem, string identity) =>
    await actorSystem
        .Cluster()
        .GetSmartBulbGrain(identity)
        .GetState(CancellationToken.None));
app.Run();