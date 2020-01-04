using DataAccess;
using DataAccess.Interface;
using DataAccess.ModelsViewModels;
using DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogic.App
{
    public class MenuManager
    {
        private readonly IMapper imap_;
        private ExecResult exec;
        public MenuManager(IMapper _imap)
        {
            exec = new ExecResult();
            imap_ = _imap;
        }
        public bool CheckIsExist(string id, ref string message)
        {
            MMenuRPO objRPO = new MMenuRPO(imap_);
            objRPO.Conditions(nameof(MMenu.ID), Operator.Equals(id));
            //objRPO.Where(nameof(MMenu.ID)).Equals(id);
            if (objRPO.ReadList(ref exec))
            {
                return objRPO.Result.AffectedRow > 0;
            }
            else
                message = exec.Message;

            return true;
        }
        public string Add(MMenu obj)
        {
            MMenuRPO RPO = new MMenuRPO(imap_);
            RPO.Insert(obj, ref exec);
            return exec.Message;
        }
        public string AddBulk(List<MMenu> lstObjRole)
        {
            MMenuRPO roleRPO = new MMenuRPO(imap_);
            foreach (var obj in lstObjRole)
            {
                if (exec.Success)
                    roleRPO.Insert(obj, ref exec);
                else
                    break;
            }
            return exec.Message;
        }
        public string Delete(string id)
        {
            MMenuRPO roleRPO = new MMenuRPO(imap_);
            roleRPO.Delete(id, ref exec);
            return exec.Message;
        }
        public string Update(MMenu objRole)
        {
            MMenuRPO roleRPO = new MMenuRPO(imap_);
            roleRPO.Update(objRole, ref exec);
            return exec.Message;
        }

        public List<MMenu> ReadList(ref string message)
        {
            MMenuRPO roleRPO = new MMenuRPO(imap_);
            if (roleRPO.ReadList(ref exec))
            {
                return roleRPO.Result.Collection;
            }
            else
                message = "Internal Server Error";

            return null;
        }

    }
}
