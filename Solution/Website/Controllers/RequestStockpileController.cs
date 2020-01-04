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
    public class RequestStockpileController : Controller
    {
        private readonly IMapper imap;
        private readonly Ilog _logger;
        private List<string> lstMessage;
        MasterABP abp;

        public RequestStockpileController(IMapper imap_, Ilog logger_)
        {
            this._logger = logger_;
            this.imap = imap_;
            lstMessage = new List<string>();
            abp = new MasterABP(imap);
        }
        public IActionResult Index()
        {           
            return View();
        }
        public IActionResult GetList(DataTableAjaxPostModel dtpm)
        {
            string mssg = "";
            int totrow;
            MasterABP tm = new MasterABP(imap);
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
                errMsg = abp.Add(objSelectedData);
            }
            else if (Action == "Update")
            {
                objSelectedData = JsonConvert.DeserializeObject<MABP>(selectedData);
                id = objSelectedData.ABPID;
                errMsg = abp.Update(objSelectedData);
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
            return Json(new { error_message = abp.Delete(param) });            
            
        }
        public ActionResult Detail(string Action, string selectedData)
        {
            string message = "";
            string key1 = JsonConvert.DeserializeObject<Dictionary<string, object>>(selectedData)["abpid"].ToString();

            return new ContentResult
            {
                ContentType = "application/json",
                Content = JsonConvert.SerializeObject(abp.ReadDetail(key1, ref message))
            };
        }
        
    }
}
