using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration.Abstract
{
    public class Constants
    {
        /// <summary>
        /// Data Types for presets, and data fields
        /// </summary>
        public enum SY_DataType
        {
            NONE,
            STRING,
            NUMBER,
            BOOL,
            GUID,
            ENCRYPTED,
            PASSWORD,
            ENUM,
            DATE,
            DATETIME
        }

        public enum TM_SyncType
        {
            UNKNOWN = 0,
            ADD = 1,
            UPDATE = 2,
            ADD_AND_UPDATE = 3,
            DELETE = 4,
            DELETE_TRIGGERED_UPDATE = 5 //Fired when we want to run an update mapping based on a delete occuring
        }

        public enum WH_ExternalSupport
        {
            NONE,
            FULL_SUPPORT,
            LOGICAL_SUPPORT_FOR_POLLING
        }
    }
}
