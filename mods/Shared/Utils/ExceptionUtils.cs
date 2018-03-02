using System;

namespace SharedCode.Utils
{
    public class ExceptionUtils
    {
        public static string GetExceptionErrorString(Exception e)
        {
            var result = "";

            while (e != null)
            {
                result += "Exception Message: " + e.Message + Environment.NewLine;
                result += e.StackTrace + Environment.NewLine;
                e = e.InnerException;
            }

            result += Environment.NewLine;
            return result;
        }
    }
}
