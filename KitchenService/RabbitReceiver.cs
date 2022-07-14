using Messaging;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

namespace KitchenService
{
	public class RabbitReceiver : IHostedService
	{
		private readonly RabbitMQSettings _rabbitSettings;
		private readonly IModel _channel;
		private readonly IHubContext<OrderHub> _orderHub;
		public RabbitReceiver(RabbitMQSettings rabbitSettings, IModel channel, IHubContext<OrderHub> hub)
		{
			_rabbitSettings = rabbitSettings;
			_channel = channel;
			_orderHub = hub;
		}

		
		public Task StartAsync(CancellationToken cancellationToken)
		{
			DoStuff();
			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			_channel.Dispose();
			return Task.CompletedTask;
		}

		private void DoStuff()
		{
			_channel.ExchangeDeclare(exchange: _rabbitSettings.ExchangeName,
				type: _rabbitSettings.ExchangeType);

			var queueName = _channel.QueueDeclare().QueueName;


			_channel.QueueBind(queue: queueName,
							  exchange: _rabbitSettings.ExchangeName,
							  routingKey: "order.cookwaffle");


			var consumerAsync = new AsyncEventingBasicConsumer(_channel);
            consumerAsync.Received += async (model, ea) =>
			{
				var body = ea.Body.ToArray();
				var message = Encoding.UTF8.GetString(body);
				var order = JsonSerializer.Deserialize<Order>(message);
				await _orderHub.Clients.All.SendAsync("new-order", order);

                _channel.BasicAck(ea.DeliveryTag, false);
            };

			_channel.BasicConsume(queue: queueName,
								 autoAck: false,
								 consumer: consumerAsync);			
		}
	}
}
