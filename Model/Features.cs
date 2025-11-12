using System;
using System.Collections.Generic;
using System.Text;

namespace Integration.Abstract.Model
{
    public class Features
    {
        public enum SupportLevel
        {
            None,
            Partial,
            Full
        }

        public int MappingCollectionType { get; set; }
        public int MappingDirectionId { get; set; }
        public SupportLevel Support { get; set; }
        public string AdditionalInformation { get; set; }
        public bool AllowInitialization { get; set; }

        public bool? CollisionHandlingSupported { get; set; }
        public bool? CustomfieldSupported { get; set; }
        public bool? IndependentTransferSupported { get; set; }
        public bool? PollingSupported { get; set; }
        public bool? RecordMatchingSupported { get; set; }
        public int? ExternalWebhookSupportId { get; set; }

        public List<FeatureSupportEndpoint> SupportedEndpoints { get; set; }
        public List<ExternalIdFormat> ExternalIdFormats { get; set; }
        public List<FeatureSupportDataType> ExternalDataTypes { get; set; }

        public List<int> SupportedMethods { get; set; }

        public Features()
        {
            AllowInitialization = false;
            SupportedEndpoints = new List<FeatureSupportEndpoint>();
            ExternalIdFormats = new List<ExternalIdFormat>();
            ExternalDataTypes = new List<FeatureSupportDataType>();
            SupportedMethods = new List<int>();
        }
    }

    public class FeatureSupportEndpoint
    {
        public string Value { get; set; }
        public string Note { get; set; }
    }

    public class ExternalIdFormat
    {
        public string RecordExternalIdFormat { get; set; }
        public string WebhookExternalIdFormat { get; set; }
        public string Note { get; set; }
    }

    public class FeatureSupportDataType
    {
        public string Value { get; set; }
        public string Note { get; set; }
    }
}
