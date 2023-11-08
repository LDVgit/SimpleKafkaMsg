namespace Producer
{
    using Confluent.Kafka;

    public class OrderRequest
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public OrderStatus Status { get; set; }
    }
}