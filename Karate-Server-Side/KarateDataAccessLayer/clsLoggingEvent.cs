using System;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;


namespace clsKarateDataAccesse
{
    public class clsLoggingEvent
    {




        public static void LoogingEvent(string ErrorMessage)
        {
            string sourceName = "KarateApp";

            if (!EventLog.SourceExists(sourceName))
            {
                EventLog.CreateEventSource(sourceName, "Application");
            }
            EventLog.WriteEntry(sourceName, ErrorMessage, EventLogEntryType.Error);

        }
    }
}