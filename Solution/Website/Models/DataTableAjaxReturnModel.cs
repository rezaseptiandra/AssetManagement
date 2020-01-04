using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Website.Models
{
    public class SampleTableReturn

    {
        public int id { get; set; }
        public string name { get; set; }
        public string position { get; set; }
        public string office { get; set; }
        public int age { get; set; }
        public DateTime startdate { get; set; }
        public decimal salary { get; set; }
    }
    public class DataTableAjaxReturnModel
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public object data { get; set; }
    }

   
}
