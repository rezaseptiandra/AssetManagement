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
    public class RoleManager
    {
        private readonly IMapper imap_;
        private ExecResult exec_;
        public RoleManager(IMapper _imap)
        {
            exec_ = new ExecResult();
            imap_ = _imap;
        }
        public bool CheckIsExist(string id, ref string message)
        {            
            MRoleRPO RPO = new MRoleRPO(imap_);
            //RPO.Where(nameof(MRole.RoleID)).Equals(id);
            RPO.Conditions(nameof(MRole.RoleID), Operator.Equals(id));
            if (RPO.ReadOne(ref exec_))
            {
                return RPO.Result.AffectedRow > 0;
            }
            else
                message = exec_.Message;

            return true;
        }
        public string Add(MRole obj)
        {
            MRoleRPO RPO = new MRoleRPO(imap_);
            RPO.Insert(obj, ref exec_); 
            return exec_.Message;
        }
        public string AddBulk(List<MRole> lstObj)
        {
            MRoleRPO RPO = new MRoleRPO(imap_);
            RPO.BeginTrans();
            foreach (var obj in lstObj)
            {
                if (exec_.Success)
                    RPO.Insert(obj, ref exec_);
                else
                    break;
            }
            RPO.EndTrans(exec_);
            return exec_.Message;
        }
        public string Delete(string id)
        {
            MRoleRPO RPO = new MRoleRPO(imap_);
            RPO.Delete(id, ref exec_);
            return exec_.Message;
        }
        public string Update(MRole obj)
        {
            MRoleRPO RPO = new MRoleRPO(imap_);
            RPO.Update(obj, ref exec_);
            return exec_.Message;
        }

        public List<MRole> ReadList(ref string message)
        {            
            MRoleRPO RPO = new MRoleRPO(imap_);
            if (RPO.ReadList(ref exec_))
            {
                return RPO.Result.Collection;
            }
            else            
                message = "Internal Server Error";

            return null;
        }

    }
}
