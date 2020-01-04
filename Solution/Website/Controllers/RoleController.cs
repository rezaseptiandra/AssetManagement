using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Website.Models;
using BusinessLogic.Module;
using DataAccess.Interface;
using Microsoft.AspNetCore.Http;
using BusinessLogic.App;
using DataAccess.ModelsViewModels;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using Logger;
using Newtonsoft.Json.Serialization;
using System.Text;
using Website.Helpers;
using DataAccess;

namespace Website.Controllers
{
    public class RoleController : Controller
    {
        private RoleManager rlm;
        private readonly Ilog logger_;
        readonly IMapper imap___;
        public RoleController(IMapper imap_, Ilog logger)
        {
            logger_ = logger;
            imap___ = imap_;
            rlm = new RoleManager(imap_);
        }

        #region Services  
        public IActionResult SubmitAdd(string obj)
        {
            MRole obj_ = new MRole();
            string errMsg = Validate(obj, ref obj_);
            if (errMsg == "")
            {
                bool isExist = rlm.CheckIsExist(obj_.RoleID, ref errMsg);
                if (errMsg == "")
                {
                    errMsg = isExist ? "Data already exist" : "";
                }
                else
                {
                    logger_.ERROR(errMsg);
                    errMsg = "Internal Server Error";
                }
            }
            if (errMsg == "")
            {
                errMsg = rlm.Add(obj_);
                if (!string.IsNullOrEmpty(errMsg))
                {
                    logger_.ERROR(errMsg);
                    errMsg = "Internal Server Error";
                }
            }
            return Z_Result.SetResult(errMsg);
        }
        public IActionResult SubmitDelete(string id)
        {
            string errMsg = string.IsNullOrEmpty(id) ? "RoleID can't be empty" : "";
            if (errMsg == "")
            {
                id = id.ToUpper();
                bool isDuplicate = rlm.CheckIsExist(id, ref errMsg);
                if (errMsg == "")
                {
                    errMsg = !isDuplicate ? "Data does not exist" : "";
                }
                else
                {
                    logger_.ERROR(errMsg);
                    errMsg = "Internal Server Error";
                }
            }
            if (errMsg == "")
            {
                errMsg = rlm.Delete(id);
                if (!string.IsNullOrEmpty(errMsg))
                {
                    logger_.ERROR(errMsg);
                    errMsg = "Internal Server Error";
                }
            }

            return Z_Result.SetResult(errMsg);
        }
        public IActionResult SubmitUpdate(string obj)
        {
            MRole obj_ = new MRole();
            string errMsg = Validate(obj, ref obj_);
            if (errMsg == "")
            {
                bool isExist = rlm.CheckIsExist(obj_.RoleID, ref errMsg);
                if (errMsg == "")
                {
                    errMsg = !isExist ? "Data does not exist" : "";
                }
                else
                {
                    logger_.ERROR(errMsg);
                    errMsg = "Internal Server Error";
                }
            }
            if (errMsg == "")
            {
                errMsg = rlm.Update(obj_);
                if (!string.IsNullOrEmpty(errMsg))
                {
                    logger_.ERROR(errMsg);
                    errMsg = "Internal Server Error";
                }
            }
            return Z_Result.SetResult(errMsg);
        }
        public IActionResult GetList()
        {
            string mssg = "";
            List<MRole> ret = rlm.ReadList(ref mssg);
            return Z_Result.SetResult(mssg, ret);
        }
        private string Validate(string _obj, ref MRole obj)
        {
            List<string> errMessage = new List<string>();
            try
            {
                obj = JsonConvert.DeserializeObject<MRole>(_obj);
            }
            catch (Exception e)
            {
                logger_.ERROR(e.Message);
                errMessage.Add("Role can't be empty");
            }

            if (string.IsNullOrEmpty(obj.RoleID))
                errMessage.Add("RoleID can't be empty");
            if (string.IsNullOrEmpty(obj.Descriptions))
                errMessage.Add("Descriptions can't be empty");

            if (errMessage.Count == 0)
                obj.RoleID = obj.RoleID.ToUpper();

            return string.Join(" \n ", errMessage);
        }
        #endregion

        public IActionResult Add()
        {
            return View();
        }
        public IActionResult Delete()
        {
            return View();
        }
        public IActionResult Update()
        {
            return View();
        }
        public IActionResult Read()
        {
            return View();
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult loaddata(DataTableAjaxPostModel dtpm)
        {
            int totarec = 0;
            var users = new List<MKaryawanVM>();
            MKaryawanVMRPO krp = new MKaryawanVMRPO(imap___);
            ExecResult exec = new ExecResult();
            GridModel grdMdl = FilterOption.BindToGridModel(dtpm, typeof(MKaryawanVM));
            if (krp.ReadListPaged(grdMdl, out totarec, ref exec))
            {
                users = krp.Result.Collection;
            }

            //Conditions ncon = new Conditions();
            //ncon.AddSelect("ID");
            //ncon.AddSelect("Age");
            //ncon.AddSelect("Name");
            //ncon.AddFilter("Age", Operator.Equals(823458));
            ////ncon.AddGroupBy("Name");
            //ncon.AddOrderBy("ID");
            //ncon.SetOrderAscending();
            //if (krp.ReadOne(ref exec) && krp.Result.AffectedRow>0)
            //{
            //    //users = krp.Result.Collection;
            //}
            //krp.Conditions(ncon);

            ////krp.Conditions("Name", Operator.Like("a"));
           
            //if (krp.ReadOne(ref exec) && krp.Result.AffectedRow > 0)
            //{
            //    //users = krp.Result.Collection;
            //}
            return new ContentResult
            {
                ContentType = "application/json",
                Content = JsonConvert.SerializeObject(new { draw = dtpm.draw, recordsFiltered = totarec, recordsTotal = totarec, data = users }, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() })
            };
        }
        public IActionResult ti()
        {
            //var ObjTK = new TigaKey();
            //ObjTK.ID1 = 5;
            //ObjTK.ID2 = 2;
            //ObjTK.ID3 = 3;
            //ObjTK.Deskripsi = "";
            //ObjTK.Dates = DateTime.Now;
            //ObjTK.IsActive = true;
            //ObjTK.Gaji = 3453454325.8798;
            ////ObjKaryawan.ID = 23523;
            //TigaKeyRPO krp = new TigaKeyRPO(imap___);
            //ExecResult exec = new ExecResult();
            //if (krp.Insert(ObjTK, ref exec))
            //{

            //}

            var ObjTK = new MKaryawan();
            ObjTK.ID = 1;
            ObjTK.Name = "ASDFQW";
            ObjTK.Office = "LKHSDFL";
            ObjTK.Age = 978;
            ObjTK.Position = "kKJHKJH";
            ObjTK.Salary = 8979821397;
            ObjTK.StartDate = DateTime.Now;
            ObjTK.Active = true;
            MKaryawanRPO krp = new MKaryawanRPO(imap___);
            ExecResult exec = new ExecResult();
            if (krp.InsertGetKeys(ObjTK, ref exec))
            {

            }

            return new ContentResult
            {
                ContentType = "application/json",
                Content = exec.Message
            };
        }
        private List<string> BindColumnToSearch(DataTableAjaxPostModel dtpm)
        {
            return (from s in dtpm.columns.Where(x => x.searchable) select s.data).ToList();           
        }
    }
}
