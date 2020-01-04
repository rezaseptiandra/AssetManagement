using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Website.Models;
using DataAccess.Interface;
using Microsoft.AspNetCore.Http;
using BusinessLogic.App;
using DataAccess.ModelsViewModels;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using Logger;
namespace Website.Controllers
{
    public class AccountController : Controller
    {
        private AccountManager acc;
        private readonly Ilog _logger;
        public AccountController(IMapper imap_, Ilog logger)
        {
            _logger = logger;
            acc = new AccountManager(imap_);
        }

        #region Services        
        [AllowAnonymous]
        public IActionResult Logout()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(Helpers.SessionKeyUser.Key)))
            {
                HttpContext.Session.Remove(Helpers.SessionKeyUser.Key);
            }
            return Z_Result.SetResult(""); 
        }
        [AllowAnonymous]
        public IActionResult Authenticate(string username, string password)
        {
            string errMsg;
            int? sessioninputTimes = HttpContext.Session.GetInt32(Helpers.SessionKeyUser.KeyOfInputPass);
            if (sessioninputTimes == null)
            {
                errMsg = string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) ? "Username or Password can't be empty" : "";
                HttpContext.Session.SetInt32(Helpers.SessionKeyUser.KeyOfInputPass, 1);
            }
            else
            {
                int? locked = HttpContext.Session.GetInt32(Helpers.SessionKeyUser.KeyOfLockedUser);
                if (locked == 1)
                {
                    errMsg = "account is locked";
                }
                else if (sessioninputTimes >= 5)
                {
                    if (locked != 1)
                    {
                        HttpContext.Session.SetInt32(Helpers.SessionKeyUser.KeyOfLockedUser, 1);
                    }
                    errMsg = "incorrect username or pass 5 times";
                }
                else
                {
                    errMsg = string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) ? "Username or Password can't be empty" : "";
                    HttpContext.Session.SetInt32(Helpers.SessionKeyUser.KeyOfInputPass, (int)sessioninputTimes + 1);
                }
            }

            
            if (errMsg == "")
            {
                MUserVM obj = acc.Login(username, password, out errMsg);
                if (errMsg == "")
                {
                    HttpContext.Session.SetInt32(Helpers.SessionKeyUser.KeyOfInputPass, 0);
                    MUser result = obj.objUser;
                    UserSessionModel userSessionModel = new UserSessionModel();
                    userSessionModel.username = result.Username;
                    userSessionModel.roleid = obj.ListRole;
                    userSessionModel.fullname = result.FullName;
                    HttpContext.Session.SetString(Helpers.SessionKeyUser.Key, JsonConvert.SerializeObject(userSessionModel));
                    HttpContext.Session.SetInt32(Helpers.SessionKeyUser.KeyOfInputPass, 0);
                }
            }
            return Z_Result.SetResult(errMsg);
        }
        [AllowAnonymous]
        public IActionResult SignUp(string fullname, string email, string username, string password, string password2, string listRole)
        {
            List<MRole> lstRole = new List<MRole>();
            string errMsg;
            MUser objMuser = new MUser()
            {
                FullName = fullname,
                Email = email,
                Username = username,
                Password = password,
                IsActive = true,
                IsLocked = false
            };
            errMsg = ValidateUserData(objMuser, password, listRole, ref lstRole);
            if (errMsg == "")
            {
                MUserVM obju = new MUserVM();
                obju.ListRole = new List<TuserRole>();
                foreach (var role in lstRole)
                {
                    obju.ListRole.Add(new TuserRole()
                    {
                        Username = username,
                        RoleID = role.RoleID
                    });
                }
                obju.objUser = objMuser;
                errMsg = acc.Register(obju, password2);
                return Z_Result.SetResult(errMsg);
            }
            return Z_Result.SetResult(errMsg);
        }
        public IActionResult SubmitDelete(string password)
        {
            MUser objmuser = new MUser();
            string objIdentity = HttpContext.Session.GetString(Helpers.SessionKeyUser.Key);
            if (!string.IsNullOrEmpty(objIdentity))
                objmuser.Username = JsonConvert.DeserializeObject<UserSessionModel>(objIdentity).username;

            return Z_Result.SetResult(acc.DeactiveAccount(objmuser.Username, password));
        }
        public IActionResult SubmitUpdateUser(string fullname, string email, string username, string password, string password2, string listRole)
        {
            List<MRole> lstRole = new List<MRole>();
            string errMsg;
            MUser objMuser = new MUser()
            {
                FullName = fullname,
                Email = email,
                Username = username,
                Password = password,
                IsActive = true,
                IsLocked = false
            };
            errMsg = ValidateUserData(objMuser, password, listRole, ref lstRole);
            if (errMsg == "")
            {
                MUserVM obju = new MUserVM();
                obju.ListRole = new List<TuserRole>();
                foreach (var role in lstRole)
                {
                    obju.ListRole.Add(new TuserRole()
                    {
                        Username = username,
                        RoleID = role.RoleID
                    });
                }
                obju.objUser = objMuser;
                errMsg = acc.UpdateUserInfo(obju, password2);
                return Z_Result.SetResult(errMsg);
            }
            return Z_Result.SetResult(errMsg);
        }
        public IActionResult SubmitRequestResetPassword(string email)
        {            
            return Z_Result.SetResult(acc.RequestForgotPassword(email));
        }
        public IActionResult SubmitNewPassword(string newPass, string newPass2, string Token)
        {            
            return Z_Result.SetResult(acc.ResetPassword(newPass, newPass2, Token));
        }
        [AllowAnonymous]
        public IActionResult ReadListUser()
        {
            string mssg = "";
            List<MUserVM> ret =  acc.ReadListUser(ref mssg);
            return Z_Result.SetResult(mssg,ret);
        }
        private string ValidateUserData(MUser objMuser,string password, string _lstRole,ref List<MRole> lstRole)
        {
            List<string> errMessage=new List<string>();
            if (string.IsNullOrEmpty(objMuser.FullName))
                errMessage.Add("Name can't be empty");
            if (string.IsNullOrEmpty(objMuser.Username))
                errMessage.Add("Username can't be empty");
            if (string.IsNullOrEmpty(objMuser.Email))
                errMessage.Add("Email can't be empty");
            if (string.IsNullOrEmpty(password))
                errMessage.Add("Password can't be empty");
            try
            {
                lstRole = JsonConvert.DeserializeObject<List<MRole>>(_lstRole);
            }
            catch {
                //TODO LOG
                errMessage.Add("List Role can't be empty");
            }

            return string.Join(" \n ", errMessage);
        }
        #endregion

        [AllowAnonymous]
        //[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client, NoStore = true)]
        public IActionResult Login()
        {
            //TODO if available return info 
            //var data = HttpContext.Session.GetString("name");
            return View();
        }
        [AllowAnonymous]
        public IActionResult Register()
        {
           return View();
        }
        public IActionResult UpdateUser()
        {            
            return View();
        }
        public IActionResult DeleteAccount()
        {
            return View();
        }
        public IActionResult RequestResetPassword()
        {
            return View();
        }public IActionResult ResetPassword()
        {
            return View();
        }
        }
}
