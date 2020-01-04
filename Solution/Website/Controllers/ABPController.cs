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
    public class ABPController : BaseController
    {
        private readonly IMapper IM;
        private readonly Ilog LOG;
        private List<string> ListMessage;
        private ExecResult ExecRes;

        public ABPController(IMapper imap, Ilog logger)
        {
            LOG = logger;
            IM = imap;
            ListMessage = new List<string>();
            ExecRes = new ExecResult();
        }
        public IActionResult Index()
        {           
            return View();
        }
        public IActionResult GetList(DataTableAjaxPostModel dtpm)
        {
            GridModel gmd = FilterOption.BindToGridModel(dtpm, typeof(MABPVM));
            List<MABPVM> ret = new List<MABPVM>();

            MABPVMDA AbpDA = new MABPVMDA(IM);
            if (AbpDA.ReadListPaged(gmd, ref ExecRes))
            {
                ret = AbpDA.Result.Collection;
            }
            else
                ListMessage.Add(AbpDA.Result.Message);

            return DataTableRecord(dtpm, ret);
        }
        //public ActionResult Save(string Action, string selectedData)
        //{
        //    string errMsg = "";
        //    string id = "";
        //    MABP objSelectedData;
        //    if (Action == "New")
        //    {
        //        objSelectedData = JsonConvert.DeserializeObject<MABP>(selectedData);
        //        id = objSelectedData.ABPID;
        //        errMsg = _abp.Add(objSelectedData);
        //    }
        //    else if (Action == "Update")
        //    {
        //        objSelectedData = JsonConvert.DeserializeObject<MABP>(selectedData);
        //        id = objSelectedData.ABPID;
        //        errMsg = _abp.Update(objSelectedData);
        //    }

        //    return Json(new { abpid = id, action = Action, errMessage = errMsg });
        //}
        //public ActionResult SubmitDelete(string selectedData)
        //{
        //    List<object> param = new List<object>() ;
        //    foreach (MABP obj in JsonConvert.DeserializeObject<List<MABP>>(selectedData))
        //    {
        //        param.Add(obj.ABPID);
        //    }
        //    return Json(new { error_message = _abp.Delete(param) });            
            
        //}
        //public ActionResult Detail(string Action, string selectedData)
        //{
        //    string message = "";
        //    string key1 = JsonConvert.DeserializeObject<Dictionary<string, object>>(selectedData)["abpid"].ToString();

        //    return new ContentResult
        //    {
        //        ContentType = "application/json",
        //        Content = JsonConvert.SerializeObject(_abp.ReadDetail(key1, ref message))
        //    };
        //}
        
    }
}
