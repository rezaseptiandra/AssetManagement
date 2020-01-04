using System;
using System.Collections.Generic;
using Dapper;
using System.Linq;
using DataAccess.Interface;
using DataAccess.Repository;

namespace DataAccess.ModelsViewModels
{
    [Table("MRole")]
    public class MRole
    {
        [Key]
        [Required]        
        public string RoleID { get; set; }
        public string Descriptions { get; set; }
       

    }

    public class MRoleVM
    {
        public string RoleID { get; set; }
        public string Descriptions { get; set; }

        #region Main View
        //[IgnoreInsert]
        //[IgnoreUpdate]
        //public string NewField { get; set; }
        #endregion

    }

    public class MRoleCustomVM
    {
        public static string SPName = "selcustomsp";
        [IgnoreInsert]
        [IgnoreUpdate]
        public string Field1 { get; set; }
        [IgnoreInsert]
        [IgnoreUpdate]
        public string Field2 { get; set; }
    }

    public class MRoleRPO : FullDataAccess<MRole>
    {
        public MRoleRPO(IMapper _mpp)
        {
            base.Initialize(_mpp);
        }
    }

}
