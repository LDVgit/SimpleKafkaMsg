namespace Consumer
{
    using Confluent.Kafka;

    public static class ConfigKafka
    {
        public static void ConfigureKafka(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<KafkaOptions>
                (configuration.GetSection("KafkaConfig"));

            var kafkaOptions = configuration
                .GetSection("KafkaConfig")
                .Get<KafkaOptions>();

            var newConsumerConfig = new ConsumerConfig
            {
                // если есть еще с таким же GroupId, 
                // события делятся автоматически, nfr;t можно управлять как читать partitions
                GroupId = kafkaOptions.GroupId,
                
                // коммит на пачку
                // коммит на 1 сообщение
                // коммит значит прочтено
                // есть StoreOffset(consumeResult)
                
                // EnableAutoCommit автокоммит, теряется прозрачность обработки. .StoreOffset(..)
                // не нужно вызывать
                EnableAutoCommit = kafkaOptions.EnableAutoCommit,
                AutoCommitIntervalMs = kafkaOptions.AutoCommitIntervalMs,// как часто автокоммит
                
                BootstrapServers = kafkaOptions.BootstrapServers,
                
                // с какого сообщения читать
                AutoOffsetReset = kafkaOptions.AutoOffsetReset,
                SecurityProtocol = SecurityProtocol.Plaintext,
            };

            var consumer = new ConsumerBuilder<string, string>(newConsumerConfig).Build();

            services.AddSingleton<IConsumer<string,string>>(provider =>
                new ConsumerBuilder<string, string>(newConsumerConfig).Build());

            consumer.Subscribe("order");
            services.AddHostedService(sp => new ConsumerService(sp.GetRequiredService<ILogger<ConsumerService>>(), consumer));
        }
    }
}