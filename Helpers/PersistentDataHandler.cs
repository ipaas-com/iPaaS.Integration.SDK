using Integration.Abstract.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Integration.Abstract.Helpers
{
    /// <summary>
    /// This class is for persisting data, such as a login token, between calls. Currently this feature is not fully supported.
    /// </summary>
    public class PersistentDataHandler
    {
        public List<PersistentData> Values = new List<PersistentData>();

        public void SaveValue(string name, object value, DateTimeOffset? expirationDateTime = null )
        {
            SaveValue(new PersistentData() { Name = name, Value = value, ExpirationDateTime = expirationDateTime });
        }

        public void SaveValue(PersistentData value)
        {
            var existingValue = GetValue(value.Name);
            //If a value with this name already existed, remove it and replace it.
            if (existingValue != null)
                Values.Remove(value);
            Values.Add(value);
        }


        public PersistentData GetValue(string name)
        {
            var match = Values.Find(x => x.Name == name);
            return match;
        }
    }
}
