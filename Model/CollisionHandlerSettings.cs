using System;
using System.Collections.Generic;
using System.Text;

namespace Integration.Abstract.Model
{
    public class CollisionHandlerSettings
    {
        //The method used for handling collisions:  ERROR, REMAP_AND_LINK, UPDATE_AND_LINK, UPDATE_AND_NO_LINK
        public int? CollisionHandlerMethod { get; set; }

        //A place to store misc. options for collision handling
        public string CollisionHandlerOptions { get; set; }

        //Not currently used, but might in the future
        public bool Precheck { get; set; }
        //Not currently used, but might in the future
        public string PrecheckField { get; set; }
    }
}
