using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ConditionFilters
    {
        private Type typeclass_ { get; set; }
        public Type typeclass { get => typeclass_; }
        private List<string> ListGroupBy_ { get; set; }
        private List<string> ListOrderBy_ { get; set; }
        private List<string> ListField_ { get; set; }
        private List<string> ListWhere_ { get; set; }
        private List<string> ListWhereOfSingleValueForAllColumns_ { get; set; }
        public List<string> ListWhereOfSingleValueForAllColumns { get => ListWhereOfSingleValueForAllColumns_; }

        public void SetGridFilter(GridModel gridModel, Type type)
        {
            PropertyInfo[] clfilt = type.GetProperties();
            List<string> rett = new List<string>();
            var colres = from grdModel in gridModel.ColumnsWithValueFiltered
                         join propCol in clfilt.Where(x => x.CustomAttributes.Where(n => n.AttributeType.Name.Contains("Ignore")).ToList().Count == 0)
                         on grdModel.Key.ToLower() equals propCol.Name.ToLower()
                         select new
                         {
                             grdModel.Value,
                             propCol.CustomAttributes,
                             propCol.PropertyType,
                             propCol.Name
                         };
            foreach (var coldata in colres)
            {
                string val = coldata.Value.ToString();
                if (coldata.PropertyType.Name.ToLower() == nameof(Boolean).ToLower())
                {
                    if ("yes".Contains(val.ToLower()))
                    {
                        val = "1";
                        ListWhere_.Add("(" + coldata.Name + " LIKE'%" + val + "%' )");
                    }
                    else if ("no".Contains(val.ToLower()))
                    {
                        val = "0";
                        ListWhere_.Add("(" + coldata.Name + " LIKE'%" + val + "%' )");
                    }
                }
                else
                    ListWhere_.Add("(" + coldata.Name + " LIKE'%" + val + "%' )");
            }

            var colress = from propCol in clfilt.Where(x => x.CustomAttributes.Where(n => n.AttributeType.Name.Contains("Ignore")).ToList().Count == 0)
                          join grdModel in gridModel.ColumnForSingleValueFiltered.ToList()
                          on propCol.Name.ToLower() equals grdModel.ToLower()
                          select new
                          {
                              propCol.CustomAttributes,
                              propCol.PropertyType,
                              propCol.Name
                          };

            foreach (var coldata in colress)
            {
                string val = gridModel.ValueFilteredForAllColumns;
                if (coldata.PropertyType.Name.ToLower() == nameof(Boolean).ToLower())
                {
                    if ("yes".Contains(val.ToLower()))
                    {
                        val = "1";
                        ListWhereOfSingleValueForAllColumns_.Add("(" + coldata.Name + " LIKE'%" + val + "%' )");
                    }
                    else if ("no".Contains(val.ToLower()))
                    {
                        val = "0";
                        ListWhereOfSingleValueForAllColumns_.Add("(" + coldata.Name + " LIKE'%" + val + "%' )");
                    }
                }
                else
                    ListWhereOfSingleValueForAllColumns_.Add("(" + coldata.Name + " LIKE'%" + val + "%' )");
            }

        }
        public string ResultFilter
        {
            get
            {
                string ret = "";
                if (ListWhere_.Count > 0)
                    ret = "WHERE " + string.Join(" AND ", ListWhere_);
                else
                    ret = "";

                if (ListWhereOfSingleValueForAllColumns_.Count > 0)
                    ret += ret == "" ? "WHERE " + string.Join(" OR ", ListWhereOfSingleValueForAllColumns_) : " AND (" + string.Join(" OR ", ListWhereOfSingleValueForAllColumns_) + ")";

                return ret;
            }
        }
        public string ResultSelect
        {
            get
            {
                if (ListField_.Count > 0)
                    return "SELECT " + string.Join(" , ", ListField_);
                else
                    return "SELECT * ";
            }
        }
        public string ResultGroupBy
        {
            get
            {
                if (ListGroupBy_.Count > 0)
                    return "GROUP BY " + string.Join(" , ", ListGroupBy_);
                else
                    return "";
            }
        }
        public string ResultOrderBy
        {
            get
            {
                if (ListOrderBy_.Count > 0)
                    return "ORDER BY " + string.Join(" , ", ListOrderBy_);
                else
                    return "";
            }
        }
        public string ResultOrderByWiithoutPrefix
        {
            get
            {
                if (ListOrderBy_.Count > 0)
                    return string.Join(" , ", ListOrderBy_);
                else
                    return "";
            }
        }
        public string ReturnValue { get; }
        private string ReturnValue_ { get; set; }
        private string Field { get; set; }
        public ConditionFilters(Type typeClass = null)
        {
            this.ListWhere_ = new List<string>();
            this.ListField_ = new List<string>();
            this.ListGroupBy_ = new List<string>();
            this.ListOrderBy_ = new List<string>();
            this.ReturnValue_ = "";
            this.typeclass_ = typeClass;
            this.ListWhereOfSingleValueForAllColumns_ = new List<string>();
        }
        public ConditionFilters(string Field)
        {
            this.ReturnValue_ = "";
        }

        public void AddFIeld(string Field)
        {
            this.Field = Field;
        }

        public void SelectField(string Field)
        {
            ListField_.Add(Field);
        }

        public void AddGroupBy(string GroupBy)
        {
            ListGroupBy_.Add(GroupBy);
        }

        public void AddOrderBy(string field, OrderType ordrtyp)
        {
            ListOrderBy_.Add(field + " " + (ordrtyp == OrderType.ASC ? "ASC" : "DESC"));
        }
        #region Operator
        public void Equals(string val_) { ListWhere_.Add("( " + this.Field + " ='" + val_ + "' )"); }
        public void Equals(int val_) { ListWhere_.Add("( " + this.Field + " ='" + val_ + "' )"); }
        public void NotEquals(string val_) { ListWhere_.Add("(" + this.Field + " <>'" + val_ + "' )"); }
        public void NotEquals(int val_) { ListWhere_.Add("(" + this.Field + " <>'" + val_ + "' )"); }
        public void In(List<string> val_) { ListWhere_.Add("(" + this.Field + " IN ('" + string.Join("','", val_) + "' ))"); }
        public void In(List<int> val_) { ListWhere_.Add("(" + this.Field + " IN ('" + string.Join("','", val_) + "' ))"); }
        public void NotIn(List<string> val_) { ListWhere_.Add("(" + this.Field + " NOT IN ('" + string.Join("','", val_) + "' ))"); }
        public void NotIn(List<int> val_) { ListWhere_.Add("(" + this.Field + " NOT IN ('" + string.Join("','", val_) + "' ))"); }
        public void Like(string val_) { ListWhere_.Add("(" + this.Field + " LIKE'%" + val_ + "%' )"); }
        public void Like(int val_) { ListWhere_.Add("(" + this.Field + " LIKE'%" + val_ + "%' )"); }
        public void NotLike(string val_) { ListWhere_.Add("(" + this.Field + " LIKE'%" + val_ + "%' )"); }
        public void NotLike(int val_) { ListWhere_.Add("(" + this.Field + " LIKE'%" + val_ + "%' )"); }
        public void GreatherThan(decimal val_) { ListWhere_.Add("(" + this.Field + " >" + val_ + " )"); }
        public void GreatherThan(int val_) { ListWhere_.Add("(" + this.Field + " >" + val_ + " )"); }
        public void GreatherThanEquals(decimal val_) { ListWhere_.Add("(" + this.Field + " >=" + val_ + " )"); }
        public void GreatherThanEquals(int val_) { ListWhere_.Add("(" + this.Field + " >=" + val_ + " )"); }
        public void LessThan(decimal val_) { ListWhere_.Add("(" + this.Field + " <" + val_ + " )"); }
        public void LessThan(int val_) { ListWhere_.Add("(" + this.Field + " <" + val_ + " )"); }
        public void LessThanEquals(int val_) { ListWhere_.Add("(" + this.Field + " <=" + val_ + " )"); }
        public void LessThanEquals(decimal val_) { ListWhere_.Add("(" + this.Field + " <=" + val_ + " )"); }
        public void IsNull() { ListWhere_.Add("(" + this.Field + " IS NULL)"); }
        public void IsNotNull() { ListWhere_.Add("(" + this.Field + " IS NOT NULL)"); }
        #endregion
    }

}
