using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;

namespace DataAccess
{
    public interface IViewModel
    {
        string SPname { get; }
    }
    public interface IObjectConnection
    {
        IDbConnection ObjConn { get; set; }
        IDbTransaction ObjTrans { get; set; }
        Dialect Dialect { get; set; }
        bool WithTrans { get; set; }
    }
}
