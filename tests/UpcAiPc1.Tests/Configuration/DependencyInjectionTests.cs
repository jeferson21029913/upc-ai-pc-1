using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using UpcAiPc1.Application;
using UpcAiPc1.Application.Services;
using UpcAiPc1.Domain.Interfaces;
using UpcAiPc1.Infrastructure;
using UpcAiPc1.Infrastructure.Repositories;

namespace UpcAiPc1.Tests.Configuration;

public sealed class DependencyInjectionTests
{
    [Fact]
    public void whenAddApplicationIsCalledThenResolveJsonPayloadServiceReturnRegisteredImplementation()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddSingleton(Mock.Of<IJsonSourceRepository>());
        services.AddApplication();

        // Act
        var provider = services.BuildServiceProvider();
        var service = provider.GetService<IJsonPayloadService>();

        // Assert
        Assert.NotNull(service);
        Assert.IsType<JsonPayloadService>(service);
    }

    [Fact]
    public void whenAddInfrastructureIsCalledThenResolveJsonSourceRepositoryReturnRegisteredImplementation()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddSingleton<IHostEnvironment>(new TestHostEnvironment());
        services.AddInfrastructure();

        // Act
        var provider = services.BuildServiceProvider();
        var service = provider.GetService<IJsonSourceRepository>();

        // Assert
        Assert.NotNull(service);
        Assert.IsType<FileJsonSourceRepository>(service);
    }

    private sealed class TestHostEnvironment : IHostEnvironment
    {
        public string EnvironmentName { get; set; } = "Test";
        public string ApplicationName { get; set; } = "UpcAiPc1.Tests";
        public string ContentRootPath { get; set; } = Path.GetTempPath();
        public Microsoft.Extensions.FileProviders.IFileProvider ContentRootFileProvider { get; set; } =
            null!;
    }
}
