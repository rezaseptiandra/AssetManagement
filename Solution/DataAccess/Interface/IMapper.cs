using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;

namespace DataAccess.Interface
{
    public interface IMapper: IAccessDB
    {
        
        IDbTransaction ObjTrans { get; set; }
        IDbConnection ObjConn { get; set; }
        string user { get; set; }
        string host { get; set; }
        void SetCmdTimeOut(int duration);
        void SetDialect(Dialect dialect);
        void Open();
        void Close();
        void Commit();
        void RollBack();
        void BeginTrans();
    }
    
}
