namespace Producer
{
    using Confluent.Kafka;
    using Producer.Services;

    public static class ConfigKafka
    {
        public static void ConfigureKafka(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<KafkaOptions>
                (configuration.GetSection("KafkaConfig"));

            var kafkaOptions = configuration
                .GetSection("KafkaConfig")
                .Get<KafkaOptions>();

            var config = new ProducerConfig
            {
                BootstrapServers = kafkaOptions.Host
            };

            services.AddSingleton<IProducer<string, string>>(provider => new ProducerBuilder<string, string>(config).Build());
            services.AddScoped<IProducerService, ProducerService>();
        }
    }
}