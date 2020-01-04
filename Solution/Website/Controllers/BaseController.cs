using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Website.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Website.Controllers
{
    public class BaseController : Controller
    {
        protected ContentResult DataTableRecord<T>(DataTableAjaxPostModel dtpm, List<T> lstObj, List<string> ErrorMessage = null) where T:class {

            if (ErrorMessage != null) { 
                //'TODO'
            }
            int totrow = lstObj.Count;
            return new ContentResult
            {
                ContentType = "application/json",
                Content = JsonConvert.SerializeObject(new { draw = dtpm.draw, recordsFiltered = totrow, recordsTotal = totrow, data = lstObj }, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() })
            };
        }
    }
}