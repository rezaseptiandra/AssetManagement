using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DataAccess
{
    public enum Order
    {
        [Description("ASC")]
        Ascending,

        [Description("DESC")]
        Descending

    }
    public class GridModel
    {
        public int Draw { get; set; }
        public int Page { get; set; }
        public int TotalPage { get; set; }
        public int TotalRow {get;set;}
        public int RowsPerPage { get; set; }
        public int TotalFiltered { get; set; }
        public object[] Data { get; set; }
        public Dictionary<string,object> ColumnsWithValueFiltered { get; set; }
        public string ValueFilteredForAllColumns { get; set; }
        public List<string> ColumnForSingleValueFiltered { get; set; }
        public Type TypeOfObject { get; set; }
        public string OrderByField { get; set; }
        public string OrderByType { get; set; }
    }
    public class ExecResult
    {
        public string Message { get; set; }
        public bool Success { get; set; }
    }
    public class ObjectConnection : IObjectConnection
    {
        public IDbConnection ObjConn { get; set; }
        public IDbTransaction ObjTrans { get; set; }
        public Dialect Dialect { get; set; }
        public bool WithTrans { get; set; }
    }
    public static class Connection
    {
        internal static ObjectConnection Zafi()
        {
            return new ObjectConnection
            {
                ObjConn = new SqlConnection() { ConnectionString = "" },
                Dialect = Dialect.SQLServer,
                WithTrans = false
            };
        }
        internal static ObjectConnection Connstring1()
        {
            return new ObjectConnection
            {
                ObjConn = new SqlConnection() { ConnectionString = "" }
            };
        }
        internal static ObjectConnection BMI()
        {
            return new ObjectConnection
            {
                ObjConn = new SqlConnection() { ConnectionString = "" }
            };
        }
        internal static IObjectConnection Connstring3()
        {
            return new ObjectConnection
            {
                ObjConn = new SqlConnection() { ConnectionString = "" }
            };
        }
    }
    

}
