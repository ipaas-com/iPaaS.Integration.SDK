using Integration.Abstract.Model;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Integration.Abstract.Helpers
{
    public class QuotaException : Exception
    {
        public ResponseObject Response;

        //A base initializer that creates a standard exception with a reasonable default message.
        public QuotaException() : base("API call over quota limit")
        {
            ;
        }

        public QuotaException(string message) : base(message)
        {
            ;
        }

        public QuotaException(string message, Exception innerException) : base(message, innerException)
        {
            ;
        }

        public QuotaException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            ;
        }

        /// <summary>
        /// A helper function that browses all ResposeQuotas and returns the latest reset datetime.
        /// </summary>
        /// <returns></returns>
        public DateTime? GetResetDT()
        {
            if (Response == null)
                return null;
            if (Response.ResponseQuotas == null)
                return null;

            DateTime? retVal = null;
            foreach(var rq in Response.ResponseQuotas)
            {
                if(rq.QuotaResetDateTime.HasValue)
                {
                    if (!retVal.HasValue)
                        retVal = rq.QuotaResetDateTime;
                    //We want to return the latest resetDT
                    else if(retVal.Value < rq.QuotaResetDateTime.Value)
                        retVal = rq.QuotaResetDateTime;
                }
            }

            return retVal;
        }

        /// <summary>
        /// A helper function to create a basic ResponseQuota and assign it to the current QuotaException
        /// </summary>
        /// <param name="quotaId"></param>
        /// <param name="totalCallsInQuota"></param>
        /// <param name="remainingCallsInQuota"></param>
        /// <param name="quotaResetDateTime"></param>
        /// <param name="availableCallsAtReset"></param>
        public void AddResponseQuota(string quotaId, long? totalCallsInQuota = null, long? remainingCallsInQuota = null, DateTime? quotaResetDateTime = null, long? availableCallsAtReset = null)
        {
            if (Response == null)
                Response = new ResponseObject();

            if (Response.ResponseQuotas == null)
                Response.ResponseQuotas = new List<ResponseQuota>();

            var responseQuota = Response.ResponseQuotas.Find(x => x.QuotaId == quotaId);
            if (responseQuota == null)
            {
                responseQuota = new ResponseQuota() { QuotaId = quotaId };
                responseQuota.TotalCallsInQuota = totalCallsInQuota;
                responseQuota.RemainingCallsInQuota = remainingCallsInQuota;
                responseQuota.QuotaResetDateTime = quotaResetDateTime;
                responseQuota.AvailableCallsAtReset = availableCallsAtReset;
                Response.ResponseQuotas.Add(responseQuota);
            }
            else
            {
                if (totalCallsInQuota.HasValue)
                    responseQuota.TotalCallsInQuota = totalCallsInQuota;
                if (remainingCallsInQuota.HasValue)
                    responseQuota.RemainingCallsInQuota = remainingCallsInQuota;
                if (quotaResetDateTime.HasValue)
                    responseQuota.QuotaResetDateTime = quotaResetDateTime;
                if (availableCallsAtReset.HasValue)
                    responseQuota.AvailableCallsAtReset = availableCallsAtReset;
            }
        }

    }
}
