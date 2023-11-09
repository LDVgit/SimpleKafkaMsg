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
                BootstrapServers = kafkaOptions.Host,
                Partitioner = Partitioner.Consistent, // раскладка в partitions
                Acks = Acks.Leader, // ответ от записи, запись в лидере брокера
                // есть минимально требуемые реплики для Acks.All
                // min.insync.replicas = (2 например)

                // нужен баланс с задержкой и пропускной способностью
                // отправка сообщений пачками
                BatchSize = 1,
                BatchNumMessages = 1,

                LingerMs = 5, // время накопления сообщений в пачке

                // еще вариант использовать сжатие
                CompressionType = CompressionType.None, // забивает cpu
            };

            services.AddSingleton<IProducer<string, string>>(provider => new ProducerBuilder<string, string>(config).Build());
            services.AddScoped<IProducerService, ProducerService>();
        }

        // Produce и ProduceAsync
        // Task API несколько затратный для чтения ответа от кафки,
        // callback для последовательной обработки. 

        // для чего компрессия - для больших сериализованных данных
        // жертва cpu в пользу диска(hdd), плотность записи выше
        // для разгрузки сети. Кафка берет сеть, сколько есть.
        // для передачи данных между дата центров

        // rabbit streaming
        // apache pulsar
        // apache rocketMq - alibabba строили
    }
}