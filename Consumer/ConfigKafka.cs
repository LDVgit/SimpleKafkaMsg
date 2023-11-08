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
                GroupId = kafkaOptions.GroupId,
                EnableAutoCommit = kafkaOptions.EnableAutoCommit,
                BootstrapServers = kafkaOptions.BootstrapServers,
                AutoCommitIntervalMs = kafkaOptions.AutoCommitIntervalMs,
                AutoOffsetReset = kafkaOptions.AutoOffsetReset,
                SecurityProtocol = SecurityProtocol.Plaintext
            };

            var consumer = new ConsumerBuilder<string, string>(newConsumerConfig).Build();

            services.AddSingleton<IConsumer<string,string>>(provider =>
                new ConsumerBuilder<string, string>(newConsumerConfig).Build());

            consumer.Subscribe("order");
            services.AddHostedService(sp => new ConsumerService(sp.GetRequiredService<ILogger<ConsumerService>>(), consumer));
        }
    }
}