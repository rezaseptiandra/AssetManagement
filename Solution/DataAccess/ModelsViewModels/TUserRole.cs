using System;
using System.Collections.Generic;
using Dapper;
using System.Linq;
using DataAccess.Repository;
using DataAccess.Interface;

namespace DataAccess.ModelsViewModels
{
    
    [Table("TUserRole")]
    public class TUserRole
    {
        [Key]
        [ReadOnly(true)]
        public int ID { get; set; }
        public string Username { get; set; }
        public string RoleID { get; set; }

    }
   
    public class TUserRoleCustomVM
    {
        public static string SPName = "TUserRole_SP_GetRoleAdmin";
        
        public string Username { get; set; }
       
        public string RoleID { get; set; }
    }

    public class TUserRoleRPO : FullRepository<TUserRole>
    {
        public TUserRoleRPO(IMapper _mpp)
        {
            base.Initialize(_mpp);
        }
    }
}
