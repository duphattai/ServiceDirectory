using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServiceDirectory.Models;
using PagedList;
namespace ServiceDirectory.Areas.NormalUser.Controllers
{
    public class MaintainDirectorateController : Controller
    {
        //
        // GET: /NormalUser/MaintainDirectorate/
        ServiceDirectoryEntities db = MaintainOrganisationController.database;

        static int OrganisationID;
        static int PageNumber;
        int PageSize = 5;
        static bool IncludeInActive = false;

        public ActionResult Index(string OrganisationID)
        {
            MaintainDirectorateController.OrganisationID = int.Parse(OrganisationID);
            return PartialView("List");
        }

        public ActionResult GetListDirectorates(int page = -1)
        {
            List<tblDirectorate> list = null;

            if (IncludeInActive == true)
                list = db.tblDirectorates.Where(t => t.OrgID == OrganisationID).ToList();
            else
                list = db.tblDirectorates.Where(t => t.OrgID == OrganisationID && t.IsActive == true).ToList();

            PageNumber = page != -1 ? page : PageNumber;
            return PartialView("Elements/ListItem", list.ToPagedList(PageNumber, PageSize));
        }

        public ActionResult Details(string DirectorateID)
        {
            // get record to get address from organisation
            tblOrganisation organisation = db.tblOrganisations.Where(t => t.OrgID == OrganisationID).SingleOrDefault();

            tblDirectorate model = null;
            if (!string.IsNullOrEmpty(DirectorateID)) // edit mode
            {
                int direcID = int.Parse(DirectorateID);
                model = db.tblDirectorates.Where(t => t.DirectorateID == direcID).SingleOrDefault();
            }
            else // add mode
            {
                model = new tblDirectorate();

                // set default value
                model.BusinessID = organisation.BusinessID;
                model.tblBusinessType = organisation.tblBusinessType;

                model.WebAddress = organisation.WebAddress;
            }

            // set copy address
            ViewBag.OrgAddressLine1 = organisation.AddressLine1;
            ViewBag.OrgAddressLine2 = organisation.AddressLine2;
            ViewBag.OrgAddressLine3 = organisation.AddressLine3;

            return PartialView("Elements/Details", model);
        }

        // if DepartmentID == "" add mode
        public ActionResult Add_ActionLink(string DirectorateID)
        {
            ViewBag.DirectorateID = DirectorateID;
            return PartialView("Add");
        }

        public ActionResult InsertUpdate_Directorate(tblDirectorate model)
        {
            model.IsActive = true;

            if (model.DirectorateID == 0) // add mode
            {
                // check depart ment name exists
                int exist = db.tblDirectorates.Where(t => t.DirectorateName == model.DirectorateName).ToList().Count;

                if (exist != 0) // not find out
                    return Content("Directorate name is existed. Please input another name");

                model.OrgID = OrganisationID;
                model.IsActive = true;

                db.tblDirectorates.Add(model);
            }
            else // edit mode
            {
                // check new department name is exist
                int exist = db.tblDirectorates.Where(t => t.DirectorateName == model.DirectorateName && t.DirectorateID != model.DirectorateID).ToList().Count;

                if (exist != 0) // cannot update
                    return Content("Directorate name is existed. Please input another name");

                tblDirectorate update = db.tblDirectorates.First(t => t.DirectorateID == model.DirectorateID);

                if (update != null)
                {
                    // update information
                    db.Entry(update).CurrentValues.SetValues(model);
                    db.Entry(update).Property(t => t.OrgID).IsModified = false;
                }
            }

            try
            {
                db.SaveChanges();
            }
            catch
            {
                return Content("Cannot save this directorate");
            }

            return Content("Save directorate successfully");
        }

        public ActionResult Edit_ActionLink(string DirectorateID)
        {
            ViewBag.DirectorateID = DirectorateID;
            return PartialView("Edit");
        }


        public string GetListDirectoratesFromCheckbox(bool IncludeInActive)
        {
            List<tblDirectorate> list = null;
            MaintainDirectorateController.IncludeInActive = IncludeInActive; // store tage of checkbox include in-active

            if (IncludeInActive == true)
                list = db.tblDirectorates.Where(t => t.OrgID == OrganisationID).ToList();
            else
                list = db.tblDirectorates.Where(t => t.OrgID == OrganisationID && t.IsActive == true).ToList();


            string html = MaintainTeamController.RenderPartialViewToString(this, "~/Areas/NormalUser/Views/MaintainDirectorate/Elements/ListItem.cshtml", list.ToPagedList(PageNumber, PageSize));
            
            // fix href missing name of ares
            html = html.Replace("/MaintainDirectorate/Edit_ActionLink", "/NormalUser/MaintainDirectorate/Edit_ActionLink");
            html = html.Replace("/MaintainDirectorate/GetListDirectorates", "/NormalUser/MaintainDirectorate/GetListDirectorates");
            return html;
        }

        public ActionResult MakeInActive(string DirectorateID)
        {
            int id = int.Parse(DirectorateID);
            tblDirectorate model = db.tblDirectorates.Where(t => t.DirectorateID == id).SingleOrDefault();
            if (model != null)
            {
                model.IsActive = false;
                db.SaveChanges();
            }

            return Content("Make this directorate in-active successfully");
        }

        public ActionResult Delete_ActionLink(string DirectorateID, int page)
        {
            int id = int.Parse(DirectorateID);

            tblDirectorate delete = db.tblDirectorates.Where(t => t.DirectorateID == id).SingleOrDefault();

            if (delete != null)
            {
                db.tblDirectorates.Remove(delete);
                db.SaveChanges();
            }

            return GetListDirectorates(page);
        }
    }
}
