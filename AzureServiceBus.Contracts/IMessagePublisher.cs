namespace AzureServiceBus.Contracts
{
    public interface IMessagePublisher
    {
        public Task PublishQueueAsync<T>(T message, string queueName);

        public Task PublishTopicAsync<T>(T message, string topicName);
    }
}
