using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using DataAccess.ModelsViewModels;

namespace DataAccess.Interface
{
    public interface Ilog
    {
        void SetLogInfo(ZLogger obj);
        void SetLogInfo(string message, string path);
        ZLogger GetLogInfo { get; }
        void INFO(string message);
        void ERROR(string message);
        void DEBUG(string message);
        void WARNING(string message);
    }

}
