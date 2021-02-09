using System;

namespace DAL.Extensions
{
    public static class DateTimeExtensions
    {
        public static long ToJsTimestamp(this DateTime input)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var time = input.Subtract(new TimeSpan(epoch.Ticks));
            return time.Ticks / 10000;
        }
    }
}