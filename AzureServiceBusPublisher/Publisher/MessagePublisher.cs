using Azure.Messaging.ServiceBus;
using AzureServiceBus.Contracts;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace AzureServiceBusPublisher.Publisher
{
    public class MessagePublisher : IMessagePublisher
    {
        private readonly AppConfig _settings;

        public MessagePublisher(IOptionsMonitor<AppConfig> settings)
        {
            _settings = settings.CurrentValue;
        }

        public async Task PublishQueueAsync<T>(T message, string queueName)
        {
            // Create the clients that we'll use for sending and processing messages.
            var client = new ServiceBusClient(_settings.ConnectionStrings.ServiceBus);
            var sender = client.CreateSender(queueName);

            // create a batch 
            using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();

            var msgBody = JsonConvert.SerializeObject(message);

            //for (int i = 1; i <= numOfMessages; i++)
            //{
            // try adding a message to the batch
            var msg = new ServiceBusMessage(msgBody);
            msg.ApplicationProperties.Add("message_type", typeof(T).Name);

            if (!messageBatch.TryAddMessage(msg))
            {
                // if it is too large for the batch
                throw new Exception($"The message {msg} is too large to fit in the batch.");
            }
            //}

            try
            {
                // Use the producer client to send the batch of messages to the Service Bus queue
                await sender.SendMessagesAsync(messageBatch);
                //Console.WriteLine($"A batch of {numOfMessages} messages has been published to the queue.");
            }
            finally
            {
                // Calling DisposeAsync on client types is required to ensure that network
                // resources and other unmanaged objects are properly cleaned up.
                await sender.DisposeAsync();
                await client.DisposeAsync();
            }
        }

        public async Task PublishTopicAsync<T>(T message, string topicName)
        {
            // Create the clients that we'll use for sending and processing messages.
            var client = new ServiceBusClient(_settings.ConnectionStrings.ServiceBus);
            var sender = client.CreateSender(topicName);

            // create a batch 
            using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();

            var msgBody = JsonConvert.SerializeObject(message);

            //for (int i = 1; i <= numOfMessages; i++)
            //{
            // try adding a message to the batch
            var msg = new ServiceBusMessage(msgBody);
            msg.ApplicationProperties.Add("message_type", typeof(T).Name);


            if (!messageBatch.TryAddMessage(msg))
            {
                // if it is too large for the batch
                throw new Exception($"The message {msgBody} is too large to fit in the batch.");
            }
            //}

            try
            {
                // Use the producer client to send the batch of messages to the Service Bus queue
                await sender.SendMessagesAsync(messageBatch);
                //Console.WriteLine($"A batch of {numOfMessages} messages has been published to the queue.");
            }
            finally
            {
                // Calling DisposeAsync on client types is required to ensure that network
                // resources and other unmanaged objects are properly cleaned up.
                await sender.DisposeAsync();
                await client.DisposeAsync();
            }
        }
    }
}
