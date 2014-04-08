using System;
using System.Text;
using System.IO;

using Discuz.Common;
using Discuz.Config;

namespace Discuz.Forum.ScheduledEvents
{
    public sealed class EventLogs
    {
        public static string LogFileName = string.Empty;

        public static void WriteFailedLog(string logContent)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(DateTime.Now);
            builder.Append("\t");
            builder.Append(Environment.MachineName);
            builder.Append("\t");
            builder.Append(logContent);
            builder.Append("\r\n");
            using (FileStream fs = new FileStream(LogFileName, FileMode.Append, FileAccess.Write, FileShare.Write))
            {
                Byte[] info = System.Text.Encoding.UTF8.GetBytes(builder.ToString());
                fs.Write(info, 0, info.Length);
                fs.Close();
            }
        }
    }
}
