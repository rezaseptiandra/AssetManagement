using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Website.Models;
using BusinessLogic.Module;
using DataAccess.Interface;
using DataAccess.ModelsViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Logger;
using Microsoft.AspNetCore.Diagnostics;
using DataAccess;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Website.Helpers;

namespace Website.Controllers
{
    [AllowAnonymous]
    public class ABPController : Controller
    {
        private readonly IMapper _imap;
        private readonly Ilog _logger;
        private List<string> lstMessage;
        MasterABP _abp;

        public ABPController(IMapper imap, Ilog logger)
        {
            _logger = logger;
            _imap = imap;
            lstMessage = new List<string>();
            _abp = new MasterABP(_imap);
        }
        public IActionResult Index()
        {           
            return View();
        }
        public IActionResult GetList(DataTableAjaxPostModel dtpm)
        {
            string mssg = "";
            int totrow;
            MasterABP tm = new MasterABP(_imap);
            GridModel gmd = FilterOption.BindToGridModel(dtpm, typeof(MABPVM));
            List<MABPVM> ret = tm.ReadList(gmd, out totrow,ref mssg);
            return new ContentResult
            {
                ContentType = "application/json",
                Content = JsonConvert.SerializeObject(new { draw = dtpm.draw, recordsFiltered = totrow, recordsTotal = totrow, data = ret }, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() })
            };
        }
        public ActionResult Save(string Action, string selectedData)
        {
            string errMsg = "";
            string id = "";
            MABP objSelectedData;
            if (Action == "New")
            {
                objSelectedData = JsonConvert.DeserializeObject<MABP>(selectedData);
                id = objSelectedData.ABPID;
                errMsg = _abp.Add(objSelectedData);
            }
            else if (Action == "Update")
            {
                objSelectedData = JsonConvert.DeserializeObject<MABP>(selectedData);
                id = objSelectedData.ABPID;
                errMsg = _abp.Update(objSelectedData);
            }

            return Json(new { abpid = id, action = Action, errMessage = errMsg });
        }
        public ActionResult SubmitDelete(string selectedData)
        {
            List<object> param = new List<object>() ;
            foreach (MABP obj in JsonConvert.DeserializeObject<List<MABP>>(selectedData))
            {
                param.Add(obj.ABPID);
            }
            return Json(new { error_message = _abp.Delete(param) });            
            
        }
        public ActionResult Detail(string Action, string selectedData)
        {
            string message = "";
            string key1 = JsonConvert.DeserializeObject<Dictionary<string, object>>(selectedData)["abpid"].ToString();

            return new ContentResult
            {
                ContentType = "application/json",
                Content = JsonConvert.SerializeObject(_abp.ReadDetail(key1, ref message))
            };
        }
        
    }
}
