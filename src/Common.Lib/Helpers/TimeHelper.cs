namespace Common.Lib.Helpers;

public static class TimeHelper
{
    /// <summary>
    /// 时间戳
    /// </summary>
    public static long Timestamp => DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

    public static readonly TimeZoneInfo CstTimeZone = GetChinaTimeZoneId();

    private static TimeZoneInfo GetChinaTimeZoneId()
    {
        string osName = Environment.OSVersion.Platform.ToString();

        if (osName.Contains("Win"))
        {
            return TimeZoneInfo.FindSystemTimeZoneById("China Standard Time");
        }
        else
        {
            return TimeZoneInfo.FindSystemTimeZoneById("Asia/Shanghai");
        }
    }

    public static DateTime NowCst => TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, CstTimeZone);

    public static DateTime TimestampToDateTime(long timestamp)
    {
        DateTimeOffset utcDateTimeMs = DateTimeOffset.FromUnixTimeMilliseconds(timestamp);
        return TimeZoneInfo.ConvertTimeFromUtc(utcDateTimeMs.UtcDateTime, CstTimeZone);
    }
}