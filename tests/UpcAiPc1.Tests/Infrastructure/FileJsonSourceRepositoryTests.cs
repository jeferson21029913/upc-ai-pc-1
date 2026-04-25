using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moq;
using UpcAiPc1.Infrastructure.Repositories;

namespace UpcAiPc1.Tests.Infrastructure;

public sealed class FileJsonSourceRepositoryTests
{
    [Fact]
    public async Task whenSourceFileExistsThenReadSourceAsyncReturnFileContent()
    {
        // Arrange
        var rootPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        var dataPath = Path.Combine(rootPath, "Data");
        Directory.CreateDirectory(dataPath);
        var sourcePath = Path.Combine(dataPath, "source.json");
        await File.WriteAllTextAsync(sourcePath, "{\"ok\":true}");

        var hostEnvironment = new TestHostEnvironment { ContentRootPath = rootPath };
        var loggerMock = new Mock<ILogger<FileJsonSourceRepository>>();
        var repository = new FileJsonSourceRepository(hostEnvironment, loggerMock.Object);

        try
        {
            // Act
            var result = await repository.ReadSourceAsync();

            // Assert
            Assert.Equal("{\"ok\":true}", result);
            VerifyLog(loggerMock, LogLevel.Information, "source.json");
        }
        finally
        {
            Directory.Delete(rootPath, true);
        }
    }

    [Fact]
    public async Task whenSourceFileDoesNotExistThenReadSourceAsyncReturnFileNotFoundException()
    {
        // Arrange
        var rootPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(rootPath);

        var hostEnvironment = new TestHostEnvironment { ContentRootPath = rootPath };
        var loggerMock = new Mock<ILogger<FileJsonSourceRepository>>();
        var repository = new FileJsonSourceRepository(hostEnvironment, loggerMock.Object);

        try
        {
            // Act
            var action = () => repository.ReadSourceAsync();

            // Assert
            var exception = await Assert.ThrowsAsync<FileNotFoundException>(action);
            Assert.Contains("source.json", exception.FileName);
        }
        finally
        {
            Directory.Delete(rootPath, true);
        }
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

    private sealed class TestHostEnvironment : IHostEnvironment
    {
        public string EnvironmentName { get; set; } = "Test";
        public string ApplicationName { get; set; } = "UpcAiPc1.Tests";
        public string ContentRootPath { get; set; } = string.Empty;
        public Microsoft.Extensions.FileProviders.IFileProvider ContentRootFileProvider { get; set; } =
            null!;
    }
}
