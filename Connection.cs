using Integration.Abstract.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Integration.Abstract
{
    //This is the master class for a given connection to a given site.
    public abstract class Connection 
    {
        #region IPaaS connection and details
        //A token that can be used to make any required iPaaS API calls
        public string IPaaSApiToken;

        //The system ID of your iPaaS system (not your external system). There are currently no features that require this value, but it is available for potential use in the future.
        public long IPaaSSystemId;

        //An object used by iPaaS to handling loading and unlaoding of external DLLs. This object should not be modified.
        public object Assembly;
        #endregion

        #region External Id and Call Wrapper
        //The system ID of your external system. This can be used for looking up or saving external IDs in iPaaS that go beyond the standard external ID framework.
        public long ExternalSystemId;

        //the ID for the external system type you are connecting to. There are currently no features that require this value, but developers may find it useful while looking up scopes or iPaaS-stored mapping metadata.
        public int ExternalSystemType;

        //The Id for the external system type version. 
        public string ExternalIntegrationVersionId;
        #endregion

        //Instances of all the External classes
        #region External Classes
        //Each of the following classes will be created as locally-typed instances
        public CallWrapper CallWrapper;
        public Settings Settings;
        public TranslationUtilities TranslationUtilities;
        public CustomFieldHandler CustomFieldHandler;

        //Since the ConversionFunction class should consist entirely of static methods, we do not instantiate it. But we do store the value of the ConversionFunctionType
        //   so that we can use it with FLEE later.
        public Type ConversionFunctionType;

        //We need to call a function specific to the External DLL, so at creation time we will store a Type reference that will allow us to execute it's methods
        //  more flexibly
        public Type TranslationUtilitiesType;

        //This is an instance of the logger helper class to allow external DLLs easy access to logging features.
        public Logger Logger;
        #endregion

        #region Delegates
        public delegate Task DataHandler(TransferRequest transferRequest);
        public delegate Task ClapbackTracker(long systemId, int mappingCollectionType, string id, int mappingDirection);

        //A procedure to accept data transfer requests from the external DLL. This is useful for data initialization calls and for pre-requisites and post-actions. 
        [JsonIgnore]
        public DataHandler DataHandlerFunctionAsync;

        //Any call to an external API that might trigger a response hook is required to first register the call with this function so that we know to ignore the 
        //  response hook when it arrives.
        [JsonIgnore]
        public ClapbackTracker ClapbackTrackerFunctionAsync;
        #endregion

        #region Method(s)
        /// <summary>
        /// Dispose of any declared objects. This method should be overridden in the external system to dispose of any other decalred objects.
        /// </summary>
        public void Dispose()
        {
            this.Assembly = null;
            this.CallWrapper = null;
            this.ClapbackTrackerFunctionAsync = null;
            this.ConversionFunctionType = null;
            this.CustomFieldHandler = null;
            this.DataHandlerFunctionAsync = null;
            this.IPaaSApiToken = null;
            this.Logger = null;
            this.Settings = null;
            this.TranslationUtilities = null;
            this.TranslationUtilitiesType = null;
        }
        #endregion
    }
}
