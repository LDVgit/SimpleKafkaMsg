namespace Producer.Services
{
    using Confluent.Kafka;
    using Newtonsoft.Json;

    public class ProducerService : IProducerService
    {
        private readonly ILogger<ProducerService> _logger;
        private readonly IProducer<string, string> _producer;

        public ProducerService(ILogger<ProducerService> logger, IProducer<string, string> producer)
        {
            _logger = logger;
            _producer = producer;
        }
        
        public async Task<string> SendMessage(string topic, string order, bool display, int key, CancellationToken ct)
        {
            var orderObject = JsonConvert.DeserializeObject<OrderRequest>(order);

            var deliveryObject = new OrderDelivery { OrderRequestId = orderObject?.Id ?? 0, CurrentLocation = "Melbourne", DeliveredBy = "John Sena" };

            var message = JsonConvert.SerializeObject(deliveryObject);

            var msg = new Message<string, string>
            {
                Key = key.ToString(),
                Value = message
            };

            DeliveryResult<string, string> delRep;

            if (key > 1)
            {
                var p = new Partition(key);
                var tp = new TopicPartition(topic, p);
                delRep = await _producer.ProduceAsync(tp, msg, ct);
                _logger.Log(LogLevel.Information, "Send kafka", message);
            }
            else
            {
                delRep = await _producer.ProduceAsync(topic, msg, ct);
            }

            var topicOffset = delRep.TopicPartitionOffset;

            if (display) { Console.WriteLine($"Delivered '{delRep.Value}' to: {topicOffset}"); }

            return message;
        }
    }
}