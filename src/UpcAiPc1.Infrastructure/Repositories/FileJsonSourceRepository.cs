using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using UpcAiPc1.Domain.Interfaces;

namespace UpcAiPc1.Infrastructure.Repositories;

public sealed class FileJsonSourceRepository(
    IHostEnvironment environment,
    ILogger<FileJsonSourceRepository> logger) : IJsonSourceRepository
{
    private const string SourceFile = "Data/source.json";

    public async Task<string> ReadSourceAsync(CancellationToken cancellationToken = default)
    {
        var sourcePath = Path.Combine(environment.ContentRootPath, SourceFile);

        if (!File.Exists(sourcePath))
        {
            throw new FileNotFoundException("Source JSON file was not found.", sourcePath);
        }

        logger.LogInformation("Archivo de origen encontrado: {FileName}", SourceFile);
        return await File.ReadAllTextAsync(sourcePath, cancellationToken);
    }
}
