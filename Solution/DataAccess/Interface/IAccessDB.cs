using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;

namespace DataAccess.Interface
{
    public interface IAccessDB
    {
        /// <summary>
        /// Return last inserted ID as int
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        int Insert<T>(T model, out object Keys, Conditions conditions, bool GetLastKey = false) where T : class;
        T ReadOne<T>(List<object> keys, Conditions conditions);
        List<T> ReadList<T>(Conditions conditions);
        List<T> ReadListPaged<T>(Conditions conditions,int pageNumber, int rowsPerPage, out int totalrow);
        int Update<T>(T model, bool isDirect, Conditions conditions) where T : class;
        int Delete<T>(List<object> keys, Conditions conditions) where T : class;
        void QueryStr(string qry);
    }
    
}
