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
        ServiceDirectoryEntities db = MaintainOrganisationController.database;
        static string OrgID = null;
        //
        // GET: /NormalUser/MaintainSupportingMaterial/


        public ActionResult Index(string orgID)
        {
            OrgID = orgID;
            return PartialView("List");
        }

        public ActionResult List(int? page)
        {

            var list = db.tblSupportingMaterials.Where(t => t.OrgID == int.Parse(OrgID)).OrderBy(g => g.URL).ToList();
            int pageSize = 15;
            int pageNumber = (page ?? 1);
            return PartialView("Elements/ListItem", list.ToPagedList(pageNumber, pageSize));
        }


        // if URL == "" add mode, otherwise edit mode
        public ActionResult Add_ActionLink(string URL = "")
        {
            @ViewBag.URL = URL;
            return PartialView("Add");
        }



        // if URL == "" is add mode, else edit mode
        [HttpPost]
        public ActionResult InsertUpdate_SupportMaterial(tblSupportingMaterial model, string OldURL = "")
        {
            if(string.IsNullOrEmpty(OldURL)) // add mode
            {
                // check URL is exists
                tblSupportingMaterial exists = db.tblSupportingMaterials.Where(t => t.URL == model.URL && t.OrgID == int.Parse(OrgID)).SingleOrDefault();

                if(exists == null) // not exists, can insert
                {
                    model.AddedDate = DateTime.Now;
                    model.OrgID = int.Parse(OrgID);
                    db.tblSupportingMaterials.Add(model);
                    db.SaveChanges();

                    return Content("Save supporting material successfully");
                }
                else
                    return Content("URL is exist. Please input difference URL");  
            }
            else //edit mode
            {
                // check new URL is exists
                List<tblSupportingMaterial> list = db.tblSupportingMaterials.Where(t => (t.URL == model.URL || t.URL == OldURL ) && t.OrgID == int.Parse(OrgID)).ToList();

                if(list.Count < 2) // not exist, can update
                {
                    tblSupportingMaterial update = db.tblSupportingMaterials.Where(t => t.URL == OldURL).SingleOrDefault();
                    update.URL = model.URL;
                    update.ShortDescription = model.ShortDescription;
                    update.TypeFile = model.TypeFile;

                    db.SaveChanges();
                    return Content("Save supporting material successfully");
                }
                else // new URL is exists
                    return Content("URL is exist. Please input difference URL");
            }
        }

        // if Id == "" add mode, otherwise edit mode
        public ActionResult Details(string URL = "")
        {
            tblSupportingMaterial model = new tblSupportingMaterial();
            if(!string.IsNullOrEmpty(URL))
            {
                model = db.tblSupportingMaterials.Where(t => t.URL == URL && t.OrgID == int.Parse(OrgID)).SingleOrDefault();
            }
            model.AddedDate = DateTime.Today;
            return PartialView("Elements/Details", model);
        }

        // delete item from list
        public ActionResult Delete_ActionLink(string URL, string offset = "")
        {
            // delete item selected
            Guid guid = new Guid(OrgID);
            tblSupportingMaterial delete = db.tblSupportingMaterials.Where(t => t.URL == URL && t.OrgID == int.Parse(OrgID)).SingleOrDefault();
            if(delete != null)
            {
                db.tblSupportingMaterials.Remove(delete);
                db.SaveChanges();
            }

            // show new list item
            List<tblSupportingMaterial> model = db.tblSupportingMaterials.OrderByDescending(order => order.URL).Skip(0).Take(15).ToList();
            // create user account into model
            foreach (var item in model)
            {
                item.tblUser = db.tblUsers.Where(t => t.UserID == item.UserID).SingleOrDefault();
            }

            return PartialView("Elements/ListItem", model);
        }
    }
}
