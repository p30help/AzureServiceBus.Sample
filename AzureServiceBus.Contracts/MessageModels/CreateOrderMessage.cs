namespace AzureServiceBus.Contracts.MessageModels
{
    public class CreateOrderMessage
    {
        public Guid Id { get; set; }

        public DateTime RecordDate { get; set; }

        public int UserId { get; set; }

        public string Address { get; set; }

    }
}
