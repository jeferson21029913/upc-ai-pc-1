namespace UpcAiPc1.Domain.Interfaces;

public interface IJsonSourceRepository
{
    Task<string> ReadSourceAsync(CancellationToken cancellationToken = default);
}
