using System;
using System.Collections.Generic;

namespace Spotter_Azure.Actions
{
    public enum LogError
    {
        Success,
        Failure,
        Warning,
        Info
    }

    public static class Log
    {
        #region Fields

        public static List<LogEntry> _history = new List<LogEntry>();

        #endregion Fields

        #region Methods

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

        #endregion Methods
    }

    public class LogEntry
    {
        #region Fields

        public LogError error;
        public string message;
        public DateTime when;

        #endregion Fields

        #region Constructors

        public LogEntry(string message, LogError err = LogError.Warning)
        {
            error = err;
            this.message = message;
            when = DateTime.Now;
        }

        #endregion Constructors
    }
}
