using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Website.Models;
using BusinessLogic.Module;
using DataAccess.Interface;
using Microsoft.AspNetCore.Http;
using BusinessLogic.App;
using DataAccess.ModelsViewModels;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using Logger;
namespace Website.Controllers
{
    public class PermissionController : Controller
    {
        private PermissionManager prm;
        private readonly Ilog _logger;
        public PermissionController(IMapper imap_, Ilog logger)
        {
            _logger = logger;
            prm = new PermissionManager(imap_);
        }

        #region Services  
        public IActionResult SubmitAddRoleAccess(string obj)
        {
            TControllerRoleAccess objRoleAccess = new TControllerRoleAccess();
            string errMsg = ValidateRoleAccess(obj, ref objRoleAccess);            
            if (errMsg == "")            {
                objRoleAccess.RoleID = objRoleAccess.RoleID.ToUpper();
                bool isDuplicate = prm.CheckDuplicateRoleAccess(objRoleAccess.ControllerName, objRoleAccess.RoleID, ref errMsg);
                if (errMsg == "")
                {
                    errMsg = isDuplicate ? "Data Already Exist" : "";
                }
                else
                    errMsg = "Internal Server Error";
            }
            if (errMsg == "")
            {
                errMsg = prm.Add(objRoleAccess) == "" ? "" : "Internal Server Error";
            }
            return Z_Result.SetResult(errMsg);
        }
        public IActionResult SubmitDeleteRoleAccess(string obj)
        {
            TControllerRoleAccess objRoleAccess = new TControllerRoleAccess();
            string errMsg = ValidateRoleAccess(obj, ref objRoleAccess);
            objRoleAccess.RoleID = objRoleAccess.RoleID.ToUpper();
            if (errMsg == "")
            {
                bool isDuplicate = prm.CheckDuplicateRoleAccess(objRoleAccess.ControllerName, objRoleAccess.RoleID, ref errMsg);
                if (errMsg == "")
                {
                    errMsg = !isDuplicate ? "Data is not exist" : "";
                }
                else
                    errMsg = "Internal Server Error";
            }
            if (errMsg == "")
            {
                errMsg = prm.DeleteRoleAccess(objRoleAccess) != "" ? "Internal Server Error" : "";
            }
            return Z_Result.SetResult(errMsg);
        }
        public IActionResult SubmitUpdateRoleAccess(string obj)
        {
            TControllerRoleAccess objRoleAccess = new TControllerRoleAccess();
            string errMsg = ValidateRoleAccess(obj, ref objRoleAccess);
            if (errMsg == "")
            {
                objRoleAccess.RoleID = objRoleAccess.RoleID.ToUpper();
                bool isDuplicate = prm.CheckDuplicateRoleAccess(objRoleAccess.ControllerName, objRoleAccess.RoleID, ref errMsg);
                if (errMsg == "")
                {
                    errMsg = !isDuplicate ? "Data is not exist" : "";
                }
                else
                    errMsg = "Internal Server Error";
            }
            if (errMsg == "")
            {
                errMsg = prm.UpdateRoleAccess(objRoleAccess) != "" ? "Internal Server Error" : "";
            }

            return Z_Result.SetResult(errMsg);
        }
        public IActionResult GetListRoleAccess()
        {
            string mssg = "";
            List<TControllerRoleAccess> ret = prm.ReadListRoleAccess(ref mssg);
            return Z_Result.SetResult(mssg, ret);
        }
        public IActionResult SubmitUpdateActionPermission(string obj)
        {
            List<TActionPermission> objActPermission = new List<TActionPermission>();
            string errMsg = ValidateActPermission(obj, ref objActPermission);
            if (errMsg == "")
            {                
                errMsg = prm.UpdateActPermission(objActPermission) != "" ? "Internal Server Error" : "";
            }

            return Z_Result.SetResult(errMsg);
        }
        public IActionResult GetListActionPermission(string controllerName)
        {
            string mssg = "";
            List<TActionPermission> ret = prm.ReadListActionPermission(ref mssg, controllerName);
            return Z_Result.SetResult(mssg, ret);
        }
        private string ValidateRoleAccess(string _objRole, ref TControllerRoleAccess objRole)
        {
            List<string> errMessage = new List<string>();
            try
            {
                objRole = JsonConvert.DeserializeObject<TControllerRoleAccess>(_objRole);
            }
            catch
            {
                //TODO LOG
                errMessage.Add("Object ControllerRoleAccess can't be empty");
            }

            if (string.IsNullOrEmpty(objRole.RoleID))
                errMessage.Add("RoleID can't be empty");
            if (string.IsNullOrEmpty(objRole.ControllerName))
                errMessage.Add("ControllerName can't be empty");

            return string.Join(" \n ", errMessage);
        }
        private string ValidateActPermission(string _objRole, ref List<TActionPermission> obj)
        {
            List<string> errMessage = new List<string>();
            try
            {
                obj = JsonConvert.DeserializeObject<List<TActionPermission>>(_objRole);
            }
            catch(Exception e)
            {
                _logger.ERROR(e.Message);
                errMessage.Add("Object ActionPermission can't be empty");
            }

            if (obj.Count == 0)
                errMessage.Add("Please input at least 1 data");
            else
            {
                foreach (var s in obj)
                {
                    if (string.IsNullOrEmpty(s.ActionName))
                    {
                        errMessage.Add("Some action name is empty");
                        break;
                    }
                }
            }
            return string.Join(" \n ", errMessage);
        }
        #endregion

        public IActionResult AddRoleAccess()
        {
            return View();
        }
        public IActionResult DeleteRoleAccess()
        {
            return View();
        }
        public IActionResult UpdateRoleAccess()
        {
            return View();
        }
        public IActionResult UpdateActionPermission()
        {
            return View();
        }
    }
}
