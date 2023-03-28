using System;
using System.Collections.Generic;
using System.Text;

namespace Integration.Abstract
{
    public abstract class CustomFieldHandler
    {
        //Since custom fields can be implemented differently in each external system, we require that each system define a method for returning
        //   and setting a custom field value.
        public abstract object GetValueCustomField(object inputObject, string propertyName);

        //Since custom fields can be implemented differently in each external system, we require that each system define a method for setting a custom field value. 
        //  This function should return True if the setting was correctly made, or False if it was not.
        public abstract Boolean SetValueCustomField(object inputObject, string propertyName, object propertyVal);

        //Since custom fields can be implemented differently, this call will direct the external DLL to normalize the custom field list into a key/value pair list that
        //  can be used elsewhere (e.g. to make the custom fields available as standard fields for formulas)
        public abstract List<KeyValuePair<string, object>> GetCustomFieldKVPs(object inputObject);
    }
}
