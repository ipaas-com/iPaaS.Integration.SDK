using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Integration.Abstract.Model
{
    public class TranslationExternalId
    {
        //Note that this class differs from the ExternalIdRequest in that we store a MappingCollectionType instead of a table name
        [JsonProperty("mapping_collection_type", Order = 10, Required = Required.Always)]
        public int MappingCollectionType { get; set; }

        [JsonProperty("system_id", Order = 15, Required = Required.Always)]
        public long SystemId { get; set; }

        [JsonProperty("external_id", Order = 20, Required = Required.Always)]
        public string ExternalId { get; set; }

        [JsonProperty("internal_id", Order = 25)]
        public string InternalId { get; set; }
    }
}
