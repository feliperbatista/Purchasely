namespace Purchasely.Infrastructure.Extensions;

public static class DateTimeExtensions
{
    public static DateTime ToUtc(this DateTime dt) =>
        DateTime.SpecifyKind(dt, DateTimeKind.Utc);
}