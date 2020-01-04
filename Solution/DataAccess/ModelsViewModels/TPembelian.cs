using System;
using System.Collections.Generic;
using Dapper;
using System.Linq;
using DataAccess.Repository;
using DataAccess.Interface;

namespace DataAccess.ModelsViewModels
{
    [Table("TPembelian")]
    public class TPembelian
    {
        [Key]
        [ReadOnly(true)]
        public string TransactionID { get; set; }
        public int KaryawanID { get; set; }
        public decimal Pembelian { get; set; }
        private static string SP_INSERT { get => "[TPembelian_SP_INSERT]"; }
        private static string SP_UPDATE { get => "[TPembelian_SP_UPDATE]"; }

    }
    public class TPembelianRPO : FullDataAccess<TPembelian>
    {
        public TPembelianRPO(IMapper _mpp)
        {
            base.Initialize(_mpp);
        }
    }
}
