using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Framing;
using SagaShared.Instance;

namespace SagaMng.Publisher
{
    public class Publisher : IPublisher
    {
        private IModel _model;

        public Publisher(IModel model)
        {
            _model = model;            
        }

        public Task Publish(SagaActivity data)
        {
            _model.ExchangeDeclare(exchange: "sagainstance_exchange", type: "direct", durable: true, autoDelete: false);
            var queueDeclareOk = _model.QueueDeclare(queue: "sagainstance_queue", durable: true, exclusive: true, autoDelete: false);
            _model.QueueBind("sagainstance_queue","sagainstance_exchange","");
            var properties = new BasicProperties()
            {
                Persistent = true,
                DeliveryMode = 2,
                ContentEncoding = "application/json",
                CorrelationId = data.SagaId.ToString()
            };
            var jsonData = JsonConvert.SerializeObject(data);
            var bytes = Encoding.UTF8.GetBytes(jsonData);
            _model.BasicPublish(exchange: "sagainstance_exchange", routingKey: "", basicProperties: properties, body: bytes);
            return Task.CompletedTask;
        }
    }
}