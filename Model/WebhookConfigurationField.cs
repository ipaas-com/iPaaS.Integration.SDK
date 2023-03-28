using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Integration.Abstract.Model
{
    public class WebhookConfigurationField
    {
        #region Constants
        public enum ValueName_Enum
        {
            AuthToken,
            Scope,
            ExternalId
        }

        public enum RetrievalType_Enum
        {
            Header, 
            BodyJsonPath, 
            UrlSegment, 
            Static
        }

        public enum ProcessingType_Enum
        {
            AsIs,
            Expression
        }
        #endregion

        #region Properties

        /// <summary>
        /// The name of the field =
        /// </summary>
        public ValueName_Enum ValueName { get; set; }

        /// <summary>
        /// Where the field can be found in the hook
        /// </summary>
        public RetrievalType_Enum RetrievalType { get; set; }

        /// <summary>
        /// Where the value is located in the hook
        /// For RetrievalType_Enum.Header, this is the header name, for BodyJsonPath it's the json path, for Static it's the value
        /// </summary>
        public string RetrievalValue { get; set; }

        /// <summary>
        /// Whether the RetrievalValue can be used as-is, or if it needs to be processed with an expression first
        /// </summary>
        public ProcessingType_Enum ProcessingType { get; set; }

        /// <summary>
        /// The expression for the Retrieval Value to be sent to the Expression Evaluator, if ProcessingType = ProcessingType_Enum.Expression
        /// </summary>
        public string ProcessingValue { get; set; }
        #endregion
    }
}
