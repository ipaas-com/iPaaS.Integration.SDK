using System;
using System.Collections.Generic;
using System.Text;

namespace Integration.Abstract
{
    /// <summary>
    /// This class is used to load meta data from new exteranl systems. The LoadMetaData method should be overriden to populate all of the specified properties.
    /// This data will be consumed by iPaaS and used to populate the data required to set up a new integration.
    /// </summary>
    public abstract class MetaData
    {
        #region Properties
        /// <summary>
        /// Provides the info for this integration dll
        /// </summary>
        public Integration.Abstract.Model.IntegrationInfo Info;

        /// <summary>
        /// List of Table & Field structure for all endpoints. This is used for mapping purposes
        /// </summary>
        public List<Integration.Abstract.Model.TableInfo> Tables;

        /// <summary>
        /// List of scopes that this integration sends out
        /// </summary>
        public List<Integration.Abstract.Model.Scope> Scopes;

        /// <summary>
        /// List of preset values that can be set when creating an integration in iPaaS
        /// </summary>
        public List<Integration.Abstract.Model.Preset> Presets;

        // <summary>
        // A list of what mapping collection types are supported, and what their support is like
        // </summary>
        public List<Integration.Abstract.Model.Features> FeatureSupport;

        /// <summary>
        /// If webhooks will be arriving to iPaaS in a non-standard format, this will contain the settings for how to interpret the hook data
        /// </summary>
        public Integration.Abstract.Model.WebhookConfiguration WebhookConfigurationSettings;
        #endregion

        #region Method(s)
        public abstract void LoadMetaData();
        #endregion
    }
}
