namespace Application.Common.Interfaces;

public interface IDocumentNumberGenerator
{
    Task<string> NextAsync(string sequenceKey, CancellationToken ct);
}