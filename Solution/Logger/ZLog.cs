using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess;
using DataAccess.Interface;
using DataAccess.ModelsViewModels;

namespace Logger
{
   
    public class ZLog : Ilog
    {
        private readonly IMapper imap_;
        private ExecResult exec;
        public ZLog(IMapper imap)
        {
            this.exec = new ExecResult();
            this.imap_ = imap;
            _obj = new ZLogger();
        }
        private ZLogger _obj { get; set; }
        public ZLogger GetLogInfo { get => _obj; }
        public void SetLogInfo(ZLogger obj)
        {
            _obj = obj;

        }
        public void SetLogInfo(string username, string path)
        {
            _obj.Username = username == "" ? this._obj.Username : username;
            _obj.Path = path == "" ? this._obj.Path : path;
        }
        public void INFO(string message)
        {
            insertLogDB("INFO", message);
        }
        public void ERROR(string message)
        {
            //insertLogDB("ERROR", message);
        }
        public void DEBUG(string message)
        {
            insertLogDB("DEBUG", message);
        }
        public void WARNING(string message)
        {
            insertLogDB("WARNING", message);
        }
        private void insertLogDB(string level, string message)
        {
            //_obj.Message = message;
            //ZLoggerRPO zlogRPO = new ZLoggerRPO(imap_);
            //if (string.IsNullOrEmpty(_obj.Username))
            //    _obj.Username = "unknown-user";
            //_obj.Timestamp = DateTime.Now;
            //_obj.Level = level;
            //zlogRPO.Insert(_obj, ref exec);
        }
    }
    /*
     * 
     */

}
