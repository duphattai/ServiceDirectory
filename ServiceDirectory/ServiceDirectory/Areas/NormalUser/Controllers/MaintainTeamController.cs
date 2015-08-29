using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using ServiceDirectory.Models;
namespace ServiceDirectory.Areas.NormalUser.Controllers
{
    public class MaintainTeamController : Controller
    {
        ServiceDirectory.Models.ServiceDirectoryEntities db = MaintainOrganisationController.database;
        //
        // GET: /NormalUser/MaintainTeam/
        static string DepartmentID;

        
        public ActionResult Index(string DepartmentID)
        {
            MaintainTeamController.DepartmentID = DepartmentID;
            return PartialView("List");
        }

        public ActionResult GetListTeams(int? page)
        {
            Guid guid = new Guid(DepartmentID);

            var list = db.tblTeams.Where(t => t.DepartmentID == guid).OrderBy(g => g.TeamName).ToList();

            foreach(var item in list)
            {
                //item.tblContact = db.tblContacts.Where(t => t.ContactID == item.ContactID).SingleOrDefault();
                //item.tblAddress = db.tblAddresses.Where(t => t.AddressID == item.AddressID).SingleOrDefault();
                if (item.BusinessID == null)
                    item.tblBusinessType = new tblBusinessType();
                if (item.AddressID == null)
                {
                    item.tblAddress = new tblAddress();
                    item.tblAddress.tblTown = new tblTown();
                    item.tblAddress.tblTown.tblCounty = new tblCounty();
                    item.tblAddress.tblTown.tblCounty.tblCountry = new tblCountry();
                }
                if (item.ContactID == null)
                    item.tblContact = new tblContact();
                if (item.DepartmentID == null)
                    item.tblDepartment = new tblDepartment();
            }
            int pageSize = 15;
            int pageNumber = (page ?? 1);
            return PartialView("Elements/ListItem", list.ToPagedList(pageNumber, pageSize));
        }


        // if TeamId == null is add mode, else edit mode
        [HttpPost]
        public ActionResult Add_ActionLink(string TeamID = "")
        {
            ViewBag.TeamID = TeamID; // make edit or add mode
            return PartialView("Add");
        }

        public ActionResult Details(string TeamID = "")
        {
            tblTeam model = new tblTeam();

            if (!string.IsNullOrEmpty(TeamID)) // edit mode
            {
                model = db.tblTeams.Where(t => t.TeamID == new Guid(TeamID)).SingleOrDefault();
            }

            if (model.BusinessID == null)
            {
                if (string.IsNullOrEmpty(TeamID)) // set default value for add mode
                {
                    tblDepartment department = db.tblDepartments.Where(t => t.DepartmentID == new Guid(DepartmentID)).SingleOrDefault();
                    model.tblBusinessType = db.tblBusinessTypes.Where(t => t.BusinessID == department.tblDirectorate.tblOrganisation.BusinessID).SingleOrDefault();
                }
                else
                    model.tblBusinessType = new tblBusinessType();
            }
             
            if (model.AddressID == null)
            {
                model.tblAddress = new tblAddress();
                model.tblAddress.tblTown = new tblTown();
                model.tblAddress.tblTown.tblCounty = new tblCounty();
                model.tblAddress.tblTown.tblCounty.tblCountry = new tblCountry();
            }              
            if (model.ContactID == null)
                model.tblContact = new tblContact();
            if (model.DepartmentID == null)
                model.tblDepartment = new tblDepartment();

            return PartialView("Elements/Details", model);
        }

        public ActionResult Delete_ActionLink(string TeamID, string page)
        {
            tblTeam model = db.tblTeams.Where(t => t.TeamID == new Guid(TeamID)).SingleOrDefault();
            if(model != null)
            {
                db.tblTeams.Remove(model);
                db.SaveChanges();
            }

            return GetListTeams(int.Parse(page));
        }


        // if teamID == null add mode ortherwise edit mode
        public ActionResult InsertUpdate_Team(tblTeam model)
        {
            if(model.TeamID == Guid.Empty) // add mode
            {
                int count = db.tblTeams.Where(t => t.TeamName == model.TeamName).ToList().Count; // check new TeamName not exists
                if (count != 0)
                    return Content("Team name is existed. Please input another name!");
                
                model.TeamID = Guid.NewGuid();
                model.DepartmentID = Guid.Parse(DepartmentID);
                model.IsActive = true;
                db.tblTeams.Add(model);

                try
                {
                    db.SaveChanges();
                }
                catch(Exception ex)
                {
                    return Content("Failed to insert!");
                }

                return Content("Insert team successfully!");
            }
            else
            {
                int count = db.tblTeams.Where(t => t.TeamName == model.TeamName && t.TeamID != model.TeamID).ToList().Count; // check new TeamName not exists
                
                if(count != 0) // if new team name is exist
                    return Content("Team name is existed. Please input another name!");

                // not exist
                tblTeam update = db.tblTeams.Where(t => t.TeamID == model.TeamID).SingleOrDefault();
                
                if(update == null)
                    return Content("Team not exists!");

                // update information
                update.WebAddress = model.WebAddress;
                update.TeamName = model.TeamName;
                update.ShortDescription = model.ShortDescription;
                update.PhoneNumber = model.PhoneNumber;
                update.FullDescription = model.FullDescription;
                update.Fax = model.Fax;
                update.Email = model.Email;
                update.ContactID = model.ContactID;
                update.BusinessID = model.BusinessID;
                update.AddressLine1 = model.AddressLine1;
                update.AddressLine2 = model.AddressLine2;
                update.AddressLine3 = model.AddressLine3;
                update.AddressID = model.AddressID;

                try 
                {
                    db.SaveChanges();
                }
                catch(Exception ex)
                {
                    return Content(ex.Message);
                    //return Content("Failed to update!");
                }

                return Content("Update team successfully!");
            }
        }
    }
}
