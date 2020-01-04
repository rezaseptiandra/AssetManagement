using System;
using System.Collections.Generic;
using Dapper;
using System.Linq;
using DataAccess.Repository;
using DataAccess.Interface;

namespace DataAccess.ModelsViewModels
{   

    public class ActPermissionVM
    {
        public static string SP_SELECT { get => "[TActionPermission_SP_GetRolePermission]"; }
        [IgnoreInsert]
        [IgnoreUpdate]
        public string ControllerName { get; set; }
        [IgnoreInsert]
        [IgnoreUpdate]
        public string ActionName { get; set; }
        [IgnoreInsert]
        [IgnoreUpdate]
        public string IsAllowed { get; set; }
        [IgnoreInsert]
        [IgnoreUpdate]
        public string Username { get; set; }
    }
    public class ActPermissionVMRPO : FullRepository<ActPermissionVM>
    {
        public ActPermissionVMRPO(IMapper _mpp)
        {
            base.Initialize(_mpp);
        }
    }

}
