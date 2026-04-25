using System.Text.Json;

namespace UpcAiPc1.Application.Services;

public interface IJsonPayloadService
{
    Task<JsonElement> GetPayloadAsync(CancellationToken cancellationToken = default);
}
