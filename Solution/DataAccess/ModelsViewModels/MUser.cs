using System;
using System.Collections.Generic;
using Dapper;
using System.Linq;
using DataAccess.Interface;
using DataAccess.Repository;

namespace DataAccess.ModelsViewModels
{
    [Table("MUser")]

    public class MUser : BaseModel
    {
        [Key]
        [Required]
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public bool IsLocked { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
    }
    public class MUserVM : BaseModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public bool IsLocked { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public List<TuserRole> ListRole { get; set; }
        public MUserVM()
        {
            objUser = new MUser();
        }
        public MUser objUser { get; set; }

    }
    public class MUserRPO : FullDataAccess<MUser>
    {
        public MUserRPO(IMapper _mpp)
        {
            Initialize(_mpp);
            //Set_SP_Insert("MABP_SP_INSERT");
            //Set_SP_Update("MABP_SP_UPDATE");
            //Set_SP_Delete("MABP_SP_DELETE");
        }


    }
    public class MUserVMRPO : ViewDataAccess<MUserVM>
    {
        public MUserVMRPO(IMapper _mpp)
        {
            Initialize(_mpp);
            Set_SP_Select("[MUserVM_SP_SELECT]");
        }
    }
}
