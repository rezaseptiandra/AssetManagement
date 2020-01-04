using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess
{
    public struct ObjExecute
    {
        public ObjExecute(string result = "", int affectedrow = 0)
        {
            this.Result = result;
            this.AffectedRow = affectedrow;
        }
        public string Result { get; }
        public int AffectedRow { get; }
    }
}