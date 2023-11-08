namespace Producer.Controllers
{
    using Newtonsoft.Json;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("[controller]")]
    public class ProducerController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<ProducerController> _logger;
        private readonly IProducerService _producer;

        public ProducerController(ILogger<ProducerController> logger, IProducerService producer)
        {
            _logger = logger;
            _producer = producer;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody]OrderRequest value, CancellationToken ct)
        {
            string serializedOrder = JsonConvert.SerializeObject(value);

            var res = await _producer.SendMessage("order", serializedOrder, true, 1, ct);
            _logger.Log(LogLevel.Information, "Sended", res);
            return Ok(res);
        }
    }
}