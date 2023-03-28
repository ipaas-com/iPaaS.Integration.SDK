using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Integration.Abstract.Helpers
{
    //This exception is intended to be used for any "expected" errors. E.g. If the Website Url is not specified for an ecommerce site, we throw an error.
    //  This is different from unexpected errors, such as null reference not set to an instance of an object. The ExpectedException signals to the 
    //  logger that we only need to log the message, and not the full object (which would include stack trace, innerexception, etc.). This will allow for 
    //  much cleaner error messages. 
    public class ExpectedException : Exception
    {
        [JsonIgnore]
        public override string StackTrace => base.StackTrace;

        [JsonIgnore]
        public override System.Collections.IDictionary Data => base.Data;

        [JsonIgnore]
        public override string Source => base.Source;

        //If some data was saved in the external system but we were not able to complete the transfer, that should be passed here. We will save the external
        //  ids of this data so that the next transfer doesn't attempt to recreate it.
        public object SavedExternalData { get; set; }

        public ExpectedException(string message) : base(message)
        {
            ;
        }

    }
}
