using Azure.Messaging.ServiceBus;
using AzureServiceBus.Contracts;
using Microsoft.Extensions.Options;

namespace AzureServiceBusConsumer.BackgroundServices
{
    public class OrderCunsomerService : BackgroundService
    {

        private readonly AppConfig _settings;

        public OrderCunsomerService(IOptionsMonitor<AppConfig> settings)
        {
            _settings = settings.CurrentValue;
        }

        // the client that owns the connection and can be used to create senders and receivers
        ServiceBusClient _client;

        // the processor that reads and processes messages from the queue
        ServiceBusProcessor _processor;

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {

            // The Service Bus client types are safe to cache and use as a singleton for the lifetime
            // of the application, which is best practice when messages are being published or read
            // regularly.
            //

            // Create the client object that will be used to create sender and receiver objects
            _client = new ServiceBusClient(_settings.ConnectionStrings.ServiceBus);


            ////////////////////
            /// Way 1
            ////////////////////
            // 
            // while (true)
            // {
            // 
            //     // create a receiver that we can use to receive and settle the message
            //     ServiceBusReceiver receiver = _client.CreateReceiver(Consts.OrderQueueName);
            // 
            //     // the received message is a different type as it contains some service set properties
            //     ServiceBusReceivedMessage receivedMessage = await receiver.ReceiveMessageAsync();
            // 
            //     // get the message body as a string
            //     string body = receivedMessage.Body.ToString();
            //     Console.WriteLine(body);
            // 
            //     // complete the message, thereby deleting it from the service
            //     await receiver.CompleteMessageAsync(receivedMessage);
            // 
            //     break;
            // 
            // }
            // 
            ///////////////////
            /// End of way 1
            ///////////////////

            ////////////////////
            /// Way 2
            ////////////////////

            // create a processor that we can use to process the messages
            _processor = _client.CreateProcessor(Consts.OrderQueueName, new ServiceBusProcessorOptions()
            {
                MaxConcurrentCalls = 1,
                AutoCompleteMessages = false

            });

            try
            {
                // add handler to process messages
                _processor.ProcessMessageAsync += MessageHandler;

                // add handler to process any errors
                _processor.ProcessErrorAsync += ErrorHandler;

                // start processing 
                await _processor.StartProcessingAsync();

                Console.WriteLine("Wait for a minute and then press any key to end the processing");
                //Console.ReadKey();

                // stop processing 
                //Console.WriteLine("\nStopping the receiver...");
                //await _processor.StopProcessingAsync();
                //Console.WriteLine("Stopped receiving messages");
            }
            finally
            {
                // Calling DisposeAsync on client types is required to ensure that network
                // resources and other unmanaged objects are properly cleaned up.
                //await _processor.DisposeAsync();
                //await _client.DisposeAsync();
            }

            ///////////////////
            /// End of way 2
            ///////////////////
        }

        // handle received messages
        async Task MessageHandler(ProcessMessageEventArgs args)
        {
            string body = args.Message.Body.ToString();
            Console.WriteLine($"Received: {body}");

            // complete the message. messages is deleted from the queue. 
            await args.CompleteMessageAsync(args.Message);
        }

        // handle any errors when receiving messages
        Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }
    }
}
