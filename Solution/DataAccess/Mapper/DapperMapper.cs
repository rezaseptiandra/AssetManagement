using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Interface;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using System.Reflection;

namespace DataAccess.Mapper
{
        /*
        var invoice = connection.Get<Invoice>(1);

        var invoiceList = connection.GetList<Invoice>();

        var webInvoices = connection.GetList<Invoice>(new { Kind = InvoiceKind.WebInvoice });

        var listPaged = connection.GetListPaged<Invoice>(1, 10, "where Code like '%Invoice%'", "Code desc");

        var invoiceId = connection.Insert(new Invoice { Kind = InvoiceKind.WebInvoice, Code = "Insert_Single_1" });

        invoice.Code = "Update_Invoice";
        invoice.Kind = InvoiceKind.StoreInvoice;

        var id = connection.Update(invoice);

        invoice = connection.Get<Invoice>(invoiceId);

        var status = connection.Delete(invoice);

        var count = connection.RecordCount<Invoice>("where Code like '%Invoice%'");
         */
    public class DapperMapper : IMapper
    {
        public DapperMapper()
        {
            CommandTimeOut = 200;
        }
        public IDbConnection ObjConn { get; set; }
        public IDbTransaction ObjTrans { get; set; }
        private int CommandTimeOut { get; set; }
        public string user { get; set; }
        public string host { get; set; }

        public void SetCmdTimeOut(int duration)
        {
            CommandTimeOut = duration;
        }
        public void SetDialect(Dialect dialect)
        {
            SimpleCRUD.Dialect DapperDialect;
            switch (dialect)
            {
                case Dialect.MySQL:
                    DapperDialect = SimpleCRUD.Dialect.MySQL;
                        break;
                case Dialect.PostgreSQL:
                    DapperDialect = SimpleCRUD.Dialect.PostgreSQL;
                        break;
                case Dialect.SQLServer:
                    DapperDialect = SimpleCRUD.Dialect.SQLServer;
                    break;
                case Dialect.SQLite:
                    DapperDialect = SimpleCRUD.Dialect.SQLite;
                    break;

                default:
                    DapperDialect = SimpleCRUD.Dialect.SQLServer;
                    break;
            }
            SimpleCRUD.SetDialect(DapperDialect);
        }

        public void Open()
        {
            this.ObjConn.Open();
        }
        public void BeginTrans()
        {
            this.ObjTrans = this.ObjConn.BeginTransaction();
        }
        public void Commit()
        {
            this.ObjTrans.Commit();
        }
        public void RollBack()
        {
            this.ObjTrans.Rollback();
        }
        public void Close()
        {
            this.ObjConn.Close();
        }

        public int Insert<T>(T model, out object Keys, Conditions conditions, bool GetLastKey = false) where T : class
        {
            List<string> field = new List<string>();
            List<string> val = new List<string>();
            Dictionary<string,object> lstKey = new Dictionary<string, object>();
            string qry = "INSERT INTO " + model.GetType().Name + " (";
            bool isSkip;
            bool isKey;
            bool isRequired;
            bool isReadOnly;
            bool isAnyValueToInsert;
            string spName = conditions.SP_INSERT;//GetStoredProcedureOfInsert(typeof(T));          

            foreach (PropertyInfo objprop in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                isSkip = false;
                isKey = false;
                isRequired = false;
                isReadOnly = false;

                if (objprop.CustomAttributes.Count() > 0)
                {
                    foreach (CustomAttributeData obj in objprop.CustomAttributes.ToList())
                    {
                        isKey = (obj.AttributeType.Name == "KeyAttribute");
                        if (isKey) {
                            var vals_ = model.GetType().GetProperty(objprop.Name).GetValue(model, null);                           
                            lstKey.Add(objprop.Name, vals_);
                        }
                        isRequired = (obj.AttributeType.Name == "RequiredAttribute");
                        isReadOnly = (obj.AttributeType.Name == "ReadOnlyAttribute");
                    }
                    isSkip = (isKey && !isRequired) || isReadOnly ;
                }
                if (!isSkip)
                {
                    var vals_ = model.GetType().GetProperty(objprop.Name).GetValue(model, null);
                    if (vals_ != null)
                    {
                        field.Add(objprop.Name);
                        val.Add(vals_.ToString());
                    }
                }
            }

            isAnyValueToInsert = val.Count > 0;
            if (isAnyValueToInsert)
            {
                StoredProcedureParameter.Insert spInsertParam = new StoredProcedureParameter.Insert();
                object rets;
                if (GetLastKey)
                {
                    Dictionary<string, object> OutKeys = new Dictionary<string, object>();
                    if (!string.IsNullOrEmpty(spName))
                    {
                        spInsertParam.field = "(" + string.Join(",", field) + ")";
                        spInsertParam.value= "('" + string.Join("','", val) + "')";
                        spInsertParam.getkey = true;
                        rets = ObjConn.ExecuteScalar(spName, spInsertParam, ObjTrans, CommandTimeOut, CommandType.StoredProcedure);
                        if (lstKey.Count > 1)
                            OutKeys = lstKey;
                        else
                            OutKeys.Add("Key1", rets);

                        Keys = OutKeys;
                        return 1;

                    }
                    else
                    {
                        qry += string.Join(",", field) + ") VALUES ('" + string.Join("','", val) + "')";
                        qry += ";" + "SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY];";
                        rets = ObjConn.ExecuteScalar(qry, null, ObjTrans, CommandTimeOut);
                        if (lstKey.Count > 1)
                            OutKeys = lstKey;
                        else
                            OutKeys.Add("Key1", rets);

                        Keys = OutKeys;
                        return 1;
                    }                   
                }
                else
                {
                    if (!string.IsNullOrEmpty(spName))
                    {                       
                        spInsertParam.field = "(" + string.Join(",", field) + ")";
                        spInsertParam.value = "('" + string.Join("','", val) + "')";
                        spInsertParam.getkey = false;
                        rets = ObjConn.ExecuteScalar(qry, spInsertParam, ObjTrans, CommandTimeOut, CommandType.StoredProcedure);
                        Keys = null;
                        return (int)rets;
                    }
                    else
                    {
                        qry += string.Join(",", field) + ") VALUES ('" + string.Join("','", val) + "')";
                        Keys = null;
                        return ObjConn.Execute(qry, null, ObjTrans, CommandTimeOut);
                    }
                }
            }
            else
            {
                throw new Exception("No value param to insert");
            }
        }
        public int Update<T>(T model, bool isDirect, Conditions conditions) where T : class
        {
            //TODO DATETIME PROBLEM IF NULL

            //Create Set Value
            string SetValue = "";
            string prefix = "";
            var asdsadf = model.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(x => x.CanWrite && !x.CustomAttributes.Any(y => y.AttributeType.Name == "RequiredAttribute" || y.AttributeType.Name == "ReadOnlyAttribute"));
            foreach (PropertyInfo objprop in model.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(x => x.CanWrite && !x.CustomAttributes
                    .Any(y=>y.AttributeType.Name == "RequiredAttribute" || y.AttributeType.Name == "ReadOnlyAttribute" )))
            {
                prefix = SetValue.Length == 0 ? "" : ", ";
                var VAL = objprop.GetValue(model, null);

                if (VAL != null)
                {
                    if (objprop.PropertyType.Name == "DateTime")
                    {
                        if (VAL.ToString() != "1/1/0001 12:00:00 AM")
                        {
                            SetValue = SetValue + prefix + objprop.Name + "='" + VAL.ToString() + "'";
                        }
                    }
                    else
                        SetValue = SetValue + prefix + objprop.Name + "='" + VAL.ToString() + "'";
                }
            }
            
            //Add Filter Based on Key Models: Auto Add Default Primary Key as Filter
            if (isDirect)
            {
                object keyVal;
                foreach (PropertyInfo objprop in model.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.CustomAttributes.Any(y => y.AttributeType.Name == "KeyAttribute")))
                {
                    keyVal = objprop.GetValue(model, null);
                    if (keyVal == null)
                        break;
                    conditions.AddFilter(objprop.Name, Operator.Equals(keyVal.ToString()));
                }
            }

            //Execute
            string tableName = GetTableName(typeof(T));
            string storedProcedure = conditions.SP_UPDATE;  //GetStoredProcedureOfUpdate(typeof(T));
            if (string.IsNullOrEmpty(storedProcedure))
            {
                string query = "UPDATE " + tableName + " SET " + SetValue + " " + conditions.GetAllFilterParam();
                return ObjConn.Execute(query, null, ObjTrans, CommandTimeOut, CommandType.Text);
            }
            else
            {
                StoredProcedureParameter.Update spParams = new StoredProcedureParameter.Update();
                spParams.filter = conditions.ResultFilter;
                spParams.setvalue = SetValue;
                spParams.isDebug = true;
                if (spParams.isDebug)
                {
                    object execQuery = ObjConn.ExecuteScalar(storedProcedure, spParams, ObjTrans, CommandTimeOut, CommandType.StoredProcedure);
                    return 0;
                }
                else
                    return ObjConn.Execute(storedProcedure, spParams, ObjTrans, CommandTimeOut, CommandType.StoredProcedure);
            }
        }
        public int Delete<T>(List<object> keys, Conditions conditions) where T : class
        {
            if (keys.Count > 0)
            {
                Type obj = typeof(T);
                int keyCount = 0;
                foreach (PropertyInfo objprop in obj.GetProperties().Where(x => x.CustomAttributes.Count() > 0))
                {
                    if (objprop.CustomAttributes.Any(x => x.AttributeType.Name == "KeyAttribute"))
                    {
                        conditions.AddFilter(objprop.Name, Operator.Equals(keys[keyCount]));
                        keyCount++;
                    }
                    if (keyCount >= keys.Count)
                        break;
                }              
            }
            
            string tableName = GetTableName(typeof(T));
            string storedProcedure = conditions.SP_DELETE;//GetStoredProcedureOfDelete(typeof(T));
            if (string.IsNullOrEmpty(storedProcedure))
            {
                string query = "DELETE FROM " + tableName + " " + conditions.GetAllFilterParam();
                return ObjConn.Execute(query, null, ObjTrans, CommandTimeOut, CommandType.Text);
            }
            else
            {
                StoredProcedureParameter.Delete spParams = new StoredProcedureParameter.Delete();
                spParams.filter = conditions.ResultFilter;
                return ObjConn.Execute(storedProcedure, spParams, ObjTrans, CommandTimeOut, CommandType.StoredProcedure);
            }           
        }
        public void QueryStr(string qry)
        {
            throw new NotImplementedException();
        }
        public T ReadOne<T>(List<object> keys, Conditions conditions)
        {
            if (keys.Count > 0)
            {
                Type obj = typeof(T);
                int keyCount = 0;
                foreach (PropertyInfo objprop in obj.GetProperties().Where(x => x.CustomAttributes.Count() > 0))
                {
                    if (objprop.CustomAttributes.Any(x => x.AttributeType.Name == "KeyAttribute"))
                    {
                        conditions.AddFilter(objprop.Name, Operator.Equals(keys[keyCount]));
                        keyCount++;
                    }
                    if (keyCount >= keys.Count)
                        break;
                }
            }

            string storedProcedureName = conditions.SP_SELECT;//GetStoredProcedureOfSelect(typeof(T));
            string tableName = GetTableName(typeof(T));
            if (storedProcedureName != "")
            {
                StoredProcedureParameter.Select spParam = new StoredProcedureParameter.Select()
                {
                    filter = conditions.GetFilterParam(),
                    selectedField = conditions.GetTop1SelectParam(),
                    groupBy = conditions.GetGroupByParam(),
                    orderBy = conditions.GetOrderByParam()
                };
                var retn = ObjConn.Query<T>(storedProcedureName, spParam, ObjTrans, true, CommandTimeOut, CommandType.StoredProcedure);
                if (retn.Count() > 0)
                    return retn.ToList()[0];
                else
                    return default;

            }
            else
            {
                string qry = conditions.GetTop1SelectParam() + " FROM " + tableName + conditions.GetAllFilterParam();
                var retn = ObjConn.Query<T>(qry, null, ObjTrans, true, CommandTimeOut, CommandType.Text);
                if (retn.Count() > 0)
                    return retn.ToList()[0];
                else
                    return default;
            }
        }
        public List<T> ReadList<T>(Conditions conditions)
        {
            string storedProcedureName = conditions.SP_SELECT;//GetStoredProcedureOfSelect(typeof(T));
            string tableName = GetTableName(typeof(T));            

            if (storedProcedureName!="")
            {
                StoredProcedureParameter.Select spParam = new StoredProcedureParameter.Select()
                {
                    filter = conditions.GetFilterParam(),
                    selectedField = conditions.GetSelectParam(),
                    groupBy = conditions.GetGroupByParam(),
                    orderBy = conditions.GetOrderByParam()
                };               
                return ObjConn.Query<T>(storedProcedureName, spParam, ObjTrans, true, CommandTimeOut, CommandType.StoredProcedure).ToList();
            }
            else
            {               
                string qry = conditions.GetSelectParam() +" FROM " + tableName + conditions.GetAllFilterParam();
                return ObjConn.Query<T>(qry, null, ObjTrans, true, CommandTimeOut, CommandType.Text).ToList();
            }

        }
        public List<T> ReadListPaged<T>(Conditions conditions, int pageNumber, int rowsPerPage, out int totalRow)
        {            
            string spName = conditions.SP_SELECT;//GetStoredProcedureOfSelect(typeof(T));           
            string tableName = GetTableName(typeof(T));  
            if (spName!="")
            {
                StoredProcedureParameter.Select spParam = new StoredProcedureParameter.Select()
                {
                    filter = conditions.GetFilterParam(),
                    selectedField = conditions.GetSelectParam(),
                    groupBy = conditions.GetGroupByParam(),
                    orderBy = GetDefaultOrderField(typeof(T), conditions),
                    pageNumber = pageNumber,
                    pageSize = rowsPerPage,
                    isCount = true
                };

                totalRow = (int)ObjConn.ExecuteScalar(spName, spParam, ObjTrans, CommandTimeOut, CommandType.StoredProcedure);
                spParam.isCount = false;
                return ObjConn.Query<T>(spName, spParam, ObjTrans, true, CommandTimeOut, CommandType.StoredProcedure).ToList();
            }
            else
            {
                string qry = conditions.GetSelectParam() +" FROM " + tableName + conditions.GetFilterParam() + conditions.GetGroupByParam();
                string countQuery = "WITH _CTE AS(" + qry + ") SELECT COUNT(*) FROM _CTE";
                string defaultOrderField = GetDefaultOrderField(typeof(T), conditions);

                totalRow = (int)ObjConn.ExecuteScalar(countQuery, null, ObjTrans, CommandTimeOut, CommandType.Text);
                qry = "WITH _CTE AS(" + qry + ") SELECT * FROM _CTE "+ defaultOrderField + " OFFSET " + rowsPerPage + " * (" + pageNumber + " - 1) ROWS FETCH NEXT " + rowsPerPage + " ROWS ONLY;";
                return ObjConn.Query<T>(qry, null, ObjTrans, true, CommandTimeOut, CommandType.Text).ToList();                
            }
        }
        
        private string GetDefaultOrderField(Type type,Conditions cond)
        {
            string defaultField  = cond.GetOrderByParam();
            if (!string.IsNullOrEmpty(defaultField))
                return defaultField;
            else if (cond.ResultSelect.Count > 0)
                return " ORDER BY " + cond.ResultSelect[0];
            else
            {
                foreach (PropertyInfo objprop in type.GetProperties().Where(x => x.CustomAttributes.ToList().Where(y => y.AttributeType.Name == "KeyAttribute").Any()))
                {
                    defaultField = objprop.Name;
                    break;
                }

                if (defaultField == "")
                {
                    foreach (PropertyInfo objprop in type.GetProperties())
                    {
                        defaultField = objprop.Name;
                        break;
                    }
                    defaultField = defaultField == "" ? "ID" : defaultField;
                }
                
            }
           
            return defaultField;
        }
        private string GetTableName(Type type)
        {
            string tableName = type.Name;
            foreach (var tab in type.CustomAttributes.Where(x => x.AttributeType.Name == "TableAttribute"))
            {
                tableName = tab.ConstructorArguments[0].Value.ToString();
            }
            return tableName;
        }
        //private string GetStoredProcedureOfSelect(Type type)
        //{
        //    string SP = "";
        //    PropertyInfo propinf = type.GetProperty("SP_SELECT", BindingFlags.NonPublic | BindingFlags.Static);
        //    if (propinf != null)
        //    {
        //        var obj = propinf.GetValue(null, null).ToString();
        //        if (obj != null)
        //        {
        //            SP = obj.ToString();                    
        //        }
        //    }
        //    return SP;
        //}
        //private string GetStoredProcedureOfDelete(Type type)
        //{
        //    string SP = "";
        //    PropertyInfo propinf = type.GetProperty("SP_DELETE", BindingFlags.NonPublic | BindingFlags.Static);
        //    if (propinf != null)
        //    {
        //        var obj = propinf.GetValue(null, null).ToString();
        //        if (obj != null)
        //        {
        //            SP = obj.ToString();
        //        }
        //    }
        //    return SP;
        //}
        //private string GetStoredProcedureOfUpdate(Type type)
        //{
        //    string SP = "";
        //    PropertyInfo propinf = type.GetProperty("SP_UPDATE", BindingFlags.NonPublic | BindingFlags.Static);
        //    if (propinf != null)
        //    {
        //        var obj = propinf.GetValue(null, null).ToString();
        //        if (obj != null)
        //        {
        //            SP = obj.ToString();
        //        }
        //    }
        //    return SP;
        //}
        //private string GetStoredProcedureOfInsert(Type type)
        //{
        //    string SP = "";
        //    PropertyInfo propinf = type.GetProperty("SP_INSERT", BindingFlags.NonPublic | BindingFlags.Static);
        //    if (propinf != null)
        //    {
        //        var obj = propinf.GetValue(null, null).ToString();
        //        if (obj != null)
        //        {
        //            SP = obj.ToString();
        //        }
        //    }
        //    return SP;
        //}
    }


}
