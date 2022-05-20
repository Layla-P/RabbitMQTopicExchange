using Messaging;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

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

		
		public async Task StartAsync(CancellationToken cancellationToken)
		{
			await DoStuff();
			//return CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			_channel.Dispose();
			return Task.CompletedTask;
		}

		private async Task DoStuff()
		{
			_channel.ExchangeDeclare(exchange: _rabbitSettings.ExchangeName,
				type: _rabbitSettings.ExchangeType);

			var queueName = _channel.QueueDeclare().QueueName;


			_channel.QueueBind(queue: queueName,
							  exchange: _rabbitSettings.ExchangeName,
							  routingKey: "order.cookwaffle");


			Console.WriteLine(" [*] Waiting for messages. To exit press CTRL+C");

			var consumer = new EventingBasicConsumer(_channel);
			consumer.Received += async (model, ea) =>
			{
				var body = ea.Body.ToArray();
				var message = Encoding.UTF8.GetString(body);
				var order = JsonSerializer.Deserialize<Order>(message);
				var routingKey = ea.RoutingKey;
				await _orderHub.Clients.All.SendAsync("new-order", order);
			};
			_channel.BasicConsume(queue: queueName,
								 autoAck: true,
								 consumer: consumer);

			Console.WriteLine(" Press [enter] to exit.");
			Console.ReadLine();
		}
	}
}
