using DataAccess;
using DataAccess.Interface;
using DataAccess.ModelsViewModels;
using DataAccess.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Logger;

namespace Website.Attributes
{
    public class ZActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            Ilog _logger = context.HttpContext.RequestServices.GetService(typeof(Ilog)) as Ilog;
            IMapper _imapper = context.HttpContext.RequestServices.GetService(typeof(IMapper)) as IMapper;
            string username = "unknown-user";
            string objIdentity = context.HttpContext.Session.GetString(Helpers.SessionKeyUser.Key);
            if (!string.IsNullOrEmpty(objIdentity))
                username = JsonConvert.DeserializeObject<Models.UserSessionModel>(objIdentity).username;
            
            _logger.SetLogInfo(username, context.HttpContext.Request.Path.Value);
            _imapper.user = username;
            _imapper.host = context.HttpContext.Connection.RemoteIpAddress.ToString();

            //when controller not allowanonymous 
            if (!context.Filters.Any(item => item is IAllowAnonymousFilter))
            {
                bool isValiduser = false;
                if (!string.IsNullOrEmpty(objIdentity))
                {
                    ExecResult exec = new ExecResult();
                    CheckisAdminRPO userRoleRPOs = new CheckisAdminRPO(_imapper);
                    userRoleRPOs.Conditions(nameof(TuserRole.Username), Operator.Equals(username));
                    userRoleRPOs.ReadList(ref exec);
                    //if not success check administrator
                    if (!exec.Success)
                    {
                        isValiduser = false;
                    }
                    //if not admin then check allowed actionController
                    else if (userRoleRPOs.Result.AffectedRow == 0)
                    {
                        ActPermissionVMRPO actiPermissionRPO = new ActPermissionVMRPO(_imapper);
                        string controllerName = (string)context.RouteData.Values["controller"]+"Controller";
                        string actionName = (string)context.RouteData.Values["action"];
                        Conditions cnd = new Conditions();
                        cnd.AddFilter(nameof(ActPermissionVM.Username), Operator.Equals(username));
                        cnd.AddFilter(nameof(ActPermissionVM.ControllerName), Operator.Equals(controllerName));
                        cnd.AddFilter(nameof(ActPermissionVM.ActionName), Operator.Equals(actionName));
                        cnd.AddFilter(nameof(ActPermissionVM.IsAllowed), Operator.Equals("1"));
                        actiPermissionRPO.Conditions(cnd);
                        if (actiPermissionRPO.ReadList(ref exec))
                        {
                            List<ActPermissionVM> tym = actiPermissionRPO.Result.Collection;                         
                            isValiduser = actiPermissionRPO.Result.AffectedRow > 0;                            
                        }
                    }
                    else
                        isValiduser = true; //is admin
                }
                                    
                if (!isValiduser)
                {
                    _logger.WARNING("Not Authorized");
                   context.Result = new ViewResult
                    {
                        ViewName = "../Account/NotAuthorized"
                    };
                }                
            }
        }
    }
}
