using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServiceDirectory.Models;
using PagedList;
using System.Data.Entity.Validation;
namespace ServiceDirectory.Areas.NormalUser.Controllers
{
    public class MaintainDepartmentController : Controller
    {
        ServiceDirectoryEntities db = new ServiceDirectoryEntities();

        //
        // GET: /NormalUser/MaintainDepartment/
        static string DirectorateID;
        static int PageNumber;
        int PageSize = 15;
        static bool IncludeInActive = false; // save tage of include checkbox cheked
        public ActionResult Index(string DirectorateID)
        {
            MaintainDepartmentController.DirectorateID = DirectorateID;
            return PartialView("List");
        }


        // if page = -1, load current pageNumber before, else load new pageNumber
        public ActionResult GetListDepartments(int page = -1)
        {
            
            int id = int.Parse(DirectorateID);
            List<tblDepartment> list = null;

            if (IncludeInActive == true)
                list = db.tblDepartments.Where(t => t.DirectorateID == id).ToList();
            else
                list = db.tblDepartments.Where(t => t.DirectorateID == id && t.IsActive == true).ToList();

            PageNumber = page != -1 ? page : PageNumber;
            return PartialView("Elements/ListItem", list.ToPagedList(PageNumber, PageSize));
             
        }


        public string GetListDepartmentsFromCheckbox(bool IncludeInActive)
        {
             
            int id = int.Parse(DirectorateID);
            List<tblDepartment> list = null;
            MaintainDepartmentController.IncludeInActive = IncludeInActive; // store tage of checkbox include in-active

            if (IncludeInActive == true)
                list = db.tblDepartments.Where(t => t.DirectorateID == id).ToList();
            else
                list = db.tblDepartments.Where(t => t.DirectorateID == id && t.IsActive == true).ToList();

            string html = MaintainTeamController.RenderPartialViewToString(this, "~/Areas/NormalUser/Views/MaintainDepartment/Elements/ListItem.cshtml", list.ToPagedList(PageNumber, PageSize));

            // fix href missing name or ares
            html = html.Replace("/MaintainDepartment/Edit_ActionLink", "/NormalUser/MaintainDepartment/Edit_ActionLink"); // fix error ajax not work (wrong href)
            html = html.Replace("/MaintainDepartment/GetListDepartments", "/NormalUser/MaintainDepartment/GetListDepartments"); // fix error paging not work
            return html;
            
        }

        // if DepartmentID == "" add mode
        public ActionResult Add_ActionLink(string DepartmentID = null)
        {
            ViewBag.DepartmentID = DepartmentID;
            return PartialView("Add");
        }


        public ActionResult Edit_ActionLink(string DepartmentID = null)
        {
            ViewBag.DepartmentID = DepartmentID;
            return PartialView("Edit");
        }

        public ActionResult Details(string DepartmentID = null)
        {
            
            int direcID = int.Parse(DirectorateID);

            tblDirectorate direc = db.tblDirectorates.Where(t => t.DirectorateID == direcID).SingleOrDefault();
            tblDepartment model = null;

            if(!string.IsNullOrEmpty(DepartmentID)) // edit mode
            {
                int departID = int.Parse(DepartmentID);
                model = db.tblDepartments.Where(t => t.DepartmentID == departID).SingleOrDefault();
            }
            else // add mode
            {
                model = new tblDepartment();

                // set default value
                model.BusinessID = direc.tblOrganisation.BusinessID;
                model.tblBusinessType = direc.tblOrganisation.tblBusinessType;

                model.WebAddress = direc.tblOrganisation.WebAddress;
            }

            // set copy address
            ViewBag.OrgAddressLine1 = direc.tblOrganisation.AddressLine1;
            ViewBag.OrgAddressLine2 = direc.tblOrganisation.AddressLine2;
            ViewBag.OrgAddressLine3 = direc.tblOrganisation.AddressLine3;

            ViewBag.DeparAddressLine1 = direc.AddressLine1;
            ViewBag.DeparAddressLine2 = direc.AddressLine2;
            ViewBag.DeparAddressLine3 = direc.AddressLine3;

            return PartialView("Elements/Details",model);
        
        }


        public ActionResult InsertUpdate_Department(tblDepartment model)
        {
            model.IsActive = true;

            if(model.DepartmentID == 0) // add mode
            {
                // check depart ment name exists
                int exist = db.tblDepartments.Where(t => t.DepartmentName == model.DepartmentName).ToList().Count;

                if (exist != 0) // not find out
                    return Content("Department name is existed. Please input another name");
                
                model.DirectorateID = int.Parse(DirectorateID);
                model.IsActive = true;

                db.tblDepartments.Add(model);
            }
            else // edit mode
            {
                // check new department name is exist
                int exist = db.tblDepartments.Where(t => t.DepartmentName == model.DepartmentName && t.DepartmentID != model.DepartmentID).ToList().Count;

                if (exist != 0) // cannot update
                    return Content("Department name is existed. Please input another name");

                tblDepartment update = db.tblDepartments.First(t => t.DepartmentID == model.DepartmentID);

                if(update != null)
                { 
                    // update information
                    db.Entry(update).CurrentValues.SetValues(model);
                    db.Entry(update).Property(t => t.DirectorateID).IsModified = false;
                }
            }

            try
            {
                db.SaveChanges();
            }
            catch 
            {
                return Content("Cannot save this department");
            }

            return Content("Save department successfully");
            
        }


        public ActionResult Delete_ActionLink(string DepartmentID, int page)
        {
            
            int id = int.Parse(DepartmentID);

            tblDepartment delete = db.tblDepartments.Where(t => t.DepartmentID == id).SingleOrDefault();
            string message = "";
            if (delete != null)
            {
                try
                {
                    db.tblDepartments.Remove(delete);
                    db.SaveChanges();
                }
                catch 
                {
                    message = "This department is using. Cannot delete it!";
                }
            }
            ViewBag.Message = message;
            return GetListDepartments(page);
            
        }
        public ActionResult MakeInActive(string DepartmentID)
        {
            
            int id = int.Parse(DepartmentID);
            tblDepartment model = db.tblDepartments.Where(t => t.DepartmentID == id).SingleOrDefault();
            if (model != null)
            {
                model.IsActive = false;
                db.SaveChanges();
            }

            return Content("Make this department in-active successfully");
            
        }
    }
}
