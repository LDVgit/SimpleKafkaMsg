namespace Consumer
{
    using Confluent.Kafka;

    public class ConsumerService : BackgroundService
    {
        private readonly ILogger<ConsumerService> _log;
        private readonly IConsumer<string, string> _consumer;

        public ConsumerService(ILogger<ConsumerService> logger, IConsumer<string, string> consumer)
        {
            _log = logger;
            _consumer = consumer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Yield();
            
            var i = 0;
            while (!stoppingToken.IsCancellationRequested)
            {
                var consumeResult = _consumer.Consume(stoppingToken);
                
                _log.LogInformation(consumeResult.Message.Key + " - " + consumeResult.Message.Value);
                
                if (i++ % 1000 == 0)
                {
                    _consumer.Commit();
                }
            }
        }

        public override void Dispose()
        {
            _consumer.Dispose();
            base.Dispose();
        }
    }
}