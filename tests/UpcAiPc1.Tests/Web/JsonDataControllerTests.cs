using Microsoft.AspNetCore.Mvc;
using Moq;
using UpcAiPc1.Application.Exceptions;
using UpcAiPc1.Application.Services;
using UpcAiPc1.Web.Controllers;

namespace UpcAiPc1.Tests.Web;

public sealed class JsonDataControllerTests
{
    [Fact]
    public async Task whenPayloadServiceReturnsJsonThenGetReturnOkObjectResult()
    {
        // Arrange
        var serviceMock = new Mock<IJsonPayloadService>();
        serviceMock
            .Setup(x => x.GetPayloadAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(System.Text.Json.JsonDocument.Parse("{\"name\":\"demo\"}").RootElement.Clone());

        var controller = new JsonDataController(serviceMock.Object);

        // Act
        var result = await controller.Get(CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
    }

    [Fact]
    public async Task whenPayloadServiceThrowsInvalidSourceJsonExceptionThenGetReturnBadRequestObjectResult()
    {
        // Arrange
        var serviceMock = new Mock<IJsonPayloadService>();
        serviceMock
            .Setup(x => x.GetPayloadAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidSourceJsonException("mensaje de error"));

        var controller = new JsonDataController(serviceMock.Object);

        // Act
        var result = await controller.Get(CancellationToken.None);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("mensaje de error", ExtractMensaje(badRequest.Value));
    }

    [Fact]
    public async Task whenPayloadServiceThrowsFileNotFoundExceptionThenGetReturnNotFoundObjectResult()
    {
        // Arrange
        var serviceMock = new Mock<IJsonPayloadService>();
        serviceMock
            .Setup(x => x.GetPayloadAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new FileNotFoundException());

        var controller = new JsonDataController(serviceMock.Object);

        // Act
        var result = await controller.Get(CancellationToken.None);

        // Assert
        var notFound = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("No se encontro el archivo de origen JSON.", ExtractMensaje(notFound.Value));
    }

    private static string? ExtractMensaje(object? value)
    {
        if (value is null)
        {
            return null;
        }

        return value.GetType().GetProperty("mensaje")?.GetValue(value)?.ToString();
    }
}
