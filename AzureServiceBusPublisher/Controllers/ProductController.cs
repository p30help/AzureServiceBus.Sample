using AzureServiceBus.Contracts;
using AzureServiceBus.Contracts.MessageModels;
using Microsoft.AspNetCore.Mvc;

namespace AzureServiceBusPublisher.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        
        private readonly ILogger<ProductController> _logger;
        private readonly IMessagePublisher _messagePublisher;

        public ProductController(ILogger<ProductController> logger, IMessagePublisher messagePublisher)
        {
            _logger = logger;
            _messagePublisher = messagePublisher;
        }

        /// <summary>
        /// Publish message in Topic
        /// </summary>
        /// <returns></returns>
        [HttpPost("CreateProduct")]
        public async Task CreateProduct()
        {
            var msg = new CreateProductMessage()
            {
                Id = 1,
                Name = "Product-",
                Quantity = 400
            };

            await _messagePublisher.PublishTopicAsync(msg, Consts.ProductTopicName);
        }

        /// <summary>
        /// Publish message in Topic
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("DeleteProduct/{id}")]
        public async Task DeleteProduct(int id)
        {

            var msg = new DeleteProductMessage()
            {
                Id = id
            };

            await _messagePublisher.PublishTopicAsync(msg, Consts.ProductTopicName);
        }
    }
}