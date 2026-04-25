using System.Text.Json;
using Microsoft.Extensions.Logging;
using UpcAiPc1.Application.Exceptions;
using UpcAiPc1.Domain.Interfaces;

namespace UpcAiPc1.Application.Services;

public sealed class JsonPayloadService(
    IJsonSourceRepository jsonSourceRepository,
    ILogger<JsonPayloadService> logger) : IJsonPayloadService
{
    public async Task<JsonElement> GetPayloadAsync(CancellationToken cancellationToken = default)
    {
        var rawJson = await jsonSourceRepository.ReadSourceAsync(cancellationToken);

        try
        {
            using var document = JsonDocument.Parse(rawJson);
            var root = document.RootElement;
            var description = root.ValueKind == JsonValueKind.Array ? "arreglo" : "objeto unico";

            logger.LogInformation(
                "Lectura JSON completada correctamente. Tipo de contenido: {JsonDescription}.",
                description);

            return root.Clone();
        }
        catch (JsonException ex)
        {
            logger.LogError(ex, "Fallo la lectura del archivo de origen como JSON.");
            throw new InvalidSourceJsonException(
                "El archivo de origen no tiene un formato JSON valido.");
        }
    }
}
