using Proto;

var system = new ActorSystem();
var props = Props.FromProducer(() => new HelloActor());
var pid = system.Root.Spawn(props);
system.Root.Send(pid, new Hello("Swak"));
Console.ReadLine();

record Hello (string Who);

class HelloActor : IActor
{
    public Task ReceiveAsync(IContext context)
    {
        var msg = context.Message;

        if (msg is Hello helloMsg)
        {
            Console.WriteLine($"Hello {helloMsg.Who}");
        }
        return Task.CompletedTask;
    }
}