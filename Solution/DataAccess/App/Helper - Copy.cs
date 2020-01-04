using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DataAccess.App
{
    public class DataFilterConditions
    {
        public virtual string FilterResult { get; }
        public class Filter : DataFilterConditions
        {
            public Filter(string Field)
            {
                ulol = Field;
            }
            string ulol = "";
            public override string FilterResult => ulol;
        }
       
    }
}
