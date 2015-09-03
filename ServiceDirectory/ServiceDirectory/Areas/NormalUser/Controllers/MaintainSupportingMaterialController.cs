using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServiceDirectory.Models;
using PagedList;

namespace ServiceDirectory.Areas.NormalUser.Controllers
{
    public class MaintainSupportingMaterialController : Controller
    {
        ServiceDirectoryEntities db = new ServiceDirectoryEntities();
        static int OrgID;
        static bool IncludeInactive = false;
        int PageSize = 15;
        static int PageNumber;
        //
        // GET: /NormalUser/MaintainSupportingMaterial/


        public ActionResult Index(string OrgID)
        {
            MaintainSupportingMaterialController.OrgID = int.Parse(OrgID);
            return PartialView("List");
        }

        public ActionResult GetListSupportingMaterials(int page = -1)
        {
            
            List<tblSupportingMaterial> list = null;

            if (IncludeInactive)
                list = db.tblSupportingMaterials.Where(t => t.OrgID == OrgID).OrderBy(g => g.URL).ToList();
            else
                list = db.tblSupportingMaterials.Where(t => t.OrgID == OrgID && t.IsActive == true).OrderBy(g => g.URL).ToList();

            PageNumber = page != -1 ? page : PageNumber;
            return PartialView("Elements/ListItem", list.ToPagedList(PageNumber, PageSize));
            
        }

        public string GetListSupportingMaterialsFromCheckbox(bool IncludeInactive)
        {
            
                MaintainSupportingMaterialController.IncludeInactive = IncludeInactive;
                List<tblSupportingMaterial> list = null;

                if (IncludeInactive)
                    list = db.tblSupportingMaterials.Where(t => t.OrgID == OrgID).OrderBy(g => g.URL).ToList();
                else
                    list = db.tblSupportingMaterials.Where(t => t.OrgID == OrgID && t.IsActive == true).OrderBy(g => g.URL).ToList();


                string html = MaintainTeamController.RenderPartialViewToString(this, "~/Areas/NormalUser/Views/MaintainSupportingMaterial/Elements/ListItem.cshtml", list.ToPagedList(PageNumber, PageSize));

                // fix href missing name of ares
                html = html.Replace("/MaintainSupportingMaterial/Add_ActionLink", "/NormalUser/MaintainSupportingMaterial/Add_ActionLink");
                html = html.Replace("/MaintainSupportingMaterial/GetListSupportingMaterials", "/NormalUser/MaintainSupportingMaterial/GetListSupportingMaterials");
                return html;
            
        }

        // if URL == "" add mode, otherwise edit mode
        public ActionResult Add_ActionLink(string SupportID)
        {
            @ViewBag.SupportID = SupportID;
            return PartialView("Add");
        }



        // if URL == "" is add mode, else edit mode
        [HttpPost]
        public ActionResult InsertUpdate_SupportMaterial(tblSupportingMaterial model)
        {
            
            model.IsActive = true;
            if (model.SupportID == 0) // add mode
            {
                // check URL is exists
                int exists = db.tblSupportingMaterials.Where(t => t.URL == model.URL).ToList().Count;

                if (exists == 0) // not exists, can insert
                {
                    model.AddedDate = DateTime.Now;
                    model.OrgID = OrgID;
                    db.tblSupportingMaterials.Add(model);
                }
                else
                    return Content("URL is exist. Please input difference URL");
            }
            else //edit mode
            {
                // check new URL is exists
                int exist = db.tblSupportingMaterials.Where(t => t.URL == model.URL && t.SupportID != model.SupportID).ToList().Count;

                if (exist == 0) // not exist, can update
                {
                    tblSupportingMaterial update = db.tblSupportingMaterials.Where(t => t.SupportID == model.SupportID).SingleOrDefault();

                    db.Entry(update).CurrentValues.SetValues(model);
                    db.Entry(update).Property(t => t.OrgID).IsModified = false;
                    db.Entry(update).Property(t => t.AddedDate).IsModified = false;
                    db.Entry(update).Property(t => t.UserID).IsModified = false;
                }
                else // new URL is exists
                    return Content("URL is exist. Please input difference URL");
            }

            try
            {
                db.SaveChanges();
            }
            catch
            {
                return Content("Cannot save supporting material");
            }

            return Content("Save supporting material successfully");
            
        }

        // if Id == "" add mode, otherwise edit mode
        public ActionResult Details(string SupportID = "")
        {
           
            tblSupportingMaterial model = new tblSupportingMaterial();
            if (!string.IsNullOrEmpty(SupportID))
            {
                int id = int.Parse(SupportID);
                model = db.tblSupportingMaterials.Where(t => t.SupportID == id).SingleOrDefault();
            }
            model.AddedDate = DateTime.Today;
            model.UserID = ServiceDirectory.Controllers.HomeController.user.UserID;
            model.tblUser = ServiceDirectory.Controllers.HomeController.user;
            return PartialView("Elements/Details", model);
            
        }

        // delete item from list
        public ActionResult Delete_ActionLink(string SupportID)
        {
            
            int id = int.Parse(SupportID);
            // delete item selected
            tblSupportingMaterial delete = db.tblSupportingMaterials.Where(t => t.SupportID == id).SingleOrDefault();
            if (delete != null)
            {
                db.tblSupportingMaterials.Remove(delete);
                db.SaveChanges();
            }

            return GetListSupportingMaterials(-1);
            
        }
    }
}
