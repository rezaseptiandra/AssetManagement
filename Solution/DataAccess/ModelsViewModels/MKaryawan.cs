using System;
using System.Collections.Generic;
using Dapper;
using System.Linq;
using DataAccess.Repository;
using DataAccess.Interface;

namespace DataAccess.ModelsViewModels
{
    [Table("MKaryawan")]
    public class MKaryawan
    {
        [Key]
        [ReadOnly(true)]
        //[Required] if identity key
        public int ID { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Office { get; set; }
        public string Position { get; set; }
        public decimal Salary { get; set; }
        public DateTime? StartDate { get; set; }
        public bool Active { get; set; }

        [ReadOnly(true)]
        public string _Active
        {
            get
            {
                return Active ? "Yes" : "No";
            }
        }

        //private static string SP_SELECT { get => "[MKaryawan_SP_SELECT]"; }
        //private static string SP_INSERT { get => "[MKaryawan_SP_INSERT]"; }
        //private string SP_UPDATE { get => ""; }
        private static string SP_DELETE { get => "[MKaryawan_SP_DELETE]"; }
        private static string SP_UPDATE { get => "[MKaryawan_SP_UPDATE]"; }

    }
    public class MKaryawanRPO : FullDataAccess<MKaryawan>
    {
        public MKaryawanRPO(IMapper _mpp)
        {
            base.Initialize(_mpp);
        }
    }
}
