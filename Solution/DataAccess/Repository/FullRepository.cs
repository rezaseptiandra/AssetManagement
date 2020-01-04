using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Interface;
using DataAccess.ModelsViewModels;
using System.Data;
using System.Reflection;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace DataAccess.Repository
{
    public class FullDataAccess<T> : ViewDataAccess<T> where T : class
    {
        public bool InsertGetKeys(T model, ref ExecResult execRes)
        {
            return Insert_(model, true, ref execRes);
        }
        public bool Insert(T model, ref ExecResult execRes)
        {
            return Insert_(model, false, ref execRes);
        }
        private bool Insert_(T model, bool withKey, ref ExecResult execRes)
        {
            ClearResult();
            if (Open(true))
            {
                try
                {
                    object keys;
                    model = InsertFillBaseModel(model);
                    _Result.AffectedRow = objMapper.Insert(model, out keys, DataConditions, withKey);
                    _Result.LastInsertedKeyID = keys;
                    _Result.Message = execRes.Message = "";
                    _Result.Success = execRes.Success = true;
                }
                catch (Exception e)
                {
                    //TODO Log e
                    _Result.Message = execRes.Message = "Insert Failed" + e.Message;
                    _Result.Success = execRes.Success = false;
                }
            }
            else
            {
                this._Result.Message = execRes.Message = "Open Connection Failed";
                this._Result.Success = execRes.Success = false;
            }
            Close(true);
            ClearFilter();
            return _Result.Success;
        }        
        public bool Update(T model, ref ExecResult execRes)
        {           
            ClearResult();
            if (Open(true))
            {
                try
                {
                    model = UpdateFillBaseModel(model);
                    object res = objMapper.Update(model, true, DataConditions); 
                    _Result.AffectedRow = (int)res;
                    _Result.Message = execRes.Message = "";
                    _Result.Success = execRes.Success = true;
                }
                catch (Exception e)
                {
                    //TODO Log e
                    _Result.Message = execRes.Message = "Update Failed" + e.Message;
                    _Result.Success = execRes.Success = false;
                }
            }
            else
            {
                this._Result.Message = execRes.Message = "Open Connection Failed";
                this._Result.Success = execRes.Success = false;
            }
            Close(true);
            ClearFilter();
            return _Result.Success;
        }
        public bool UpdateFiltered(T model, ref ExecResult execRes)
        {

            ClearResult();
            if (Open(true))
            {
                try
                {
                    model = UpdateFillBaseModel(model);
                    object res = objMapper.Update(model, false, DataConditions);
                    _Result.AffectedRow = (int)res;
                    _Result.Message = execRes.Message = "";
                    _Result.Success = execRes.Success = true;
                }
                catch (Exception e)
                {
                    //TODO Log e
                    _Result.Message = execRes.Message = "Update Failed" + e.Message;
                    _Result.Success = execRes.Success = false;
                }
            }
            else
            {
                _Result.Message = execRes.Message = "Open Connection Failed";
                _Result.Success = execRes.Success = false;
            }
            Close(true);
            ClearFilter();
            return _Result.Success;
        }
        public bool Delete(ref ExecResult execRes)
        {
            ClearResult();
            if (Open(true))
            {
                try
                {
                    _Result.AffectedRow = 0;
                    _Result.AffectedRow = objMapper.Delete<T>(new List<object>(), DataConditions);
                    _Result.Message = execRes.Message = "";
                    _Result.Success = execRes.Success = true;
                }
                catch (Exception e)
                {
                    //TODO Log e
                    _Result.Message = execRes.Message = "Delete Failed" + e.Message;
                    _Result.Success = execRes.Success = false;
                }
            }
            else
            {
                this._Result.Message = execRes.Message = "Open Connection Failed";
                this._Result.Success = execRes.Success = false;
            }
            Close(true);
            ClearFilter();
            return _Result.Success;
        }
        public bool Delete(List<object> keys, ref ExecResult execRes)
        {
            ClearResult();
            if (Open(true))
            {
                try
                {
                    _Result.AffectedRow = 0;
                    _Result.AffectedRow = objMapper.Delete<T>(keys, DataConditions);
                    _Result.Message = execRes.Message = "";
                    _Result.Success = execRes.Success = true;
                }
                catch (Exception e)
                {
                    //TODO Log e
                    _Result.Message = execRes.Message = "Delete Failed" + e.Message;
                    _Result.Success = execRes.Success = false;
                }
            }
            else
            {
                this._Result.Message = execRes.Message = "Open Connection Failed";
                this._Result.Success = execRes.Success = false;
            }
            Close(true);
            ClearFilter();
            return _Result.Success;
        }
        public bool Delete(object key1, ref ExecResult execRes)
        {
            return Delete(new List<object>() { key1 }, ref execRes);
        }
        public bool Delete(object key1, object key2, ref ExecResult execRes)
        {
            return Delete(new List<object>() { key1, key2 }, ref execRes);
        }

        protected void Set_SP_Insert(string SPName)
        {
            this.DataConditions.SP_INSERT = SPName;
        }
        protected void Set_SP_Update(string SPName)
        {
            this.DataConditions.SP_UPDATE = SPName;
        }
        protected void Set_SP_Delete(string SPName)
        {
            this.DataConditions.SP_DELETE = SPName;
        }
    }
}
