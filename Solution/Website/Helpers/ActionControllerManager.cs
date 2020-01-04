using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DataAccess.ModelsViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Website.Helpers
{
    
    public static class ActionControllerManager
    {
        public static List<TActionPermission> GetActionInfo
        {
            get
            {
                Assembly asm = Assembly.GetExecutingAssembly();
                List<TActionPermission> ret = new List<TActionPermission>();
                foreach (var actContrl in asm.GetTypes()
                    .Where(type => typeof(Controller).IsAssignableFrom(type))
                    .SelectMany(type => type.GetMethods())
                    .Where(method => method.IsPublic && !method.IsDefined(typeof(NonActionAttribute)) && method.ReturnType.Name == "IActionResult")
                )
                {
                     ret.Add(new TActionPermission() {ControllerName= actContrl.DeclaringType.Name , ActionName = actContrl.Name } );
                }
                return ret;
            }
        }
    }
    
}
