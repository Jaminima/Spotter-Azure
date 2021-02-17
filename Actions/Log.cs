using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spotter_Azure.Actions
{
    public enum LogError
    {
        Success,
        Failure,
        Warning,
        Info
    }

    public class LogEntry
    {
        public LogError error;
        public string message;
        public DateTime when;

        public LogEntry(string message, LogError err = LogError.Warning)
        {
            error = err;
            this.message = message;
            when = DateTime.Now;
        }
    }

    public static class Log
    {
        public static List<LogEntry> _history = new List<LogEntry>();

        public static void Add(string message, LogError err = LogError.Warning)
        {
            _history.Add(new LogEntry(message, err));
        }

        public static string GetRowColor(LogEntry entry)
        {
            switch (entry.error)
            {
                default:
                    return "table-primary";
                case LogError.Success:
                    return "table-success";
                case LogError.Failure:
                    return "table-danger";
                case LogError.Warning:
                    return "table-warning";
                case LogError.Info:
                    return "table-info";
            }
        }
    }
}
