using System;

namespace SharedCode.Utils
{
    public static class DateTimeUtils
    {
        private const string MyTimeFormat = "HH:mm:ss";
        private const string MyTimeSpanFormat = "c";

        public static string GetTimeString(DateTime dateTime)
        {
            return dateTime.ToString(MyTimeFormat);
        }

        public static string GetTimeDifferenceString(DateTime start, DateTime end)
        {
            TimeSpan diff = end - start;
            return $"{(int)diff.TotalHours:00}:{diff.Minutes:00}:{diff.Seconds:00}";
        }
    }
}
