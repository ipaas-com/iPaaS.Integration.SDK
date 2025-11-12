using System;
using System.Collections.Generic;
using System.Text;

namespace Integration.Abstract.Helpers
{
    //This class represents a request to transfer data from one source to another. Typically the translation engine will 
    public class TransferRequest
    {
        public enum RequestType_Enum
        {
            Standard,
            Delete,
            Child,
            Initialization,
            Bulk,
            Polling
        }
        public RequestType_Enum RequestType;

        public long ExternalSystemId;
        public long IPaaSSystemId;
        public int MappingCollectionType;
        public int MappingDirection; //TO_IPAAS = 1, FROM_IPAAS = 2
        public object Payload;
        public bool HasErrors = false;
        public Exception Exception;

        // HS_Queue Items
        public long SourceSystemId;
        public string Destination;
        public string Scope;
        public string ExternalId;
        public Guid TrackingGuid;

        public TransferRequest()
        {
            
        }

        public TransferRequest(int mappingCollectionType, int mappingDirection, string scope, bool childRequest, Integration.Abstract.Connection connection, object payload = null, string externalId = null, Guid? trackingGuid = null)
        {
            this.MappingCollectionType = mappingCollectionType;
            this.MappingDirection = mappingDirection;
            this.Payload = payload;
            this.IPaaSSystemId = connection.IPaaSSystemId;
            this.ExternalSystemId = connection.ExternalSystemId;
            this.Scope = scope;
            this.ExternalId = externalId;

            if (childRequest)
                this.RequestType = TransferRequest.RequestType_Enum.Child;

            if (this.MappingDirection == 1) //TO_IPAAS
            {
                this.SourceSystemId = this.ExternalSystemId;
                this.Destination = "iPaaS";
            }
            else
            {
                this.SourceSystemId = -1;
                this.Destination = Convert.ToString(ExternalSystemId);
            }

            if (trackingGuid.HasValue)
                this.TrackingGuid = trackingGuid.Value;
        }
    }
}
