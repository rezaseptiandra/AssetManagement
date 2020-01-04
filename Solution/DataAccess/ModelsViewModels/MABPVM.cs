using System;
using System.Collections.Generic;
using Dapper;
using System.Linq;
using DataAccess.Repository;
using DataAccess.Interface;

namespace DataAccess.ModelsViewModels
{
    [Table("MABP")]
    public class MABP : BaseModel
    {
        [Key]
        public string ABPID { get; set; }
        public string Deskripsi { get; set; }
        public DateTime PeriodeStart { get; set; }
        public DateTime PeriodeEnd { get; set; }
    }
    public class MABPVM : BaseModel
    {
        public string ABPID { get; set; }
        public string Deskripsi { get; set; }
        public DateTime PeriodeStart { get; set; }
        public DateTime PeriodeEnd { get; set; }

    }
    public class MABPDA : FullDataAccess<MABP>
    {
        public MABPDA(IMapper _mpp)
        {
            Initialize(_mpp);
            //Set_SP_Insert("MABP_SP_INSERT");
            //Set_SP_Update("MABP_SP_UPDATE");
            //Set_SP_Delete("MABP_SP_DELETE");
        }


    }
    public class MABPVMDA : ViewDataAccess<MABPVM>
    {
        public MABPVMDA(IMapper _mpp)
        {
            Initialize(_mpp);
            Set_SP_Select("[MABPVM_SP_SELECT]");
        }
    }


}
