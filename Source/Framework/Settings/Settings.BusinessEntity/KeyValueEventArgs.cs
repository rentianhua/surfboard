using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HiiP.Framework.Settings.BusinessEntity
{
    public class KeyValueEventArgs:EventArgs
    {
        public string Key { get; set; }
        public object Value { get; set; }
        public string TargetViewID { get; set; }

        public KeyValueEventArgs():this(string.Empty,null,null)
        { 
        }
        public KeyValueEventArgs(string key, object value,string targetViewID)
        {
            Key = key;
            Value = value;
            TargetViewID = targetViewID;
        }
    }
}
