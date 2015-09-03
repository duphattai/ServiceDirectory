using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using ServiceDirectory.Models;
using System.Data.Entity.Validation;
using System.IO;
using System.Web.Routing;
namespace ServiceDirectory.Areas.NormalUser.Controllers
{
    public class MaintainTeamController : Controller
    {
        ServiceDirectoryEntities db = new ServiceDirectoryEntities();

        //
        // GET: /NormalUser/MaintainTeam/
        static string DepartmentID;
        static int PageNumber;
        int PageSize = 15;
        static bool IncludeInactive = false;
        public ActionResult Index(string DepartmentID)
        {
            MaintainTeamController.DepartmentID = DepartmentID;
            return PartialView("List");
        }



        public ActionResult GetListTeams(int page = -1)
        {
            int guid = int.Parse(DepartmentID);
            List<tblTeam> list = null;

            
            if (IncludeInactive)
                list = db.tblTeams.Where(t => t.DepartmentID == guid).OrderBy(g => g.TeamName).ToList();
            else
                list = db.tblTeams.Where(t => t.DepartmentID == guid && t.IsActive == true).OrderBy(g => g.TeamName).ToList();

            PageNumber = page != -1 ? page : PageNumber; // if not new page, will load current page
            

            return PartialView("Elements/ListItem", list.ToPagedList(PageNumber, PageSize));
        }

        public string GetListTeamsFromCheckbox(bool IncludeInActive)
        {
            int guid = int.Parse(DepartmentID);
            MaintainTeamController.IncludeInactive = IncludeInActive;
            List<tblTeam> list = null;

            
            if (IncludeInActive)
                list = db.tblTeams.Where(t => t.DepartmentID == guid).OrderBy(g => g.TeamName).ToList();
            else
                list = db.tblTeams.Where(t => t.DepartmentID == guid && t.IsActive == true).OrderBy(g => g.TeamName).ToList();
            
            
            string html = RenderPartialViewToString(this, "~/Areas/NormalUser/Views/MaintainTeam/Elements/ListItem.cshtml", list.ToPagedList(PageNumber, PageSize));

            html = html.Replace("/MaintainTeam/Add_ActionLink", "/NormalUser/MaintainTeam/Add_ActionLink"); // fix wrong href, href missing name of areas (error ajax not call)
            html = html.Replace("/MaintainTeam/GetListTeams", "/NormalUser/MaintainTeam/GetListTeams"); // fix error paging not work (wrong href)
            return html;
        }

        public static string RenderPartialViewToString(Controller controller,string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = controller.ControllerContext.RouteData.GetRequiredString("action");

            controller.ViewData.Model = model;

            using(StringWriter sw = new StringWriter())
            { 
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }

       
        // if TeamId == null is add mode, else edit mode
        [HttpPost]
        public ActionResult Add_ActionLink(string TeamID = null)
        {
            ViewBag.TeamID = TeamID; // make edit or add mode

            // check if edit mode will add button in-active
            ViewBag.IsEditMode = string.IsNullOrEmpty(TeamID) == true ? false : true;

            return PartialView("Add");
        }

        public ActionResult Details(string TeamID = null)
        {
            tblTeam model = new tblTeam();
            int departID = int.Parse(DepartmentID);
            tblDepartment department = null;
            
            department = db.tblDepartments.Where(t => t.DepartmentID == departID).SingleOrDefault();

            if (!string.IsNullOrEmpty(TeamID)) // edit mode
            {
                int teamID = int.Parse(TeamID);
                model = db.tblTeams.Where(t => t.TeamID == teamID).SingleOrDefault();
            }
            else
            {
                model.TeamID = -1; // set id for add mode
                // set default value
                model.WebAddress = department.tblDirectorate.tblOrganisation.WebAddress;
                model.tblBusinessType = db.tblBusinessTypes.Where(t => t.BusinessID == department.tblDirectorate.tblOrganisation.BusinessID).SingleOrDefault();
            }
            

            // set value for copy address checkbox
            ViewBag.OrgAddressLine1 = department.tblDirectorate.tblOrganisation.AddressLine1;
            ViewBag.OrgAddressLine2 = department.tblDirectorate.tblOrganisation.AddressLine2;
            ViewBag.OrgAddressLine3 = department.tblDirectorate.tblOrganisation.AddressLine3;

            ViewBag.DeparAddressLine1 = department.AddressLine1;
            ViewBag.DeparAddressLine2 = department.AddressLine2;
            ViewBag.DeparAddressLine3 = department.AddressLine3;

            return PartialView("Elements/Details", model);
        }

        public ActionResult Delete_ActionLink(string TeamID, string page)
        {
            
            int teamID = int.Parse(TeamID);
            tblTeam model = db.tblTeams.Where(t => t.TeamID == teamID).SingleOrDefault();
            if (model != null)
            {
                db.tblTeams.Remove(model);
                db.SaveChanges();
            }

            return GetListTeams(int.Parse(page));
        }
        

        // if teamID == -1 add mode ortherwise edit mode
        public ActionResult InsertUpdate_Team(tblTeam model)
        {
            
            model.IsActive = true;

            if(model.TeamID == -1) // add mode
            {
                int count = db.tblTeams.Where(t => t.TeamName == model.TeamName).ToList().Count; // check new TeamName not exists
                if (count != 0)
                    return Content("Team name is existed. Please input another name!");
                
                model.DepartmentID = int.Parse(DepartmentID);
                db.tblTeams.Add(model);

                try
                {
                    db.SaveChanges();
                }
                catch
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

                db.Entry(update).CurrentValues.SetValues(model);
                db.Entry(update).Property(p => p.IsActive).IsModified = true;
                db.Entry(update).Property(p => p.TeamID).IsModified = false;
                db.Entry(update).Property(p => p.DepartmentID).IsModified = false;

                try 
                {
                    db.SaveChanges();
                }
                catch(Exception ex)
                {
                    return Content(ex.Message);
                }
            }
            return Content("Update team successfully!");
            
        }


        public ActionResult MakeInActive(bool IsActive, string TeamID)
        {
            int id = int.Parse(TeamID);
            
            tblTeam model = db.tblTeams.Where(t => t.TeamID == id).SingleOrDefault();
            model.IsActive = IsActive;
            try
            {
                db.SaveChanges();
                return Content("Make the team in-active successfully");
            }
            catch
            {
                return Content("Cannot make in-active");
            }  
        }
    }
}
