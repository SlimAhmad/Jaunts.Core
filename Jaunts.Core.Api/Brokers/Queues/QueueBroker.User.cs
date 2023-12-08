using Azure.Messaging.ServiceBus;

namespace Jaunts.Core.Api.Brokers.Queues
{
    public partial class QueueBroker
    {
        public ServiceBusSender UserQueue { get; set; }

        public async void ListenToStudentsQueueAsync(Func<string, CancellationToken, Task> eventHandler)
        {
            Func<string, CancellationToken, Task> messageEventHandler =
                CompleteUserQueueMessageAsync(eventHandler);

            await SetupProcessorAsync(nameof(this.UserQueue));
        }

        private Func<string, CancellationToken, Task> CompleteUserQueueMessageAsync(
            Func<string, CancellationToken, Task> handler)
        {
            return async (message, token) =>
            {
                await handler(message, token);
                var serviceBusMessage = MapToMessage(message);
                await this.UserQueue.SendMessageAsync(serviceBusMessage);
            };
        }

    
    }
}
