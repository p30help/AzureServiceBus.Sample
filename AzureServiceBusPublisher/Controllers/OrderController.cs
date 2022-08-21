using AzureServiceBus.Contracts;
using AzureServiceBus.Contracts.MessageModels;
using Microsoft.AspNetCore.Mvc;

namespace AzureServiceBusPublisher.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        
        private readonly ILogger<OrderController> _logger;
        private readonly IMessagePublisher _messagePublisher;

        public OrderController(ILogger<OrderController> logger, IMessagePublisher messagePublisher)
        {
            _logger = logger;
            _messagePublisher = messagePublisher;
        }

        /// <summary>
        /// Publish message in Queue
        /// </summary>
        /// <returns></returns>
        [HttpPost("CreateOrder")]
        public async Task CreateOrder()
        {
            // publish message
            var msg = new CreateOrderMessage()
            {
                Id = Guid.NewGuid(),
                UserId = 40,
                RecordDate = DateTime.Now,
                Address = "Tehran, Azadi Sq"
            };

            await _messagePublisher.PublishQueueAsync(msg, Consts.OrderQueueName);
        }
    }
}