using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DataAccess.App
{
    public class Grid
    {
        //public void SetGridFilter(GridModel gridModel, Type type, ref ConditionFilters cnd)
        //{

        //    PropertyInfo[] clfilt = type.GetProperties();
        //    List<string> rett = new List<string>();
        //    var colres = from grdModel in gridModel.ColumnsWithValueFiltered
        //                 join propCol in clfilt.Where(x => x.CustomAttributes.Where(n => n.AttributeType.Name.Contains("Ignore")).ToList().Count == 0)
        //                 on grdModel.Key.ToLower() equals propCol.Name.ToLower()
        //                 select new
        //                 {
        //                     grdModel.Value,
        //                     propCol.CustomAttributes,
        //                     propCol.PropertyType,
        //                     propCol.Name
        //                 };
        //    foreach (var coldata in colres)
        //    {
        //        string val = coldata.Value.ToString();
        //        if (coldata.PropertyType.Name.ToLower() == nameof(Boolean).ToLower())
        //        {
        //            if ("yes".Contains(val.ToLower()))
        //            {
        //                val = "1";
        //                cnd.AddFIeld(coldata.Name);
        //                cnd.Like(val);
        //            }
        //            else if ("no".Contains(val.ToLower()))
        //            {
        //                val = "0";
        //                cnd.AddFIeld(coldata.Name);
        //                cnd.Like(val);
        //            }
        //        }
        //        else
        //        {
        //            cnd.AddFIeld(coldata.Name);
        //            cnd.Like(val);
        //        }
        //            //ListWhere_.Add("(" + coldata.Name + " LIKE'%" + val + "%' )");
        //    }

        //    var colress = from propCol in clfilt.Where(x => x.CustomAttributes.Where(n => n.AttributeType.Name.Contains("Ignore")).ToList().Count == 0)
        //                  join grdModel in gridModel.ColumnForSingleValueFiltered.ToList()
        //                  on propCol.Name.ToLower() equals grdModel.ToLower()
        //                  select new
        //                  {
        //                      propCol.CustomAttributes,
        //                      propCol.PropertyType,
        //                      propCol.Name
        //                  };

        //    foreach (var coldata in colress)
        //    {
        //        string val = gridModel.ValueFilteredForAllColumns;
        //        if (coldata.PropertyType.Name.ToLower() == nameof(Boolean).ToLower())
        //        {
        //            if ("yes".Contains(val.ToLower()))
        //            {
        //                val = "1";
        //                ListWhereOfSingleValueForAllColumns_.Add("(" + coldata.Name + " LIKE'%" + val + "%' )");
        //            }
        //            else if ("no".Contains(val.ToLower()))
        //            {
        //                val = "0";
        //                ListWhereOfSingleValueForAllColumns_.Add("(" + coldata.Name + " LIKE'%" + val + "%' )");
        //            }
        //        }
        //        else
        //            ListWhereOfSingleValueForAllColumns_.Add("(" + coldata.Name + " LIKE'%" + val + "%' )");
        //    }

        //}
    }
}
