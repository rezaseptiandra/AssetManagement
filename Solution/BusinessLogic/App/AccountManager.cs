using System;
using System.Collections.Generic;
using System.Linq;
using DataAccess.ModelsViewModels;
using DataAccess.Repository;
using DataAccess.Interface;
using DataAccess;
using Newtonsoft.Json;

namespace BusinessLogic.App
{
    public class ResetPassParam
    {
        public string Email { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    public class AccountManager
    {
        private readonly IMapper imap_;
        private ExecResult exec;
        public AccountManager(IMapper imap)
        {
            this.imap_ = imap;
            this.exec = new ExecResult();
        }
        public MUserVM Login(string username, string password, out string message)
        {
            string encodedPassFromDB;
            bool matchPass = false;
            MUser objRetUser = new MUser();
            MUserVM objUser = new MUserVM();
            MUserRPO muserRPO = new MUserRPO(imap_);
            Conditions cnd = new Conditions();
            cnd.AddFilter(nameof(MUser.Username), Operator.Equals(username));
            cnd.AddFilter(nameof(MUser.IsActive), Operator.Equals(1));
            cnd.AddFilter(nameof(MUser.IsLocked), Operator.Equals(0));
            muserRPO.Conditions(cnd);
            //muserRPO.Where(nameof(MUser.IsActive)).Equals(1);
            //muserRPO.Where(nameof(MUser.IsLocked)).Equals(0);
            if (muserRPO.ReadOne(ref exec) && muserRPO.Result.AffectedRow>0)
            {
                encodedPassFromDB = muserRPO.Result.Row.Password;
                matchPass = Helpers.Crypto.ValidateKey(password, encodedPassFromDB);
                objRetUser = matchPass ? muserRPO.Result.Row : null;
                if (matchPass)
                {
                    objUser.objUser = objRetUser;
                    objUser.ListRole = new List<TuserRole>();
                    TUserRoleRPO objUrole = new TUserRoleRPO(imap_);
                    objUrole.Conditions(nameof(TuserRole.Username), Operator.Equals(objRetUser.Username));
                    //objUrole.Where(nameof(TUserRole.Username)).Equals(objRetUser.Username);
                    if (objUrole.ReadList(ref exec))
                    {
                        objUser.ListRole = objUrole.Result.Collection;
                    }
                    message = exec.Message;
                }
                else
                {
                    message = "Incorrect Username or Password";
                }
            }
            else
                message = exec.Message;

            return objUser;
            
        }
        public List<MUserVM> ReadListUser(ref string message)
        {
            List<MUserVM> lstRet = new List<MUserVM>();
            MUserRPO usrRPO = new MUserRPO(imap_);
            TUserRoleRPO tusrRoleRPO = new TUserRoleRPO(imap_);
            usrRPO.BeginTrans();
            if (usrRPO.ReadList(ref exec))
            {
                tusrRoleRPO.SetObjConn(usrRPO.ObjConn);
                foreach (var _objUser in usrRPO.Result.Collection)
                {
                    tusrRoleRPO.Conditions(nameof(TuserRole.Username), Operator.Equals(_objUser.Username));
                    //tusrRoleRPO.Where(nameof(TUserRole.Username)).Equals(_objUser.Username);
                    if (tusrRoleRPO.ReadList(ref exec))
                    {
                        _objUser.Password = "";
                        lstRet.Add(new MUserVM()
                        {
                            objUser = _objUser,
                            ListRole = tusrRoleRPO.Result.Collection
                        });
                    }
                    else
                        break;
                }
            }
            message = exec.Message;
            usrRPO.EndTrans(exec);

            return lstRet;
        }
        public List<TuserRole> ReadListUserRole(MUser objUser)
        {
            return new List<TuserRole>();
        }
        public string RequestForgotPassword(string email)
        {
            string token = "";
            //todo
            //check wheater email is registered?
            //send mail for link reset pass with token POST to be validate
            bool IsValidMail = true;
            
            if (IsValidMail)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("Email", email);
                dic.Add("CreatedDate", DateTime.Now.ToString());
                string json = JsonConvert.SerializeObject(dic);  
                token = Helpers.Crypto.EncryptStringAES(json);
            }
            return token;
        }
        public string ResetPassword(string newPass, string newPassAuth, string token)
        {        
            MUserVM objMuser = new MUserVM();            
            if (string.IsNullOrEmpty(newPass) || string.IsNullOrEmpty(newPassAuth))
            {
                return "new password or old password can't be empty.";
            }
            else
            {               
                bool isValidRequest = false;
                string encryptedToken = Helpers.Crypto.DecryptStringAES(token);

                try
                {
                    ResetPassParam dic = JsonConvert.DeserializeObject<ResetPassParam>(encryptedToken);
                    objMuser.objUser.Password = newPass;
                    objMuser.objUser.Username = dic.Email;
                    isValidRequest = DateTime.Now <= dic.CreatedDate.AddMinutes(5);                   
                }
                catch
                {
                    //TODO Log error
                    return "invalid Token Request";
                }

                if (isValidRequest)
                    return AddOrUpdateUser(true, objMuser);
                else
                    return "invalid Token Request";
            }            
        }
        public string Register(MUserVM objMuser, string ConfirmationPassword)
        {
            return AddOrUpdateUser(false,objMuser, ConfirmationPassword);
        }
        public string DeactiveAccount(string username, string password)
        {
            MUserRPO muserRPO = new MUserRPO(imap_);
            muserRPO.Conditions(nameof(MUser.Username), Operator.Equals(username));
            if (muserRPO.ReadOne(ref exec) && muserRPO.Result.AffectedRow>0)
            {
                string encryptedPass = muserRPO.Result.Row.Password;
                if (Helpers.Crypto.ValidateKey(password, encryptedPass))
                {
                    muserRPO.Result.Row.IsActive = false;
                    if (muserRPO.Update(muserRPO.Result.Row, ref exec))
                        return "";
                    else
                        return exec.Message;
                }                   
                else
                    return "Incorrect Username or Password";
            }
            else
            {
                return exec.Message;
            }

        }
        public string LockAccount(string username, string password)
        {
            MUserRPO muserRPO = new MUserRPO(imap_);
            muserRPO.Conditions(nameof(MUser.Username), Operator.Equals(username));
            if (muserRPO.ReadOne(ref exec) && muserRPO.Result.AffectedRow>0)
            {
                string encryptedPass = muserRPO.Result.Row.Password;
                if (Helpers.Crypto.ValidateKey(password, encryptedPass))
                {
                    muserRPO.Result.Row.IsLocked = true;
                    if (muserRPO.Update(muserRPO.Result.Row, ref exec))
                        return "";
                    else
                        return exec.Message;
                }                   
                else
                    return "Incorrect Username or Password";
            }
            else
            {
                return exec.Message;
            }

        }
        public string UpdateUserInfo(MUserVM objMuser, string ConfirmationPassword)
        {
            //todo check user session 
            return AddOrUpdateUser(true, objMuser, ConfirmationPassword);
        }        
        private string AddOrUpdateUser(bool isUpdate, MUserVM objMuservm, string ConfirmationPassword="")
        {
            MUserRPO muserRPO = new MUserRPO(imap_);
            MUser objMuser = objMuservm.objUser;
            string message = "";
            bool validPass =
               objMuser.Password.Any(c => char.IsLetter(c)) &&
               objMuser.Password.Any(c => char.IsDigit(c));

            message = validPass ? "" : "Password must contain at least one letter and one numeric digit";
            validPass = objMuser.Password == ConfirmationPassword;
            message = validPass ? "" : "Password didn't match";

            if (validPass)
            {
                objMuser.Password = Helpers.Crypto.EncryptPassword(objMuser.Password);
                if (objMuser.Password == "")
                {
                    //todo log
                    return "Error Encrypt";
                }
                muserRPO.BeginTrans();
                if (isUpdate)
                {
                    muserRPO.Conditions(nameof(objMuser.IsActive), Operator.Equals("true"));
                    muserRPO.Update(objMuser, ref exec);
                    AddUpdateUserRole(isUpdate, objMuservm, muserRPO.ObjConn, ref exec);
                }
                else
                {
                    muserRPO.Conditions(nameof(objMuser.Username), Operator.Equals(objMuser.Username));
                    if (muserRPO.ReadList(ref exec))
                    {
                        if (muserRPO.Result.AffectedRow > 0)
                            message = "username already Exist!";
                        else
                        {
                            muserRPO.Insert(objMuser, ref exec);
                            AddUpdateUserRole(isUpdate, objMuservm, muserRPO.ObjConn, ref exec);
                        }
                    }
                }
                message = exec.Message;
                muserRPO.EndTrans(exec);               
            }           
            return message;
        }
        private void AddUpdateUserRole(bool isUpdate, MUserVM objMuser,ObjectConnection objc, ref ExecResult exec)
        {
            TUserRoleRPO trpo = new TUserRoleRPO(imap_);
            trpo.SetObjConn(objc);
            if (isUpdate)
            {
                trpo.Conditions(nameof(TuserRole.Username), Operator.Equals(objMuser.objUser.Username));
                trpo.Delete(ref exec);
                //trpo.Where(nameof(TUserRole.Username)).Equals(objMuser.objUser.Username);
                //trpo.Delete(null,ref exec);
            }
            foreach (var obj in objMuser.ListRole)
            {
                if (!exec.Success)
                    break;

                trpo.Insert(obj, ref exec);
            }
        }
    }
}
