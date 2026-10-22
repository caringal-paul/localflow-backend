namespace Application.Customers.Dtos;

public sealed record ListCustomersQuery(string? Search, int Page = 1, int PageSize = 20);