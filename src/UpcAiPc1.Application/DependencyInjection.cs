using Microsoft.Extensions.DependencyInjection;
using UpcAiPc1.Application.Services;

namespace UpcAiPc1.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IJsonPayloadService, JsonPayloadService>();
        return services;
    }
}
