using System;
using System.Collections.Generic;
using System.Text;

namespace Integration.Abstract.Model
{
    //Chekc in
    public class Authorization
    {
        public string CallbackId;
        public Dictionary<string, string> CallbackData { get; set; }
    }
}
