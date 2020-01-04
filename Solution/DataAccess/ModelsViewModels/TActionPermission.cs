using System;
using System.Collections.Generic;
using Dapper;
using System.Linq;
using DataAccess.Repository;
using DataAccess.Interface;

namespace DataAccess.ModelsViewModels
{
    [Table("TActionPermission")]
    public class TActionPermission
    {
        [Key]
        [Required]        
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public bool RequiredCreate { get; set; }
        public bool RequiredEdit { get; set; }
        public bool RequiredDelete { get; set; }
        public bool RequiredView { get; set; }

    }
    public class TActionPermissionVM
    {
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public bool RequiredCreate { get; set; }
        public bool RequiredEdit { get; set; }
        public bool RequiredDelete { get; set; }
        public bool RequiredView { get; set; }
        public bool NotInDB { get; set; }
        public bool IsNotSet { get => RequiredCreate==RequiredDelete==RequiredEdit==RequiredView==false; }
        #region Main View
        //[IgnoreInsert]
        //[IgnoreUpdate]
        //public string NewField { get; set; }
        #endregion

    }
    public class ActPermissionVM
    {
       public string ControllerName { get; set; }
       public string ActionName { get; set; }
       public string IsAllowed { get; set; }
       public string Username { get; set; }
    }
    public class TActionPermissionRPO : FullDataAccess<TActionPermission>
    {
        public TActionPermissionRPO(IMapper _mpp)
        {
            base.Initialize(_mpp);
        }
    }
    public class ActPermissionVMRPO : FullDataAccess<ActPermissionVM>
    {
        public ActPermissionVMRPO(IMapper _mpp)
        {
            base.Initialize(_mpp);
            Set_SP_Select("[GetActionPermission_SP_SELECT]");
        }

    }
}
