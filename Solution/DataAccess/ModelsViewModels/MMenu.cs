using System;
using System.Collections.Generic;
using Dapper;
using System.Linq;
using DataAccess.Repository;
using DataAccess.Interface;

namespace DataAccess.ModelsViewModels
{
    [Table("MMenu")]
    public class MMenu
    {
        [Key]
        [Required]
        public string ID { get; set; }
        public string ParentID { get; set; }
        public string MenuName { get; set; }
        public string Path { get; set; }

    }

    public class MMenuVM
    {

        public string ID { get; set; }
        public string ParentID { get; set; }
        public string MenuName { get; set; }
        public string Path { get; set; }
        #region Main View
        //[IgnoreInsert]
        //[IgnoreUpdate]
        //public string NewField { get; set; }
        #endregion

    }

    public class MMenuCustomVM
    {
        public static string SPName = "selcustomsp";
        [IgnoreInsert]
        [IgnoreUpdate]
        public string Field1 { get; set; }
        [IgnoreInsert]
        [IgnoreUpdate]
        public string Field2 { get; set; }
    }

    public class MMenuRPO : FullDataAccess<MMenu>
    {
        public MMenuRPO(IMapper _mpp)
        {
            base.Initialize(_mpp);
        }
    }
}
