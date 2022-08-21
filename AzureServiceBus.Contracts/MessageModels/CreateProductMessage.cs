namespace AzureServiceBus.Contracts.MessageModels
{
    public class CreateProductMessage
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Quantity { get; set; }

    }
}
