namespace Domain.Enums;

[Flags]
public enum NotificationChannels
{
    None  = 0,
    Email = 1,
    Sms   = 2,
    Push  = 4
}