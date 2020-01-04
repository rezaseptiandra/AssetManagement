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
    public class PermissionManager
    {
        private readonly IMapper imap_;
        private ExecResult exec;
        public PermissionManager(IMapper _imap)
        {
            exec = new ExecResult();
            imap_ = _imap;
        }
        public bool CheckDuplicateRoleAccess(string controllerName, string roleID, ref string message)
        {
            TControllerRoleAccessRPO crRPO = new TControllerRoleAccessRPO(imap_);
            Conditions cnd = new Conditions();
            cnd.AddFilter(nameof(TControllerRoleAccess.ControllerName), Operator.Equals(controllerName));
            cnd.AddFilter(nameof(TControllerRoleAccess.RoleID), Operator.Equals(roleID));
            crRPO.Conditions(cnd);
            //crRPO.Where(nameof(TControllerRoleAccess.ControllerName)).Equals(controllerName);
            //crRPO.Where(nameof(TControllerRoleAccess.RoleID)).Equals(roleID);
            if (crRPO.ReadList(ref exec))
            {
                return crRPO.Result.AffectedRow > 0;
            }
            else
                message = exec.Message;

            return true;
        }
        public string Add(TControllerRoleAccess obj)
        {
            TControllerRoleAccessRPO roleRPO = new TControllerRoleAccessRPO(imap_);
            roleRPO.Insert(obj, ref exec);
            return exec.Message;
        }
        public string AddBulk(List<TControllerRoleAccess> lstObjRole)
        {
            TControllerRoleAccessRPO roleRPO = new TControllerRoleAccessRPO(imap_);
            foreach (var obj in lstObjRole)
            {
                if (exec.Success)
                    roleRPO.Insert(obj, ref exec);
                else
                    break;
            }
            return exec.Message;
        }
        public string DeleteRoleAccess(TControllerRoleAccess obj)
        {
            TControllerRoleAccessRPO raccRPO = new TControllerRoleAccessRPO(imap_);
            //raccRPO.Where(nameof(TControllerRoleAccess.ControllerName)).Equals(obj.ControllerName);
            //raccRPO.Where(nameof(TControllerRoleAccess.RoleID)).Equals(obj.RoleID);
            Conditions cnd = new Conditions();
            cnd.AddFilter(nameof(TControllerRoleAccess.ControllerName), Operator.Equals(obj.ControllerName));
            cnd.AddFilter(nameof(TControllerRoleAccess.RoleID), Operator.Equals(obj.RoleID));
            raccRPO.Conditions(cnd);
            raccRPO.Delete(obj, ref exec);
            return exec.Message;
        }
        public string UpdateRoleAccess(TControllerRoleAccess obj)
        {
            TControllerRoleAccessRPO RPO = new TControllerRoleAccessRPO(imap_);
            //RPO.Where(nameof(TControllerRoleAccess.ControllerName)).Equals(obj.ControllerName);
            //RPO.Where(nameof(TControllerRoleAccess.RoleID)).Equals(obj.RoleID);
            Conditions cnd = new Conditions();
            cnd.AddFilter(nameof(TControllerRoleAccess.ControllerName), Operator.Equals(obj.ControllerName));
            cnd.AddFilter(nameof(TControllerRoleAccess.RoleID), Operator.Equals(obj.RoleID));
            RPO.Conditions(cnd);
            RPO.Update(obj, ref exec);
            return exec.Message;
        }
        public string UpdateActPermission(List<TActionPermission> obj)
        {
            TActionPermissionRPO RPO = new TActionPermissionRPO(imap_);
            RPO.BeginTrans();
            RPO.Conditions(nameof(TActionPermission.ControllerName),Operator.Equals(obj[0].ControllerName));
            RPO.Delete(ref exec);
            foreach (var s in obj)
            {
                if (exec.Success)
                    RPO.Insert(s, ref exec);
                else
                    break;
            }
            RPO.EndTrans(exec);
            return exec.Message;
        }

        public List<TControllerRoleAccess> ReadListRoleAccess(ref string message)
        {
            TControllerRoleAccessRPO RPO = new TControllerRoleAccessRPO(imap_);
            if (RPO.ReadList(ref exec))
            {
                return RPO.Result.Collection;
            }
            else
                message = "Internal Server Error";

            return null;
        }

        public List<TActionPermission> ReadListActionPermission(ref string message, string controllerName = "")
        {
            TActionPermissionRPO RPO = new TActionPermissionRPO(imap_);
            if (!string.IsNullOrEmpty(controllerName))
            {
                RPO.Conditions(nameof(TActionPermission.ControllerName), Operator.Equals(controllerName));
                //RPO.Where(nameof(TActionPermission.ControllerName)).Equals(controllerName);
            }
            if (RPO.ReadList(ref exec))
            {
                return RPO.Result.Collection;
            }
            else
                message = "Internal Server Error";

            return null;
        }

    }
}
