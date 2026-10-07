namespace Infrastructure.Persistence.Configurations;

internal static class SqlFilters
{
    // Unique indexes ignore soft-deleted rows so values can be reused
    public const string NotDeleted = "is_deleted = false";
}