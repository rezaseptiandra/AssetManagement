using System;
using System.Collections.Generic;
using Dapper;
using System.Linq;
using DataAccess.Repository;
using DataAccess.Interface;

namespace DataAccess.ModelsViewModels
{
    [Table("TControllerRoleAccess")]
    public class TControllerRoleAccess
    {
        [Key]
        [Required]        
        public string ControllerName { get; set; }
        public string RoleID { get; set; }
        public bool AllowCreate { get; set; }
        public bool AllowEdit { get; set; }
        public bool AllowDelete { get; set; }
        public bool AllowView { get; set; }

    }

    public class TControllerRoleAccessVM
    {
        public string ControllerName { get; set; }
        public string RoleID { get; set; }
        public bool AllowCreate { get; set; }
        public bool AllowEdit { get; set; }
        public bool AllowDelete { get; set; }
        public bool AllowView { get; set; }
        #region Main View
        //[IgnoreInsert]
        //[IgnoreUpdate]
        //public string NewField { get; set; }
        #endregion

    }

    public class TControllerRoleAccessCustomVM
    {
        public static string SPName = "";
        [IgnoreInsert]
        [IgnoreUpdate]
        public string IsAllowed { get; set; }
    }

    public class TControllerRoleAccessRPO : FullDataAccess<TControllerRoleAccess>
    {
        public TControllerRoleAccessRPO(IMapper _mpp)
        {
            base.Initialize(_mpp);
        }
    }
}
