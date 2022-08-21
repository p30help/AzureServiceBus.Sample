namespace AzureServiceBusConsumer
{
    public class AppConfig
    {
        public ConnectionStringConfig ConnectionStrings { get; set; }
    }

    public class ConnectionStringConfig
    {
        public string ServiceBus { get; set; }
    }
}
