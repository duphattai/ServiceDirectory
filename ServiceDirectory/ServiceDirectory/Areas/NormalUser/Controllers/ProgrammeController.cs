using ServiceDirectory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.IO;
using System.Data;

namespace ServiceDirectory.Areas.NormalUser.Controllers
{
    public class ProgrammeController : Controller
    {
        //
        // GET: /NormalUser/Programme/
        private ServiceDirectoryEntities db = new ServiceDirectoryEntities();
        static int PageNumber;
        int PageSize = 15;
        static bool InActive = false;

        public ActionResult Index()
        {
            return PartialView("List");
        }

        public ActionResult Add()
        {
            return PartialView(new tblProgramme());
        }

        public ActionResult Edit(string ProgrammeID)
        {
            ViewBag.ProgrammeID = ProgrammeID;
            return PartialView();
        }

        public ActionResult GetList(int page = -1)
        {
            List<tblProgramme> list = null;

            if (InActive == true)
            {
                list = db.tblProgrammes.ToList();
            }
            else
            {
                list = db.tblProgrammes.Where(t => t.IsActive == true).ToList();
            }

            PageNumber = page != -1 ? page : PageNumber;

            return PartialView("Elements/ListItem", list.ToPagedList(PageNumber, PageSize));
        }

        public string GetListInActive(bool InActive)
        {
            ProgrammeController.InActive = InActive;
            List<tblProgramme> list = null;

            if (InActive == true)
            {
                list = db.tblProgrammes.ToList();
            }
            else
            {
                list = db.tblProgrammes.Where(t => t.IsActive == true).ToList();
            }

            string url = RenderPartialViewToString(this, "~/Areas/NormalUser/Views/Programme/Elements/ListItem.cshtml", list.ToPagedList(PageNumber, PageSize));

            url = url.Replace("/Programme/Edit", "/NormalUser/Programme/Edit");
            url = url.Replace("/Programme/GetList", "/NormalUser/Programme/GetList");
            return url;
        }

        public static string RenderPartialViewToString(Controller controller, string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = controller.ControllerContext.RouteData.GetRequiredString("action");

            controller.ViewData.Model = model;

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }

        [HttpPost]
        public ActionResult Add_Edit_Programme(tblProgramme model)
        {
            model.IsActive = true;
            if (model.ProgrammeID == -1)
            {
                int exist = db.tblProgrammes.Where(t => t.ProgrammeName == model.ProgrammeName).ToList().Count;

                if (exist != 0)
                    return Content("Programme name is existed. Please input another name");

                db.tblProgrammes.Add(model);
            }
            else
            {
                int exist = db.tblProgrammes.Where(t => t.ProgrammeName == model.ProgrammeName && t.ProgrammeID != model.ProgrammeID).ToList().Count;

                if (exist != 0)
                    return Content("Programme name is existed. Please input another name");

                tblProgramme update = db.tblProgrammes.Where(t => t.ProgrammeID == model.ProgrammeID).SingleOrDefault();

                db.Entry(update).CurrentValues.SetValues(model);
                db.Entry(update).Property(t => t.ProgrammeID).IsModified = false;
            }

            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }


            return Content("Save Programme successfully!");
        }

        public ActionResult Delete(string ProgrammeID, int page)
        {
            int id = int.Parse(ProgrammeID);
            tblProgramme delete = db.tblProgrammes.Where(t => t.ProgrammeID == id).SingleOrDefault();
            string message = "";
            if (delete != null)
            {
                try
                {
                    db.tblProgrammes.Remove(delete);
                    db.SaveChanges();
                }
                catch
                {
                    message = "This Programme is using. Cannot delete it!";
                }
            }

            ViewBag.Message = message;
            return GetList(page);
        }

        public ActionResult Details(string ProgrammeID = null)
        {
            tblProgramme model = null;
            if (!string.IsNullOrEmpty(ProgrammeID))
            {
                int id = int.Parse(ProgrammeID);
                model = db.tblProgrammes.Where(t => t.ProgrammeID == id).SingleOrDefault();
            }
            else
            {
                model = new tblProgramme();
                model.ProgrammeID = -1;// key to know add mode
            }
            return PartialView("~/Areas/NormalUser/Views/Programme/Elements/Details.cshtml", model);
        }
    }
}
