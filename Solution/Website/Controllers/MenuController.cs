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
    public class MenuController : Controller
    {
        private MenuManager mmgr;
        private readonly Ilog _logger;
        public MenuController(IMapper imap_, Ilog logger)
        {
            _logger = logger;
            mmgr = new MenuManager(imap_);
        }

        #region Services  
        public IActionResult SubmitAdd(string obj)
        {
            MMenu obj_ = new MMenu();
            string errMsg = Validate(obj, ref obj_);
            if (errMsg == "")
            {
                obj_.ID = obj_.ID.ToUpper();
                bool isDuplicate = mmgr.CheckIsExist(obj_.ID, ref errMsg);
                if (errMsg == "")
                {
                    errMsg = isDuplicate ? "Data Already Exist" : "";
                }
                else
                {
                    _logger.ERROR(errMsg);
                    errMsg = "Internal Server Error";
                }
            }

            if (errMsg == "")
            {
                errMsg = mmgr.Add(obj_);
                if (!string.IsNullOrEmpty(errMsg))
                {
                    _logger.ERROR(errMsg);
                    errMsg = "Internal Server Error";
                }
            }

            return Z_Result.SetResult(errMsg);
        }
        public IActionResult SubmitDelete(string id)
        {            
            string errMsg = string.IsNullOrEmpty(id)? "ID can't be empty" : "";
            if (errMsg == "")
            {
                id = id.ToUpper();
                bool isDuplicate = mmgr.CheckIsExist(id, ref errMsg);
                if (errMsg == "")
                {
                    errMsg = !isDuplicate ? "ID is not exist" : "";
                }
                else
                {
                    _logger.ERROR(errMsg);
                    errMsg = "Internal Server Error";
                }
            }
            if (errMsg == "")
            {
                errMsg = mmgr.Delete(id);
                if (!string.IsNullOrEmpty(errMsg))
                {
                    _logger.ERROR(errMsg);
                    errMsg = "Internal Server Error";
                }
            }
            return Z_Result.SetResult(errMsg);
        }
        public IActionResult SubmitUpdate(string obj)
        {
            MMenu obj_ = new MMenu();
            string errMsg = Validate(obj, ref obj_);
            if (errMsg == "")
            {
                bool isExist = mmgr.CheckIsExist(obj_.ID, ref errMsg);
                if (errMsg == "")
                {
                    errMsg = !isExist ? "Data does not exist" : "";
                }
                else
                {
                    _logger.ERROR(errMsg);
                    errMsg = "Internal Server Error";
                }
            }
            if (errMsg == "")
            {
                errMsg = mmgr.Update(obj_);
                if (!string.IsNullOrEmpty(errMsg))
                {
                    _logger.ERROR(errMsg);
                    errMsg = "Internal Server Error";
                }
            }

            return Z_Result.SetResult(errMsg);
        }
        public IActionResult GetList()
        {
            string mssg = "";
            List<MMenu> ret = mmgr.ReadList(ref mssg);
            return Z_Result.SetResult(mssg, ret);
        }
        private string Validate(string obj_, ref MMenu obj)
        {
            List<string> errMessage = new List<string>();
            try
            {
                obj = JsonConvert.DeserializeObject<MMenu>(obj_);
            }
            catch(Exception e)
            {
                _logger.ERROR(e.Message);
                errMessage.Add("Data can't be empty");
            }

            if (string.IsNullOrEmpty(obj.ID))
                errMessage.Add("ID can't be empty");
            if (string.IsNullOrEmpty(obj.ParentID))
                errMessage.Add("ParentID can't be empty");
            if (string.IsNullOrEmpty(obj.MenuName))
                errMessage.Add("MenuName can't be empty");
            if (string.IsNullOrEmpty(obj.Path))
                errMessage.Add("Path can't be empty");

            if (errMessage.Count == 0)
                obj.ID = obj.ID.ToUpper();

            return string.Join(" \n ", errMessage);
        }
        #endregion

        public IActionResult Add()
        {
            return View();
        }
        public IActionResult Delete()
        {
            return View();
        }
        public IActionResult Update()
        {
            return View();
        }
    }
}
