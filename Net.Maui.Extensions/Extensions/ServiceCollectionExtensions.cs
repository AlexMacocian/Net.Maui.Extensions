using System.Core.Extensions;

namespace Net.Maui.Extensions.Extensions;

public static class ServiceCollectionExtensions
{
    public static IHttpClientBuilder RegisterHttpClient<TService>(this IServiceCollection services)
        where TService : class
    {
        return services.ThrowIfNull()
            .AddScoped<IHttpClient<TService>, HttpClient<TService>>(sp =>
            {
                var factory = sp.GetRequiredService<IHttpClientFactory>();
                return new HttpClient<TService>(factory.CreateClient(typeof(TService).Name), false);
            })
            .AddHttpClient(typeof(TService).Name);
    }
}
