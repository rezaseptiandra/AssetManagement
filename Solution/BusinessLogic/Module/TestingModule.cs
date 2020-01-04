using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess;
using DataAccess.ModelsViewModels;
using DataAccess.Interface;
using DataAccess.Repository;

namespace BusinessLogic.Module
{
    public class TestingModule
    {
        private readonly IMapper imap_;
        private ExecResult exec_;
        public TestingModule(IMapper imap)
        {
            this.imap_ = imap;
            exec_ = new ExecResult();
        }

        public void GoTest(ref ExecResult exec)
        {
            //MKaryawan oKaryawan = new MKaryawan();
            //oKaryawan.ID = 1;
            //oKaryawan.Name = "ASDFQW";
            //oKaryawan.Office = "LKHSDFL";
            //oKaryawan.Age = 978;
            //oKaryawan.Position = "kKJHKJH";
            //oKaryawan.Salary = 8979821397;
            //oKaryawan.StartDate = DateTime.Now;
            //oKaryawan.Active = true;
            //ObjKaryawan.ID = 23523;
            MKaryawanRPO krp = new MKaryawanRPO(imap_);
            MKaryawanVMRPO krs = new MKaryawanVMRPO(imap_);
            //krp.Conditions("ID", Operator.Equals("3"));
            //krp.UpdateFiltered(new MKaryawan() { ID=23444, Name="asdasd" }, ref exec);
            //krp.Conditions("ID", Operator.Equals(95442));
            //krp.Delete(ref exec);
            //Conditions cnd = new Conditions();
            //cnd.AddFilter_OR("ID", Operator.Equals(13));
            //krp.Conditions(cnd);
            //krp.ReadOne(77, 2, ref exec);
            //krs.ReadOne(2, ref exec);
            //if (krp.InsertGetKeys(oKaryawan, ref exec))
            //{

            //}

            //var ObjTK = new TigaKey();
            //ObjTK.ID1 = 112;
            //ObjTK.ID2 = 23;
            //ObjTK.ID3 = 34;
            //ObjTK.Deskripsi = "";
            //ObjTK.Dates = DateTime.Now;
            //ObjTK.IsActive = true;
            //ObjTK.Gaji = 3453454325.8798;
            //TigaKeyRPO krpk = new TigaKeyRPO(imap);
            //krpk.Delete(32, 2, ref exec);
            /*
             KRP.AddFilter("",Operator.IN())
             KRP.AddFilter(field).LIKE();
             lstobjfilter = new lst();
             lstobjfilter.addFilter
             KRP.AddFilterOR(LIST OBJ FILTER)

             KRP.WHERE("",)
             */
            //if (krp.Insert(ObjTK, ref exec))
            //{

            //}
        }

        public List<MKaryawan> ReadList(GridModel gmd,out int totalRow, ref string message)
        {
            MKaryawanRPO RPO = new MKaryawanRPO(imap_);

            if (RPO.ReadListPaged(gmd, out totalRow, ref exec_))
            {
                return RPO.Result.Collection;
            }
            else
                message = "Internal Server Error";

            return null;
        }
        public List<JoinedUserRoleVM> ReadListJoinedTable()
        {
            JoinedUserRoleVMRPO RPO = new JoinedUserRoleVMRPO(imap_);

            if (RPO.ReadList(ref exec_))
            {
                return RPO.Result.Collection;
            }
            else
                return null;
        }

        public bool CheckIsExist(string id, ref string message)
        {
            MRoleRPO RPO = new MRoleRPO(imap_);
            //RPO.Where(nameof(MRole.RoleID)).Equals(id);
            RPO.Conditions(nameof(MRole.RoleID), Operator.Equals(id));
            if (RPO.ReadOne(ref exec_))
            {
                return RPO.Result.AffectedRow > 0;
            }
            else
                message = exec_.Message;

            return true;
        }
        public string Add(MRole obj)
        {
            MRoleRPO RPO = new MRoleRPO(imap_);
            RPO.Insert(obj, ref exec_);
            return exec_.Message;
        }
        public string AddBulk(List<MRole> lstObj)
        {
            MRoleRPO RPO = new MRoleRPO(imap_);
            RPO.BeginTrans();
            foreach (var obj in lstObj)
            {
                if (exec_.Success)
                    RPO.Insert(obj, ref exec_);
                else
                    break;
            }
            RPO.EndTrans(exec_);
            return exec_.Message;
        }
        public string Delete(string id)
        {
            MRoleRPO RPO = new MRoleRPO(imap_);
            RPO.Delete(id, ref exec_);
            return exec_.Message;
        }
        public string Update(MRole obj)
        {
            MRoleRPO RPO = new MRoleRPO(imap_);
            RPO.Update(obj, ref exec_);
            return exec_.Message;
        }

    }
}
