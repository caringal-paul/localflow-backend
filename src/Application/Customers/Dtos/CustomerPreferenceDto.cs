namespace Application.Customers.Dtos;

public sealed record CustomerPreferenceDto(bool AllowEmail, bool AllowSms, bool AllowPush)
{
    public static readonly CustomerPreferenceDto Default = new(true, true, true);
}

public sealed record SaveCustomerPreferenceRequest(bool AllowEmail, bool AllowSms, bool AllowPush);