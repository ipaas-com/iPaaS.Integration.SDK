using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Integration.Abstract.Helpers
{
    //This class contains function references that provide access to standard logging calls. There are two types of logging provided: a technical log that 
    //will only be available to developers, and an activity tracker log that will be visible to end users of iPaaS.
    public class Logger
    {
        //This is only used for debugging purposes in the translator.
        public static bool PrintDebugMessages;

        #region delegate functions
        public delegate void Technical(string severity, string location, string details);
        public delegate void ActivityTracker(string activity, string details, string status, int detailMappingCollectionType);

        [JsonIgnore]
        public Technical Logger_Technical;
        [JsonIgnore]
        public ActivityTracker Logger_ActivityTracker;
        #endregion

        #region Procedures
        public void Log_Technical(string severity, string location, string details)
        {
            if(Logger_Technical != null)
                Logger_Technical(severity, location, details);
        }

        //This is a helper for the Log_Technical function to allow us to easily standardize exception logging
        public void Log_TechnicalException(string location, Exception ex)
        {
            string exceptionJSON;
            if (ex is ExpectedException) //If this is an exception whose only data is its message, we don't want to serialize the whole thing
                exceptionJSON = ex.Message;
            else
                exceptionJSON = JsonConvert.SerializeObject(ex, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            Log_Technical("E", location, exceptionJSON);
        }

        public void Log_ActivityTracker(string activity, string details, string status, int detailMappingCollectionType)
        {
            if (Logger_ActivityTracker == null)
                return;

            Logger_ActivityTracker(activity, details, status, detailMappingCollectionType);
        }
        #endregion
    }
}
