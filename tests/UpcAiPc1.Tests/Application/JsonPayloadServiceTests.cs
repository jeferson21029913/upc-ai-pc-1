using System.Text.Json;
using Microsoft.Extensions.Logging;
using Moq;
using UpcAiPc1.Application.Exceptions;
using UpcAiPc1.Application.Services;
using UpcAiPc1.Domain.Interfaces;

namespace UpcAiPc1.Tests.Application;

public sealed class JsonPayloadServiceTests
{
    [Fact]
    public async Task whenSourceContainsValidObjectJsonThenGetPayloadAsyncReturnObjectElement()
    {
        // Arrange
        var repositoryMock = new Mock<IJsonSourceRepository>();
        repositoryMock
            .Setup(x => x.ReadSourceAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync("{\"name\":\"demo\"}");

        var loggerMock = new Mock<ILogger<JsonPayloadService>>();
        var service = new JsonPayloadService(repositoryMock.Object, loggerMock.Object);

        // Act
        var result = await service.GetPayloadAsync();

        // Assert
        Assert.Equal(JsonValueKind.Object, result.ValueKind);
        Assert.Equal("demo", result.GetProperty("name").GetString());
        VerifyLog(loggerMock, LogLevel.Information, "objeto unico");
    }

    [Fact]
    public async Task whenSourceContainsValidArrayJsonThenGetPayloadAsyncReturnArrayElement()
    {
        // Arrange
        var repositoryMock = new Mock<IJsonSourceRepository>();
        repositoryMock
            .Setup(x => x.ReadSourceAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync("[{\"id\":1}]");

        var loggerMock = new Mock<ILogger<JsonPayloadService>>();
        var service = new JsonPayloadService(repositoryMock.Object, loggerMock.Object);

        // Act
        var result = await service.GetPayloadAsync();

        // Assert
        Assert.Equal(JsonValueKind.Array, result.ValueKind);
        Assert.Equal(1, result.GetArrayLength());
        VerifyLog(loggerMock, LogLevel.Information, "arreglo");
    }

    [Fact]
    public async Task whenSourceContainsInvalidJsonThenGetPayloadAsyncReturnInvalidSourceJsonException()
    {
        // Arrange
        var repositoryMock = new Mock<IJsonSourceRepository>();
        repositoryMock
            .Setup(x => x.ReadSourceAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync("{invalid");

        var loggerMock = new Mock<ILogger<JsonPayloadService>>();
        var service = new JsonPayloadService(repositoryMock.Object, loggerMock.Object);

        // Act
        var action = () => service.GetPayloadAsync();

        // Assert
        var exception = await Assert.ThrowsAsync<InvalidSourceJsonException>(action);
        Assert.Equal("El archivo de origen no tiene un formato JSON valido.", exception.Message);
        VerifyLog(loggerMock, LogLevel.Error, "Fallo la lectura del archivo de origen como JSON.");
    }

    private static void VerifyLog<T>(
        Mock<ILogger<T>> loggerMock,
        LogLevel level,
        string expectedMessagePart)
    {
        loggerMock.Verify(
            x => x.Log(
                level,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((value, _) => value.ToString()!.Contains(expectedMessagePart)),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}
