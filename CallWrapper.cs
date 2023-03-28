using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Integration.Abstract
{
    /// <summary>
    /// The call wrapper class will handle all API endpoint calls to the external system. It should also include a connection setup function and a 
    /// method for processing responses and throwing errors if the API call failed.
    /// </summary>
    public abstract class CallWrapper
    {
        // We use this thread static variable to track which transfer request we are working on. This Guid is assigned once it is received by iPaaS and should remain the same
        // throughout the transfer process. We use it track all log entries (tech, tracking, and debug).
        [ThreadStatic]
        public Guid TrackingGuid;

        public abstract bool Connected { get; }
        public abstract string ConnectionMessage { get; } // If there was an error connecting, we need to save it here. This will allow us to produce the connection error if someone
                                                          // attempts to use the connection later. E.g. if we get data for system 3, we can produce a message like "unable to connect to 3: password invalid".

        /// <summary>
        /// Establish a connection to the external system, if necessary. This is not an abstract method, so inheriting classes do not necessarily need to override it. 
        /// </summary>
        /// <param name="connection">Your current connection. We recommend that when this method is overriden, you convert the connection variable to a locally-typed variable and store it in your CallWrapper class.</param>
        /// <param name="settings">A collection of your system settings</param>
        public async Task EstablishConnection(Connection connection, Settings settings)
        {
            ;
        }
    }
}
