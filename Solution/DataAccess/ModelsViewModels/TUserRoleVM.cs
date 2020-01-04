using System;
using System.Collections.Generic;
using Dapper;
using System.Linq;
using DataAccess.Repository;
using DataAccess.Interface;

namespace DataAccess.ModelsViewModels
{
    [Table("TUserRole")]
    public class TuserRole : BaseModel
    {
        [Key]
        public int ID { get; set; }
        public string Username { get; set; }
        public string RoleID { get; set; }
    }
    public class TUserRoleVM : BaseModel
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string RoleID { get; set; }

    }
    public class TUserRoleRPO : FullDataAccess<TuserRole>
    {
        public TUserRoleRPO(IMapper _mpp)
        {
            Initialize(_mpp);
            //Set_SP_Insert("MABP_SP_INSERT");
            //Set_SP_Update("MABP_SP_UPDATE");
            //Set_SP_Delete("MABP_SP_DELETE");
        }


    }
    public class TUserRoleVMRPO : ViewDataAccess<TUserRoleVM>
    {
        public TUserRoleVMRPO(IMapper _mpp)
        {
            Initialize(_mpp);
            //Set_SP_Select("[TuserRoleVM_SP_SELECT]");
        }
    }
    public class CheckisAdminRPO : ViewDataAccess<TUserRoleVM>
    {
        public CheckisAdminRPO(IMapper _mpp)
        {
            Initialize(_mpp);
            Set_SP_Select("[GetAdmin_SP_SELECT]");
        }
    }
}
