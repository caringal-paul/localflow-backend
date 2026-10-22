using Application.Common.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class DocumentNumberGenerator(ApplicationDbContext db, ICurrentUserService currentUser) : IDocumentNumberGenerator
{
    public async Task<string> NextAsync(string sequenceKey, CancellationToken ct)
    {
        var tenantId = currentUser.TenantId
            ?? throw new InvalidOperationException("No tenant in context.");

        // Atomic: one statement increments and returns, so two requests never get the same number.
        var result = await db.Database.SqlQuery<string>($"""
            UPDATE document_sequences
            SET next_value = next_value + 1
            WHERE tenant_id = {tenantId} AND sequence_key = {sequenceKey} AND is_deleted = false
            RETURNING coalesce(prefix, '') || lpad((next_value - 1)::text, padding, '0') AS "Value"
            """).ToListAsync(ct);

        return result.SingleOrDefault()
            ?? throw new InvalidOperationException($"Missing document sequence '{sequenceKey}' for tenant.");
    }
}