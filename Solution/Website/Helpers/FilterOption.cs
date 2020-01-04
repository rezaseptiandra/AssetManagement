using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess;
using DataAccess.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Website.Models;

namespace Website.Helpers
{
    public static class FilterOption
    {
        public static int GetPageNumberFromDataTablesModel(DataTableAjaxPostModel dtpm)
        {
            if (dtpm.length != 0)
            {
                return ((int)Math.Floor((decimal)dtpm.start++ / dtpm.length)) + 1;
            }
            else
                return 0;
        }
        public static GridModel BindToGridModel(DataTableAjaxPostModel dtrm, Type objType)
        {
            Dictionary<string, object> filteredColumn = new Dictionary<string, object>();
            List<string> SinglefilteredColumn = new List<string>();

            if (dtrm.columns != null)
            {
                foreach (var cl in dtrm.columns.Where(x => x.searchable && !string.IsNullOrEmpty(x.search.value)))
                    filteredColumn.Add(cl.data, cl.search.value);

                if (!string.IsNullOrEmpty(dtrm.search.value))
                {
                    foreach (var cl in dtrm.columns.Where(x => x.searchable))
                        SinglefilteredColumn.Add(cl.data);
                }
            }

            return new GridModel() {
                OrderByField = dtrm.columns[dtrm.order[0].column].data,
                OrderByType = dtrm.order[0].dir,
                TypeOfObject = objType,
                Draw = dtrm.draw,
                Page = GetPageNumberFromDataTablesModel(dtrm),
                RowsPerPage = dtrm.length,
                ColumnsWithValueFiltered = filteredColumn,
                ColumnForSingleValueFiltered = SinglefilteredColumn,
                ValueFilteredForAllColumns = SinglefilteredColumn.Count > 0 && !string.IsNullOrEmpty(dtrm.search.value) ? dtrm.search.value : ""
            };
        }
    }
    
}
