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
    public class HomeController : Controller
    {
        private readonly IMapper imap;
        private readonly Ilog _logger;
        
        public HomeController(IMapper imap_, Ilog logger_)
        {
            this._logger = logger_;
            this.imap = imap_;
        }
        public IActionResult Index()
        {
            TestingModule tm = new TestingModule(imap);
            ExecResult exec = new ExecResult();
            List<JoinedUserRoleVM> returnresult =  tm.ReadListJoinedTable();
            return View(returnresult);
        }
        public IActionResult GetList(DataTableAjaxPostModel dtpm)
        {
            string mssg = "";
            int totrow = 0;
            TestingModule tm = new TestingModule(imap);
            GridModel gmd = FilterOption.BindToGridModel(dtpm, typeof(MKaryawan));
            List<MKaryawan> ret = tm.ReadList(gmd, out totrow,ref mssg);
            //return ZJsonResult.SetResult(mssg, ret);
            return new ContentResult
            {
                ContentType = "application/json",
                Content = JsonConvert.SerializeObject(new { draw = dtpm.draw, recordsFiltered = totrow, recordsTotal = totrow, data = ret }, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() })
            };
        }
        public IActionResult GetSession()
        {            
            var data = HttpContext.Session.GetString("name");
            return View("Index");
        }
        public IActionResult Privacy(string username, string password)
        {
            if (username != null && password != null && username.Equals("acc1") && password.Equals("123"))
            {
                HttpContext.Session.SetString("username", username);
                return View("Success");
            }
            else
            {
                ViewBag.error = "Invalid Account";
                return View("Index");
            }
            
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            if (exceptionFeature != null)
            {
                ZLogger objLog = _logger.GetLogInfo;
                objLog.Path = exceptionFeature.Path;
                _logger.SetLogInfo(objLog);
                _logger.ERROR(exceptionFeature.Error.Message);
            }
            //TODO create error view
            return Z_Result.SetResult("Internal Server Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
