namespace Application.Common;

public static class DocumentSequenceDefaults
{
    public static readonly (string Key, string Prefix)[] All =
    [
        (DocumentSequenceKeys.Member, "MBR-"),
        (DocumentSequenceKeys.Customer, "CUS-"),
        (DocumentSequenceKeys.Order, "ORD-")
    ];
}