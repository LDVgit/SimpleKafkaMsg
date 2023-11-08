namespace Producer
{
    using Confluent.Kafka;

    public enum OrderStatus
    {
        IN_PROGRESS,
        COMPLETED,
        REJECTED
    }
}