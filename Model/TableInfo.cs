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

        public string Type;

        public string Format;

        public int? MinLength;

        public int? MaxLength;

        public bool ReadOnly;

        public bool Required;

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
