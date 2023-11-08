namespace Producer
{

    public interface IProducerService
    {
        Task<string> SendMessage(string topic, string order, bool display, int key, CancellationToken ct);
    }
}