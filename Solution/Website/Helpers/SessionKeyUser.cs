using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Website.Helpers
{
    public static class SessionKeyUser
    {
        public static string Key = "UserSessionModel";
        public static string KeyOfInputPass = "TotalInputTimes";
        public static string KeyOfLockedUser = "IsLockedUser";
    }
    
}
