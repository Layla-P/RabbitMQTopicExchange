using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace Messaging;
public static class IServiceCollectionExtensions
{

    public static IServiceCollection SetUpRabbitMq(this IServiceCollection services, IConfiguration config)
    {
        return services; 
    }

    public class ModelFactory : IDisposable
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
