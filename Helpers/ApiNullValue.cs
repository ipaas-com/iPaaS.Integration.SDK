using System;
using System.Collections.Generic;
using System.Text;

namespace Integration.Abstract.Helpers
{
    /// <summary>
    /// Provide a property decoration that allows users to specify a default value that will only be populated when a mapping attempts to set a value for the 
    /// field. 
    /// </summary>
    /// <example>
    /// Usage: add this attribtue to the property you want to specify a default value for, with the default value in parenthesis.
    /// This will set the weight field to 0 if a null value is set by a mapping in iPaaS:
    ///          [ApiNullValue(0)]
    ///          public decimal? Weight;
    /// </example>
    /// <remarks>
    /// This feature is slightly different from specifying a JSON default, as it provides the additional feature of only activating the default value if a mapping attempts to assign it
    /// a null value. This is useful in cases where nulls are treated as a non-update to the field, but a set value is used to indicate that the existing value should be erased.
    /// For example, in the BigCommerce API, the category model has a ParentId field. If I want to update the category to have no parent, I need to send a 0 for the ParentId field. 
    /// If I want to update other fields and leave the ParentId alone, I set the ParentId to null. This ApiNullValue tag allows us to get that functionality simply by using a standard
    /// field mapping in iPaaS. If a mapping exists for ParentId but the mapping finds no parent, the field is set to 0. If there is no mapping, the field is set to null and is ignored
    /// by the BC API.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class ApiNullValue : System.Attribute
    {
        private object _nullValue;
        public object NullValue { get { return _nullValue; } }
        public ApiNullValue(object NullValue)
        {
            this._nullValue = NullValue;
        }
    }
}
