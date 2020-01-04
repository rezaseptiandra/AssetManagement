using System;
using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
using Dapper;
using System.Linq;
using DataAccess.Repository;
using DataAccess.Interface;

namespace DataAccess.ModelsViewModels
{

    [Table("DUser")]
    public partial class DUser
    {
        [Key]
        [Required]
        public int ID { get; set; }
        public int MUserID { get; set; }
        public string Name { get; set; }

        
    }
    public partial class DUser
    {
        [IgnoreInsert]
        [IgnoreUpdate]
        public string Ulolka { get; set; }
    }
    public class DUserRPO : FullDataAccess<DUser>
    {
        public DUserRPO(IMapper _mpp)
        {
            base.Initialize(_mpp);
        }
    }
}
