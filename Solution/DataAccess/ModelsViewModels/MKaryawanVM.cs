using System;
using System.Collections.Generic;
using Dapper;
using System.Linq;
using DataAccess.Repository;
using DataAccess.Interface;

namespace DataAccess.ModelsViewModels
{    
    public class MKaryawanVM
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Office { get; set; }
        public string Position { get; set; }
        public decimal Salary { get; set; }
        public DateTime StartDate { get; set; }
        public string Active { get; set; }


        private static string SP_SELECT { get => "[MKaryawanVM_SP_SELECT]"; }
        //private static string SP_INSERT { get => "[MKaryawan_SP_INSERT]"; }
        private static string SP_DELETE { get => "[MKaryawan_SP_DELETE]"; }
    }
    public class MKaryawanVMRPO : ViewDataAccess<MKaryawanVM>
    {
        public MKaryawanVMRPO(IMapper _mpp)
        {
            Initialize(_mpp);
        }
    }


}
