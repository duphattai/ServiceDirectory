using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServiceDirectory.Models;
using PagedList;
namespace ServiceDirectory.Controllers
{
    public class MaintainContactController : Controller
    {
        //
        // GET: /ListContact/
        ServiceDirectory.Models.ServiceDirectoryEntities db = new Models.ServiceDirectoryEntities();
        static string FirstName;
        static string Surname;
        static string Check;

        public ActionResult Index()
        {
            return PartialView("ListContact");
        }

        [HttpPost]
        public ActionResult GetListItems(int? page, string FirstName, string Surname, string Check)
        {
            int pageSize = 15;
            int pageNumber = (page ?? 1);
            List<tblContact> list = null;

            MaintainContactController.Check = Check;
            MaintainContactController.FirstName = FirstName;
            MaintainContactController.Surname = Surname;

            if (Check == "on")
            {

                if (string.IsNullOrEmpty(FirstName) == false && string.IsNullOrEmpty(Surname) == true)
                {
                    list = db.tblContacts.Where(t => t.FirstName.Contains(FirstName)).ToList();
                }
                else
                {
                    if (string.IsNullOrEmpty(FirstName) == true && string.IsNullOrEmpty(Surname) == false)
                    {
                        list = db.tblContacts.Where(t => t.Surname.Contains(Surname)).ToList();
                    }

                    else
                        if (string.IsNullOrEmpty(FirstName) == false && string.IsNullOrEmpty(Surname) == false)
                        {
                            list = db.tblContacts.Where(t => t.FirstName.Contains(FirstName) && t.Surname.Contains(Surname)).ToList();
                        }
                }

            }
            else
            {

                if (string.IsNullOrEmpty(FirstName) == false && string.IsNullOrEmpty(Surname) == true)
                {
                    list = db.tblContacts.Where(t => t.FirstName.Contains(FirstName) && t.IsActive == true).ToList();
                }
                else
                {
                    if (string.IsNullOrEmpty(FirstName) == true && string.IsNullOrEmpty(Surname) == false)
                    {
                        list = db.tblContacts.Where(t => t.Surname.Contains(Surname) && t.IsActive == true).ToList();
                    }

                    else
                    {
                        if (string.IsNullOrEmpty(FirstName) == false && string.IsNullOrEmpty(Surname) == false)
                            list = db.tblContacts.Where(t => t.FirstName.Contains(FirstName) && t.Surname.Contains(Surname) && t.IsActive == true).ToList();
                    }

                }

            }


            ViewBag.FirstName = FirstName;
            ViewBag.Surname = Surname;
            ViewBag.check = Check;
            return PartialView("Elements/ListItem", list.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult EditContact()
        {
            return PartialView("EditContact");
        }

        public ActionResult Add_ActionLink(string ContactID)
        {
            tblContact model = null;
            string title = "";
            if (string.IsNullOrEmpty(ContactID))
                title = "Create New Contact";

            if (string.IsNullOrEmpty(ContactID)) // add mode
            {
                model = new tblContact();
                model.ContactID = -1; // key to know add mode
            }
            else // edit mode
            {
                int id = int.Parse(ContactID);
                model = db.tblContacts.Where(t => t.ContactID == id).SingleOrDefault();
                title = "Edit Contact";
            }

            List<SelectListItem> listContactType = new List<SelectListItem>();

            List<tblReferenceData> temp = db.tblReferenceDatas.Where(t => t.RefCode == 23).ToList();
            foreach (var item in temp)
            {
                SelectListItem t = new SelectListItem { Text = item.RefValue, Value = item.RefID.ToString() };
                listContactType.Add(t);
            }

            List<SelectListItem> listBestContactMethod = new List<SelectListItem>();
            temp = db.tblReferenceDatas.Where(t => t.RefCode == 24).ToList();
            foreach (var item in temp)
            {
                SelectListItem t = new SelectListItem { Text = item.RefValue, Value = item.RefID.ToString() };
                listBestContactMethod.Add(t);
            }

            ViewBag.listBestContactMethod = listBestContactMethod;
            ViewBag.listContactType = listContactType;

            ViewBag.Title = title;
            return PartialView("AddContact", model);
        }

        public ActionResult InsertUpdate_Contact(tblContact model, string Check, string ContactType, string BestContactMethod)
        {
            model.IsActive = true;
            if (model.ContactID == -1)
            {

                if (Check != "on")
                {
                    model.IsActive = false;
                }

                int id = int.Parse(BestContactMethod);
                tblReferenceData temp = db.tblReferenceDatas.Where(t => t.RefID == id).SingleOrDefault();

                // add best contact method
                model.tblReferenceDatas.Add(temp);

                id = int.Parse(ContactType);
                temp = db.tblReferenceDatas.Where(t => t.RefID == id).SingleOrDefault();
                model.tblReferenceDatas.Add(temp);

                db.tblContacts.Add(model);
            }
            else // edit
            {
                tblContact update = db.tblContacts.Where(t => t.ContactID == model.ContactID).SingleOrDefault();

                foreach (var item in update.tblReferenceDatas.ToList())
                {
                    update.tblReferenceDatas.Remove(item);
                }
                db.SaveChanges();


                int id = int.Parse(BestContactMethod);
                tblReferenceData temp = db.tblReferenceDatas.Where(t => t.RefID == id).SingleOrDefault();

                // add best contact method
                update.tblReferenceDatas.Add(temp);

                id = int.Parse(ContactType);
                temp = db.tblReferenceDatas.Where(t => t.RefID == id).SingleOrDefault();
                update.tblReferenceDatas.Add(temp);

                db.Entry(update).CurrentValues.SetValues(model);
                db.Entry(update).Property(t => t.ContactID).IsModified = false;
            }


            try
            {
                db.SaveChanges();
            }
            catch
            {
                return Content("Cannot save contact!");
            }
            return Content("Save success!");
        }

        public ActionResult Delete_ActionLink(string ContactID, int page)
        {
            int id = int.Parse(ContactID);
            tblContact model = db.tblContacts.Where(t => t.ContactID == id).SingleOrDefault();
            string message = "";
            if (model != null)
            {
                try
                {
                    foreach (var item in model.tblReferenceDatas.ToList())
                    {
                        model.tblReferenceDatas.Remove(item);
                    }

                    db.tblContacts.Remove(model);
                    db.SaveChanges();
                }
                catch
                {
                    message = "Contact is using. Cannot delete it!";
                }
            }

            ViewBag.Message = message;
            return GetListItems(page, FirstName, Surname, Check);
        }
    }
}
