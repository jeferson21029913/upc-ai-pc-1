using Microsoft.Extensions.DependencyInjection;
using UpcAiPc1.Domain.Interfaces;
using UpcAiPc1.Infrastructure.Repositories;

namespace UpcAiPc1.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IJsonSourceRepository, FileJsonSourceRepository>();
        return services;
    }
}
