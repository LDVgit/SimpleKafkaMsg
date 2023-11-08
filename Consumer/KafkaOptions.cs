namespace Consumer
{
    using Confluent.Kafka;

    public class KafkaOptions
    {
        public string GroupId { get; set; }
        public string BootstrapServers { get; set; }
        public int AutoCommitIntervalMs { get; set; }

        public AutoOffsetReset AutoOffsetReset { get; set; }
        public bool EnableAutoCommit { get; set; }
    }
}