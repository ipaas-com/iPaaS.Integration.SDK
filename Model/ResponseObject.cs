using System;
using System.Collections.Generic;
using System.Text;

namespace Integration.Abstract.Model
{
    public class ResponseObject
    {
        public object Payload;

        public int TotalAPICallsMade;

        public List<ResponseQuota> ResponseQuotas;

        public object CollisionData;

        public Type ResponseType;
    }
}
