using System;
using System.Collections.Generic;
using System.Text;

namespace Integration.Abstract.Model
{
    public class ResponseQuota
    {
        public string QuotaId;

        public long? TotalCallsInQuota;

        public long? RemainingCallsInQuota;

        public DateTime? QuotaResetDateTime;

        public long? AvailableCallsAtReset;
    }
}
