using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess;
using DataAccess.ModelsViewModels;
using DataAccess.Interface;
using DataAccess.Repository;

namespace BusinessLogic.Module
{
    public class MasterABP
    {
        private readonly IMapper imap_;
        private ExecResult exec;
        public MasterABP(IMapper _imap)
        {
            exec = new ExecResult();
            imap_ = _imap;
        }
        public bool CheckIsExist(string id, ref string message)
        {
            MABPDA objRPO = new MABPDA(imap_);
            objRPO.Conditions(nameof(MABP.ABPID), Operator.Equals(id));
            if (objRPO.ReadList(ref exec))
            {
                return objRPO.Result.AffectedRow > 0;
            }
            else
                message = exec.Message;

            return true;
        }
        public string Add(MABP obj)
        {
            MABPDA RPO = new MABPDA(imap_);
            RPO.Insert(obj, ref exec);
            return exec.Message;
        }
        public string AddBulk(List<MABP> lstObjRole)
        {
            MABPDA roleRPO = new MABPDA(imap_);
            foreach (var obj in lstObjRole)
            {
                if (exec.Success)
                    roleRPO.Insert(obj, ref exec);
                else
                    break;
            }
            return exec.Message;
        }
        public string Delete(List<object> key)
        {
            string.Join("','", key);
            MABPDA ABPrepo = new MABPDA(imap_);
            ABPrepo.Conditions(new Conditions(nameof(MABP.ABPID), Operator.In("'"+ string.Join("','", key) + "'")));
            MABP toUpdate = new MABP() { Flag=4 };
            ABPrepo.UpdateFiltered(toUpdate, ref exec);
            return exec.Message;
        }
        public string Delete(string id)
        {
            MABPDA roleRPO = new MABPDA(imap_);
            roleRPO.Delete(id, ref exec);
            return exec.Message;
        }
        public string Update(List<MABP> objRoles)
        {
            MABPDA ABPrepo = new MABPDA(imap_);
            ABPrepo.BeginTrans();
            foreach (var obj in objRoles)
            {
                ABPrepo.Update(obj, ref exec);
                if (!exec.Success)
                    break;
            }
            ABPrepo.EndTrans(exec);
            return exec.Message;
        }
        public string Update(MABP objRole)
        {
            MABPDA ABPrepo = new MABPDA(imap_);
            ABPrepo.Update(objRole, ref exec);  
            return exec.Message;
        }
        public List<MABPVM> ReadList(GridModel gmd, out int totalRow, ref string message)
        {
            MABPVMDA RPO = new MABPVMDA(imap_);
            if (RPO.ReadListPaged(gmd, out totalRow, ref exec))
            {
                return RPO.Result.Collection;
            }
            else
                message = "Internal Server Error";

            return null;
        }
        public MABPVM ReadDetail(string id, ref string message)
        {
            MABPVMDA objRPO = new MABPVMDA(imap_);
            objRPO.Conditions(nameof(MABPVM.ABPID), Operator.Equals(id));            
            if (objRPO.ReadOne(ref exec))
            {
                return objRPO.Result.Row;
            }
            else
                message = exec.Message;

            return null;
        }

        private string validation() {

            return "";
        }
    }
}
