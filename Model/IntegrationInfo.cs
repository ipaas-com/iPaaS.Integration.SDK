using System;
using System.Collections.Generic;
using System.Text;

namespace Integration.Abstract.Model
{
    public class IntegrationInfo
    {
        #region Properties
        /// <summary>
        /// Name. 
        /// </summary>
        public string Name;

        //This field has been removed. SystemTypeId may vary by environment. This needs to be specified in the call, not in the DLL
        //public int SystemTypeId;

        /// <summary>
        /// Name of the complied Dll
        /// </summary>
        public string IntegrationFilename;

        /// <summary>
        /// The namespace that the integration dll uses
        /// </summary>
        public string IntegrationNamespace;

        /// <summary>
        /// The API version of the external system.
        /// </summary>
        /// <remarks>
        /// This field represents the version of the API being used. E.g. if we are connecting to Magento 2, the target ApiVersion might be 2.3.1.
        /// </remarks>
        public string ApiVersion;

        /// <summary>
        /// The major version of the external integration dll.
        /// </summary>
        /// <remarks>
        /// This field represents the version of the integration dll, NOT the version of the external API. E.g. we may be connecting to Magento 2, but if
        /// this is the first iteration of an iPaaS connection that API, the VersionMajor would be 1.
        /// </remarks>
        public int VersionMajor;

        /// <summary>
        /// The minor version of the external integration dll.
        /// </summary>
        public int VersionMinor;

        /// <summary>
        /// The patch version of the external integration dll.
        /// </summary>
        public int VersionPatch;

        /// <summary>
        /// List of custom field names. Note values cannot be specified here, those must be set using the API or UI.
        /// </summary>
        public List<string> VersionCustomFieldNames;

        /// <summary>
        /// The OAuth Authorization Type Id used for this integration. If your integration does not use OAuth, set this to null or 0
        /// 0=NONE, 1= OAUTH_IPAAS_DRIVEN, 2= OAUTH_EXTERNALLY_DRIVEN, 3= OAUTH_EXTERNALLY_DRIVEN_HYBRID, 4= OAUTH_EXTERNALLY_DRIVEN_LOGIN_FIRST    
        /// </summary>
        public int? OAuthAuthorizationTypeId;

        public string OAuthUrlTemplate;

        public string OAuthIdentifierField;

        public string OAuthSuccessCallbackField;

        //This field has been removed and must be set in the UI
        /// <summary>
        /// The location of where the image is stored via a url
        /// </summary>
        //public string ImageUrl;
        #endregion
    }
}
