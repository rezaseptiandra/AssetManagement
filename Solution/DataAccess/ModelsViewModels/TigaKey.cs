using System;
using System.Collections.Generic;
using Dapper;
using System.Linq;
using DataAccess.ModelsViewModels;
using DataAccess.Interface;
using DataAccess.Repository;

namespace DataAccess.ModelsViewModels
{
    [Table("TigaKey")]
    public class TigaKey
    {
        [Key]  
        [Required]
        public int ID1 { get; set; }
        [Key]
        [Required]
        public int ID2 { get; set; }
        [Key]
        [Required]
        public int ID3 { get; set; }
        public string Deskripsi { get; set; }
        public DateTime Dates { get; set; }
        public bool IsActive { get; set; }
        public double Gaji { get; set; }
        private static string SP_SELECT { get => "[TigaKey_SP_SELECT]"; }

    }

    [Table("TigaKey")]
    public class TigaKeyVM
    {       

        #region Main View
        [Key]
        public int ID1 { get; set; }
        [Key]
        public int ID2 { get; set; }
        [Key]
        public int ID3 { get; set; }
        public string Deskripsi { get; set; }
        public DateTime Dates { get; set; }
        public bool IsActive { get; set; }
        public double Gaji { get; set; }
       // private static string SP_SELECT { get => "[TigaKey_SP_SELECT]"; }
        #endregion

    }
    public class TigaKeyRPO : FullDataAccess<TigaKey>
    {
        public TigaKeyRPO(IMapper _mpp)
        {
            base.Initialize(_mpp);
        }
    }


}


    
