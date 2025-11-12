using Integration.Abstract.Helpers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Integration.Abstract
{
    public abstract class Settings
    {
        //This field will be used in future functionality and is not currently supported
        public PersistentDataHandler PersistentData;

        #region Universal system data
        public long Id { get; set; }
        public int SystemTypeId { get; set; }
        public string IntegrationVersionId { get; set; }
        public string Name { get; set; }
        public string WebhookApiKey { get; set; }
        #endregion

        #region IPaaS API URLs
        //The variables below contain environment-specific values for iPaaS URLs. Developers may need to access iPaaS data as part of a pre-requisite check 
        //  (e.g. if you want to verify if a customer is present in iPaaS before transferring a transaction assigned to that customer), covnersion functions, etc.
        //  These variables will provide you with the appropriate URLs for each API.
        public string IPaaSApi_Customers { get { return GetSetting("Customers_URL"); } }
        public string IPaaSApi_GiftCards { get { return GetSetting("Giftcards_URL"); } }
        public string IPaaSApi_Integrators { get { return GetSetting("Integrators_URL"); } } 
        public string IPaaSApi_Products { get { return GetSetting("Products_URL"); } }
        public string IPaaSApi_SSO { get { return GetSetting("SSO_URL"); } }
        public string IPaaSApi_Subscriptions { get { return GetSetting("Subscriptions_URL"); } } //3.8.22: this was changed from Integrations to Subscriptions
        public string IPaaSApi_Transactions { get { return GetSetting("Transactions_URL"); } }
        public string IPaaSApi_Token { get { return GetSetting("IPaaSApi_Token"); } }
        public string IPaaSApi_WebhookUrl { get { return GetSetting("Hook_URL"); } }
        public string IPaaSApi_EmployeeUrl { get { return GetSetting("Employees_URL"); } }
        [Obsolete("Use IPaaSApi_MediaUrl instead")]
        public string IPaaSApi_MessageUrl { get { return GetSetting("Messages_URL"); } }
        public string IPaaSApi_MediaUrl { get { return GetSetting("Messages_URL"); } }
        #endregion

        //The TrackingGuid will be a unique identifier for the current transfer session. This ID will be used in the tech log and the activity tracker.
        public Guid TrackingGuid { get 
            { 
                var trackingGuid = GetSetting("TrackingGuid");
                if (string.IsNullOrEmpty(trackingGuid))
                    return Guid.Empty;
                else
                    return Guid.Parse(trackingGuid);
            } 
        }

        #region Systme settings and accessors
        public Dictionary<string, string> SettingsDictionary = new Dictionary<string, string>();

        /// <summary>
        /// Get setting by name
        /// </summary>
        /// <remarks>
        /// If the Required parameter is set to True, we throw an error if we cannot find the parameter. This is to aid in debugging so that we know the problem is a
        /// missing setting, rather than find out down the line that a required value is just being passed around as null
        /// </remarks>
        /// <param name="settingName"></param>
        /// <param name="required"></param>
        /// <returns></returns>
        public string GetSetting(string settingName, bool required = false)
        {
            if (SettingsDictionary == null || SettingsDictionary.Count == 0)
                throw new ExpectedException("Attempt to get setting without initializing the settings values, or a required setting was not specified: " + settingName);

            if (SettingsDictionary.ContainsKey(settingName))
            {
                var value = SettingsDictionary[settingName];
                if (value == null && required)
                    throw new ExpectedException("No value specified for required setting: " + settingName);
                return value;
            }

            if (required)
                throw new ExpectedException("Unable to find required Setting: " + settingName);

            return null;
        }
        #endregion
    }
}
