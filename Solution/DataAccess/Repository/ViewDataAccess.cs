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
    public class ViewDataAccess<T> : BaseDataAccess<T> where T: class
    {
        public bool ReadOne(ref ExecResult execRes)
        {
            ClearResult();
            if (Open(true))
            {
                try
                {
                    _Result.Row = objMapper.ReadOne<T>(new List<object>(),DataConditions);
                    _Result.AffectedRow = _Result.Row != null ? 1 : 0;
                    _Result.Message = execRes.Message = _Result.Row != null ? "" : "No Data Selected";
                    _Result.Success = execRes.Success = true;
                }
                catch (Exception e)
                {
                    //TODO Log e
                    _Result.Message = execRes.Message = "Read Failed";
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
        public bool ReadOne(List<object> keys, ref ExecResult execRes)
        {
            ClearResult();
            if (Open(true))
            {
                try
                {
                    _Result.Row = objMapper.ReadOne<T>(keys, DataConditions);
                    _Result.AffectedRow = _Result.Row != null ? 1 : 0;
                    _Result.Message = execRes.Message = _Result.Row != null ? "" : "No Data Selected";
                    _Result.Success = execRes.Success = true;
                }
                catch (Exception e)
                {
                    //TODO Log e
                    _Result.Message = execRes.Message = "Read Failed";
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
        public bool ReadOne(object key1, object key2, ref ExecResult execRes)
        {
            return ReadOne(new List<object>() { key1, key2 }, ref execRes);
        }
        public bool ReadOne(object key1, ref ExecResult execRes)
        {
            return ReadOne(new List<object>() { key1 }, ref execRes);
        }
        public bool ReadList(ref ExecResult execRes)
        {
            ClearResult();
            if (Open(true))
            {
                try
                {
                    _Result.Collection = objMapper.ReadList<T>(DataConditions);
                    _Result.AffectedRow = _Result.Collection.Count;
                    _Result.Message = execRes.Message = "";
                    _Result.Success = execRes.Success = true;
                }
                catch (Exception e)
                {
                    //TODO Log e
                    _Result.Message = execRes.Message = "Read Failed";
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
        public bool ReadListPaged(GridModel gridModel, out int totalrow, ref ExecResult execRes)
        {
            totalrow = 0;
            ClearResult();
            if (Open(true))
            {
                try
                {
                    DataConditions.SetGridFilter(gridModel);
                    _Result.Collection = objMapper.ReadListPaged<T>(DataConditions, gridModel.Page, gridModel.RowsPerPage, out totalrow);
                    _Result.AffectedRow = _Result.Collection.Count;
                    _Result.Message = execRes.Message = "";
                    _Result.Success = execRes.Success = true;
                }
                catch (Exception e)
                {
                    //TODO Log e
                    _Result.Message = execRes.Message = "Read Failed";
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
        public bool ReadListPaged(GridModel gridModel, ref ExecResult execRes) //TODO COMPARE METHOD
        {
           int totalrow = 0;
            ClearResult();
            if (Open(true))
            {
                try
                {
                    DataConditions.SetGridFilter(gridModel);
                    _Result.Collection = objMapper.ReadListPaged<T>(DataConditions, gridModel.Page, gridModel.RowsPerPage, out totalrow);
                    _Result.AffectedRow = 0;
                    _Result.Message = execRes.Message = "";
                    _Result.Success = execRes.Success = true;
                }
                catch (Exception e)
                {
                    //TODO Log e
                    _Result.Message = execRes.Message = "Read Failed";
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

        protected void Set_SP_Select(string SPName)
        {
            this.DataConditions.SP_SELECT = SPName;
        }
    }
}
