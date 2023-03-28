using System;
using System.Collections.Generic;
using System.Text;

namespace Integration.Abstract.Model
{
    public class Scope
    {
        #region Properties
        /// <summary>
        /// Required field
        /// </summary>
        public string Name;

        /// <summary>
        /// 
        /// </summary>
        public string Description;

        /// <summary>
        /// Use the following endpoint found in the Integrations API to get a list of available mapping collection types 
        /// Endpoint: /v2/Lookup/MappingCollectionType
        /// </summary>
        public long MappingCollectionTypeId;

        /// <summary>
        /// Indicates the action type of this scope: 1=CREATED, 2=UPDATED, 3=DELETED, 4=INITIALIZE, 5=ALL, 6=BULK, 7=OTHER
        /// </summary>
        public int ScopeActionId;
        #endregion
    }
}
