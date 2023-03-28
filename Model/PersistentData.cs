using System;
using System.Collections.Generic;
using System.Text;

namespace Integration.Abstract.Model
{
    public class PersistentData
    {
        public string Name { get; set; }

        public object Value { get; set; }

        public DateTimeOffset? ExpirationDateTime { get; set; }
    }
}
