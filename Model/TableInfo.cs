using System;
using System.Collections.Generic;
using System.Text;

namespace Integration.Abstract.Model
{
    public class TableInfo
    {
        #region Properties
        public string Name;

        public string FriendlyName;

        public string Description;

        public long MappingCollectionTypeId;

        public List<FieldInfo> Fields;
        #endregion
    }

    public class FieldInfo
    {
        #region Properties
        public string Name;

        public string Description;

        /// <summary>
        /// One of the following: "string", "number", "boolean", "guid", "encrypted", "password", "enum", "date", "datetime"
        /// </summary>
        public string Type;

        [Obsolete("No longer supported in the API")]
        public string Format;

        [Obsolete("No longer supported in the API")]
        public int? MinLength;

        [Obsolete("No longer supported in the API")]
        public int? MaxLength;

        [Obsolete("No longer supported in the API")]
        public bool ReadOnly;

        public bool Required;

        [Obsolete("No longer supported in the API")]
        public string Example;

        public List<FieldValues> FieldValues;
        #endregion
    }

    public class FieldValues
    {
        #region Properties
        public string Name;

        public string Value;

        public string Description;
        #endregion
    }
}
