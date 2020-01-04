using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
namespace DataAccess
{
    public static class Operator
    {
        public static string Like(object Value) { return " LIKE '%" + Value + "%'"; }
        public static string NotLike(object Value) { return " NOT LIKE '%%' " + Value; }
        public static string Equals(object Value) { return " = '" + Value + "' "; }
        public static string NotEqual(object Value) { return " <> " + Value; }
        public static string GreatherThan(object Value) { return " > " + Value; }
        public static string GreatherThanEqual(object Value) { return " >= " + Value; }
        public static string LessThan(object Value) { return " < " + Value; }
        public static string LessThanEqual(object Value) { return " <= " + Value; }
        public static string In(object Value) { return " IN (" + Value + ")"; }
        public static string NotIn(object Value) { return " NOT IN (" + Value + ")"; }
        public static string IsNull(object Value) { return " IS NULL " + Value; }
        public static string IsNotNull(object Value) { return " IS NOT NULL " + Value; }
    }
    public class Conditions
    {
        public Conditions(string Field, string OperatorAndValue)
        {
            ResultSelect_ = new List<string>();
            ResultOrderBy_ = new List<string>();
            ResultGroupBy_ = new List<string>();
            OrderArranged_ = "DESC";
            ResultFilter_ += "(" + Field + OperatorAndValue + ")";
        }
        public Conditions()
        {
            ResultFilter_ = "";
            ResultSelect_ = new List<string>();
            ResultOrderBy_ = new List<string>();
            ResultGroupBy_ = new List<string>();
            SP_SELECT = "";
            SP_INSERT = "";
            SP_UPDATE = "";
            SP_DELETE = "";
        }
        public string OperatorAndValue { get; set; }
        private string AllFilter { get; set; }
        private string OrderArranged_ { get; set; }
        public string GetAllFilterParam()
        {
            AllFilter = "";
            AllFilter += GetFilterParam();
            AllFilter += GetGroupByParam();
            AllFilter += GetOrderByParam();
            return AllFilter;
        }
        public string GetFilterParamWithoutOrder()
        {
            AllFilter = "";
            AllFilter += string.IsNullOrEmpty(ResultFilter_) ? "" : " WHERE (" + ResultFilter_ + ") ";
            AllFilter += GetGroupByParam();
            return AllFilter;
        }
        public string GetFilterParam()
        {
            return string.IsNullOrEmpty(ResultFilter_) ? "" : " WHERE (" + ResultFilter_ + ") ";
        }
        public string GetSelectParam()
        {
            return ResultSelect_.Count == 0 ? "SELECT * " : "SELECT " + string.Join(",", ResultSelect_) + " ";
        }
        public string GetTop1SelectParam()
        {
            return ResultSelect_.Count == 0 ? "SELECT TOP 1 * " : "SELECT TOP 1 " + string.Join(",", ResultSelect_) + " ";
        }
        public string GetOrderByParam()
        {
            return ResultOrderBy_.Count == 0 ? "" : " ORDER BY (" + string.Join(",", ResultOrderBy_) + ") " + OrderArranged_;
        }
        public string GetGroupByParam()
        {
            return ResultGroupBy_.Count == 0 ? "" : " GROUP BY (" + string.Join(",", ResultGroupBy_) + ") ";
        }

        public string SP_SELECT { get; set; }
        public string SP_INSERT { get; set; }
        public string SP_UPDATE { get; set; }
        public string SP_DELETE { get; set; }

        public string ResultFilter { get => ResultFilter_; }
        private string ResultFilter_ { get; set; }
        private List<string> ResultSelect_ { get; set; }
        private List<string> ResultGroupBy_ { get; set; }
        private List<string> ResultOrderBy_ { get; set; }
        public List<string> ResultSelect { get => ResultSelect_; }
        public List<string> ResultGroupBy { get => ResultGroupBy_; }
        public List<string> ResultOrderBy { get => ResultOrderBy_; }
        public string OrderArranged { get => OrderArranged_; }

        public void AddFilter(string Field, string OperatorAndValue)
        {
            string prefix = string.IsNullOrEmpty(ResultFilter_) ? "" : " AND ";
            ResultFilter_ += prefix + "(" + Field + OperatorAndValue + ")";
        }
        public void AddFilter_OR(string Field, string OperatorAndValue)
        {

            string prefix = string.IsNullOrEmpty(ResultFilter_) ? "" : " OR ";
            ResultFilter_ += prefix + "(" + Field + OperatorAndValue + ")";
        }
        public void AddFilter(Conditions fcond)
        {
            string prefix = string.IsNullOrEmpty(ResultFilter_) ? "" : " AND ";
            if (string.IsNullOrEmpty(fcond.ResultFilter_))
            {
                ResultFilter_ += "";
            }
            else
            {
                ResultFilter_ += prefix + "(" + fcond.ResultFilter_ + ")";
            }
        }
        public void AddFilter_OR(Conditions fcond)
        {
            string prefix = string.IsNullOrEmpty(ResultFilter_) ? "" : " OR ";
            if (string.IsNullOrEmpty(fcond.ResultFilter_))
            {
                ResultFilter_ += "";
            }
            else
            {
                ResultFilter_ += prefix + "(" + fcond.ResultFilter_ + ")";
            }
        }
        public void AddGroupBy(string Field)
        {
            ResultGroupBy_.Add(Field);
        }
        public void AddOrderBy(string Field)
        {
            ResultOrderBy_.Add(Field);
        }
        public void AddSelect(string Field)
        {
            ResultSelect_.Add(Field);
        }
        public void SetOrderDescending()
        {
            OrderArranged_ = "DESC";
        }
        public void SetOrderAscending()
        {
            OrderArranged_ = "ASC";
        }
        public void SetGridFilter(GridModel gridModel)
        {
            Conditions cnd;
            cnd = new Conditions();
            foreach (var prop in gridModel.ColumnsWithValueFiltered)
            {
                cnd.AddFilter(prop.Key, Operator.Like(prop.Value));
            }
            if (!string.IsNullOrEmpty(cnd.ResultFilter))
                AddFilter(cnd);

            cnd = new Conditions();

            if (!string.IsNullOrEmpty(gridModel.ValueFilteredForAllColumns))
            {
                foreach (var field in gridModel.ColumnForSingleValueFiltered)
                {
                    cnd.AddFilter_OR(field.ToLower(), Operator.Like(gridModel.ValueFilteredForAllColumns));
                }
                if (!string.IsNullOrEmpty(cnd.ResultFilter))
                    AddFilter(cnd);
            }

            if (!string.IsNullOrEmpty(gridModel.OrderByField))
            {
                AddOrderBy(gridModel.OrderByField);
                OrderArranged_ = gridModel.OrderByType;
            }
        }
    }

}