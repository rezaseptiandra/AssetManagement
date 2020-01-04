using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Interface;
using System.Data;
using System.Reflection;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace DataAccess.Repository
{    
    public abstract partial class BaseDataAccess<T> where T : class
    {
        protected Conditions DataConditions { get; set; }
        public void Conditions(Conditions fcond)
        {
            fcond.SP_SELECT = DataConditions.SP_SELECT;
            fcond.SP_INSERT = DataConditions.SP_INSERT;
            fcond.SP_DELETE = DataConditions.SP_DELETE;
            fcond.SP_UPDATE = DataConditions.SP_UPDATE;
            DataConditions = fcond;
        }
        public void Conditions(string Field, string operatorAndValue)
        {
            DataConditions.AddFilter(Field,operatorAndValue);
        }
    }
    public abstract partial class BaseDataAccess<T> where T : class
    {
        protected void Initialize(IMapper objMapper)
        {
            this.objMapper = objMapper;
            //Where_ = new ConditionFilters(typeof(T));//TODO DELETE
            DataConditions = new Conditions();
            var firstUpInterface = NetworkInterface.GetAllNetworkInterfaces()
                            .OrderByDescending(c => c.Speed)
                            .FirstOrDefault(c => c.NetworkInterfaceType != NetworkInterfaceType.Loopback && c.OperationalStatus == OperationalStatus.Up);
            if (firstUpInterface != null)
            {
                var props = firstUpInterface.GetIPProperties();
                // get first IPV4 address assigned to this interface
                var firstIpV4Address = props.UnicastAddresses
                    .Where(c => c.Address.AddressFamily == AddressFamily.InterNetwork)
                    .Select(c => c.Address)
                    .FirstOrDefault();
                          
                if(firstIpV4Address.ToString() == "192.168.1.11")
                    SetObjConn(Connection.Zafi());
                else
                    SetObjConn(Connection.Zafi());
            }
            else
                SetObjConn(Connection.Zafi());

            _Result = new CommandResult()
            {
                AffectedRow = 0,
                Success = false
            };
        }
    }  
    public abstract partial class BaseDataAccess<T> where T : class
    {
        protected ObjectConnection _ObjConn { get; set; }
        protected IMapper objMapper;
        protected Ilog objLog;
        protected CommandResult _Result{ get; set; } 
        public class CommandResult
        {
            public bool Success { get; set; }
            public string Message { get; set; }
            public int AffectedRow { get; set; }
            public object LastInsertedKeyID { get; set; }
            public List<T> Collection { get; set; }
            public object CollectionVM { get; set; }
            internal void SetCollectionVM<U>(List<U> model) where U : new()
            {
                CollectionVM = model;
            }
            public T Row { get; set; }
            public object RowVM { get; set; }
        }
        //protected ConditionFilters Where_ { get; set; }
        public void SetGridFilter(GridModel gridModel)
        {
            //Where_.SetGridFilter(gridModel, typeof(T));
            DataConditions.SetGridFilter(gridModel);
        }
        //public ConditionFilters Where(string Field)
        //{
            //Where_.AddFIeld(Field);
           // return Where_;
        //}
        //public ConditionFilters SelectField(string Field)
        //{
        //    Where_.SelectField(Field);
        //    return this.Where_;
        //}
        //public ConditionFilters AddGroupBy(string GrpBy)
        //{
        //    Where_.AddGroupBy(GrpBy);
        //    return this.Where_;
        //}
        //public ConditionFilters AddOrderBy(string Field, OrderType OrdTyp)
        //{
        //    Where_.AddOrderBy(Field, OrdTyp);
        //    return this.Where_;
        //}
        public ObjectConnection ObjConn
        {
            get
            {
                _ObjConn.ObjConn = objMapper.ObjConn;
                _ObjConn.ObjTrans = objMapper.ObjTrans;
                return _ObjConn;
            }
        }
        public void SetObjConn(ObjectConnection _objConn)
        {
            _ObjConn = _objConn;
            objMapper.SetDialect(_ObjConn.Dialect);
            objMapper.ObjConn = _objConn.ObjConn;
            objMapper.ObjTrans = _objConn.ObjTrans;
        }
        public CommandResult Result
        {
            get
            {
                return _Result;
            }
        }
        protected bool Open(bool withCheckTrans = false)
        {
            bool ret = false;            
            if (withCheckTrans)
            {
                if (!_ObjConn.WithTrans) objMapper.Open();
            }
            else
            {
                objMapper.Open();
            }
            ret = true; 
            return ret;
        }
        protected void Close(bool withCheckTrans = false)
        {
            if (withCheckTrans)
            {
                if (!_ObjConn.WithTrans) objMapper.Close();
            }
            else
            {
                objMapper.Close();
            }
        }
        protected void ClearFilter()
        {
            //Where_ = new ConditionFilters();
            DataConditions = new Conditions();
        }
        protected void ClearResult()
        {
            _Result.Row = null;
            _Result.Collection = null;
            _Result.Message = null;
            _Result.Success = false;
            _Result.AffectedRow = 0;
            _Result.LastInsertedKeyID = 0;

        }
        protected void SetGridFilter(GridModel gridModel, Type type)
        {
            PropertyInfo[] clfilt = type.GetProperties();
            List<string> rett = new List<string>();
            var colres = from grdModel in gridModel.ColumnsWithValueFiltered
                         join propCol in clfilt.Where(x => x.CustomAttributes.Where(n => n.AttributeType.Name.Contains("Ignore")).ToList().Count == 0)
                         on grdModel.Key.ToLower() equals propCol.Name.ToLower()
                         select new
                         {
                             grdModel.Value,
                             propCol.CustomAttributes,
                             propCol.PropertyType,
                             propCol.Name
                         };
            foreach (var coldata in colres)
            {
                string val = coldata.Value.ToString();
                if (coldata.PropertyType.Name.ToLower() == nameof(Boolean).ToLower())
                {
                    if ("yes".Contains(val.ToLower()))
                    {
                        val = "1";                        
                    }
                    else if ("no".Contains(val.ToLower()))
                    {
                        val = "0";                        
                    }                    
                }
                DataConditions.AddFilter(coldata.Name, Operator.Like(val));
            }

            var colress = from propCol in clfilt.Where(x => x.CustomAttributes.Where(n => n.AttributeType.Name.Contains("Ignore")).ToList().Count == 0)
                          join grdModel in gridModel.ColumnForSingleValueFiltered.ToList()
                          on propCol.Name.ToLower() equals grdModel.ToLower()
                          select new
                          {
                              propCol.CustomAttributes,
                              propCol.PropertyType,
                              propCol.Name
                          };

            Conditions ConditionFilterForAll = new Conditions();
            foreach (var coldata in colress)
            {                
                string val = gridModel.ValueFilteredForAllColumns;
                if (coldata.PropertyType.Name.ToLower() == nameof(Boolean).ToLower())
                {
                    if ("yes".Contains(val.ToLower()))
                    {
                        val = "1";                        
                    }
                    else if ("no".Contains(val.ToLower()))
                    {
                        val = "0";                        
                    }
                }
                ConditionFilterForAll.AddFilter_OR(coldata.Name, Operator.Like(val));
            }
            if(colress.Any())
                DataConditions.AddFilter(ConditionFilterForAll);
        }
        public void BeginTrans()
        {
            Open();
            objMapper.BeginTrans();
            _ObjConn.WithTrans = true;
        }
        public void EndTrans(bool commit = true)
        {
            if (commit)
                objMapper.Commit();
            else
                objMapper.RollBack();
            Close();
            _ObjConn.WithTrans = false;
        }
        public void EndTrans(ExecResult exec)
        {
            if (exec.Success)
                objMapper.Commit();
            else
                objMapper.RollBack();
            Close();
            _ObjConn.WithTrans = false;
        }

        protected T InsertFillBaseModel(T model)
        {
            PropertyInfo propertyInfo = model.GetType().GetProperty("CreatedDate");
            if(propertyInfo != null)
            propertyInfo.SetValue(model, Convert.ChangeType(DateTime.Now.ToString(), propertyInfo.PropertyType), null);

            propertyInfo = model.GetType().GetProperty("CreatedBy");
            if (propertyInfo != null)
                propertyInfo.SetValue(model, Convert.ChangeType(objMapper.user, propertyInfo.PropertyType), null);

            propertyInfo = model.GetType().GetProperty("CreatedHost");
            if (propertyInfo != null)
                propertyInfo.SetValue(model, Convert.ChangeType(objMapper.host, propertyInfo.PropertyType), null);

            propertyInfo = model.GetType().GetProperty("ModifiedDate");
            if (propertyInfo != null)
                propertyInfo.SetValue(model, Convert.ChangeType(DateTime.Now.ToString(), propertyInfo.PropertyType), null);

            propertyInfo = model.GetType().GetProperty("ModifiedBy");
            if (propertyInfo != null)
                propertyInfo.SetValue(model, Convert.ChangeType(objMapper.user, propertyInfo.PropertyType), null);

            propertyInfo = model.GetType().GetProperty("ModifiedHost");
            if (propertyInfo != null)
                propertyInfo.SetValue(model, Convert.ChangeType(objMapper.host, propertyInfo.PropertyType), null);

            propertyInfo = model.GetType().GetProperty("Flag");
            if (propertyInfo != null)
                propertyInfo.SetValue(model, Convert.ChangeType("0", propertyInfo.PropertyType), null);

            return model;
        }
        protected T UpdateFillBaseModel(T model)
        {
            PropertyInfo  propertyInfo = model.GetType().GetProperty("ModifiedDate");
            if (propertyInfo != null)
                propertyInfo.SetValue(model, Convert.ChangeType(DateTime.Now.ToString(), propertyInfo.PropertyType), null);
                        
            propertyInfo = model.GetType().GetProperty("ModifiedBy");
            if (propertyInfo != null)
                propertyInfo.SetValue(model, Convert.ChangeType(objMapper.user, propertyInfo.PropertyType), null);

            propertyInfo = model.GetType().GetProperty("ModifiedHost");
            if (propertyInfo != null)
                propertyInfo.SetValue(model, Convert.ChangeType(objMapper.host, propertyInfo.PropertyType), null);

            propertyInfo = model.GetType().GetProperty("Flag");
            if (propertyInfo != null)
                propertyInfo.SetValue(model, Convert.ChangeType("0", propertyInfo.PropertyType), null);

            return model;
        }

    }

}
