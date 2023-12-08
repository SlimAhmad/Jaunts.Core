namespace Jaunts.Core.Api.Brokers.Queues
{
    public partial interface IQueueBroker
    {
         void ListenToStudentsQueueAsync(Func<string, CancellationToken, Task> eventHandler);
    }
}
