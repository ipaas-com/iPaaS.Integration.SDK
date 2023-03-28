using System;
using System.Collections.Generic;
using System.Text;

namespace Integration.Abstract.Helpers
{
    //The class is used to pass data back to iPaaS for bulk transfers. Anything specified here will be sent as hooks to the standard processing queue
    public class BulkTransferRequest
    {
        public string ExternalId;
        public int? MappingCollectionType;
        public Guid? TrackingGuid;

        public BulkTransferRequest(string externalId, int? mappingCollectionType = null, Guid? trackingGuid = null)
        {
            this.MappingCollectionType = mappingCollectionType;
            this.ExternalId = externalId;
            this.TrackingGuid = trackingGuid;
        }
    }
}
