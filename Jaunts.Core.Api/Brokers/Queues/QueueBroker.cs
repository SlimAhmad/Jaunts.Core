using Azure.Messaging.ServiceBus;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Jaunts.Core.Api.Brokers.Queues
{
    public partial class QueueBroker : IQueueBroker
    {
        private readonly IConfiguration configuration;

        public QueueBroker(IConfiguration configuration)
        {
            this.configuration = configuration;
            InitializeQueueClients();
        }

        private void InitializeQueueClients()=>
            this.UserQueue = GetQueueClientAsync().CreateSender(nameof(this.UserQueue));
        
            

        private ServiceBusClient GetQueueClientAsync()
        {
            string connectionString =
                this.configuration.GetConnectionString("ServiceBusConnection");
            return new ServiceBusClient(connectionString);      
        }

        public ServiceBusMessage MapToMessage(string message)
        {
            return new ServiceBusMessage(message);
        }
        private ServiceBusProcessorOptions SetServiceBusProcessorOptions()
        {
            return new ServiceBusProcessorOptions()
            {
                AutoCompleteMessages = false,
                MaxConcurrentCalls = 1
            };
        }
        private async Task SetupProcessorAsync(string queueName)
        {
            var client = GetQueueClientAsync();
            var processor = client.CreateProcessor(queueName, SetServiceBusProcessorOptions());
            processor.ProcessMessageAsync += MessageHandler;
            processor.ProcessErrorAsync += ErrorHandler;
            await processor.StartProcessingAsync();
            
        }

        private async Task MessageHandler(ProcessMessageEventArgs args)
        {
            string body = args.Message.Body.ToString();

            // we can evaluate application logic and use that to determine how to settle the message.
            await args.CompleteMessageAsync(args.Message);
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            // the error source tells me at what point in the processing an error occurred
            Console.WriteLine(args.ErrorSource);
            // the fully qualified namespace is available
            Console.WriteLine(args.FullyQualifiedNamespace);
            // as well as the entity path
            Console.WriteLine(args.EntityPath);
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }

       
    }
}
