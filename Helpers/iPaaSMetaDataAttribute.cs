using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Integration.Abstract.Constants;

namespace Integration.Abstract.Helpers
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class iPaaSMetaDataAttribute : Attribute
    {
        /// <summary>
        /// A human-readable description of the field.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The data type associated with this field.
        /// </summary>
        private SY_DataType _type = SY_DataType.NONE;
        //We cannot have nullable attribute properties that are settable in the [iPaaSMetaData] tag, so we need to track whether the user set this value or not
        public bool HasType { get; private set; }
        public SY_DataType Type
        {
            get => _type;
            set { _type = value; HasType = true; }
        }

        /// <summary>
        /// Indicates whether the field is required.
        /// </summary>
        private bool _required; // default false
        //We cannot have nullable attribute properties that are settable in the [iPaaSMetaData] tag, so we need to track whether the user set this value or not
        public bool HasRequired { get; private set; }
        public bool Required
        {
            get => _required;
            set { _required = value; HasRequired = true; }
        }

        public iPaaSMetaDataAttribute() { }
    }
}
