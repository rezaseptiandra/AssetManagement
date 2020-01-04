using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
namespace DataAccess
{
    public class StoredProcedureParameter
    {
        public class Select
        {
            public Select()
            {
                isDebug = false;
                isCount = false;
                pageNumber = 0;
                pageSize = 0;
            }
            public string filter { get; set; }
            public string selectedField { get; set; }
            public string groupBy { get; set; }
            public string orderBy { get; set; }
            public int pageNumber { get; set; }
            public int pageSize { get; set; }
            public bool isCount { get; set; }
            public bool isDebug { get; set; }
        }
        public class Insert
        {
            public string field { get; set; }
            public string value { get; set; }
            public bool getkey { get; set; }
        }
        public class Update
        {
            public Update()
            {
                isDebug = false;
            }
            public string filter { get; set; }
            public string setvalue { get; set; }
            public bool isDebug { get; set; }
        }
        public class Delete
        {
            public Delete()
            {
                isDebug = false;
            }
            public string filter { get; set; }
            public bool isDebug { get; set; }
        }
    }

}