using DataAccess.ModelsViewModels;
using System;
using System.Collections.Generic;

namespace Website.Models
{
    public class UserSessionModel
    {
        public string username { get; set; }
        public string fullname { get; set; }
        public List<TuserRole> roleid { get; set; }
    }
}