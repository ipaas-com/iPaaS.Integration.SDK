using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Integration.Abstract.Model
{
    /// <summary>
    /// If webhooks will be arriving to iPaaS in a non-standard format, this will contain the settings for how to interpret the hook data
    /// </summary>
    public class WebhookConfiguration
    {
        /// <summary>
        /// If the hook comes in a format that isn’t ready to read, this expression will need to be created and saved so that it can be sent to the Expression Evaluator. 
        /// Not required.
        /// </summary>
        public string BodyPreprocessingExpression { get; set; }

        /// <summary>
        /// If a system sends in multiple hooks at a time this will need to be set to true. The default for this is false. 
        /// </summary>
        public bool HasMultiple { get; set; }

        /// <summary>
        /// The JSON path to select the collection containing multiple hooks. 
        /// </summary>
        public string MultipleSelector_JsonPath { get; set; }

        /// <summary>
        /// A list of field definitions. This list must include exactly three entries, one for each of the following ValueNames: AuthToken, Scope, ExternalId
        /// </summary>
        public List<WebhookConfigurationField> FieldDefinitions { get; set; }

        public WebhookConfiguration()
        {
            FieldDefinitions = new List<WebhookConfigurationField>();
        }
    }
}
