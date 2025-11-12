using System;
using System.Collections.Generic;
using System.Text;

namespace Integration.Abstract.Model
{
    public class Preset
    {
        #region Properties
        public string Name;

        /// <summary>
        /// One of the following: "string", "number", "boolean", "guid", "encrypted", "password", "enum", "date", "datetime"
        /// </summary>
        public string DataType;

        public bool IsRequired;

        public string DefaultValue;

        public int? SortOrder;

        public List<PresetValue> PresetValues;
        #endregion
    }

    public class PresetValue
    {
        #region Properties
        public string Value;

        public string Description;
        #endregion
    }
}
