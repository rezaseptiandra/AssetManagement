using System;
using System.Collections.Generic;
using Dapper;
using System.Linq;
using DataAccess.Repository;
using DataAccess.Interface;

namespace DataAccess.ModelsViewModels
{
    [Table("ZLogger")]
    public class ZLogger
    {
        [Key]
        public int ID { get; set; }
        public string Username { get; set; }
        public string Level { get; set; }
        public string Path { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }

    }

    public class ZLoggerVM
    {

        public int ID { get; set; }
        public string Username { get; set; }
        public string Level { get; set; }
        public string Path { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
        #region Main View
        //[IgnoreInsert]
        //[IgnoreUpdate]
        //public string NewField { get; set; }
        #endregion

    }

    public class ZLoggerCustomVM
    {
        public static string SPName = "selcustomsp";
        [IgnoreInsert]
        [IgnoreUpdate]
        public string Field1 { get; set; }
        [IgnoreInsert]
        [IgnoreUpdate]
        public string Field2 { get; set; }
    }
    public class ZLoggerRPO : FullDataAccess<ZLogger>
    {
        public ZLoggerRPO(IMapper _mpp)
        {
            base.Initialize(_mpp);
        }
    }

}
