using DataAccess.ModelsViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Website.Models
{
    public class ObjectReturn
    {
        public string jsonresult { get; set; }
        public string errmessage { get; set; }
    }

    public static class Z_Result
    {
        public static JsonResult SetResult(string msgStatus = "",object jsonData = null )
        {
            return new JsonResult(new {status=msgStatus, data = jsonData });
        }
    }
}