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

        public ActionResult Index()
        {
            ////tblContact model = new tblContact();
            ////model.ContactID = new Guid();
            ////model.FirstName = "Minh";
            ////model.Surname = "Nhan";
            ////model.Email = "123456@yahoo.com";
            ////model.IsActive = true;

            ////db.tblContacts.Add(model);
            ////db.SaveChanges();

            return PartialView("ListContact");
        }

        [HttpPost]
        public ActionResult GetListItems(int? page, string FirstName, string Surname, string check)
        {
            int pageSize = 15;
            int pageNumber = (page ?? 1);
            List<tblContact> list = null;
            string a = check;
            if (string.IsNullOrEmpty(FirstName) == false && string.IsNullOrEmpty(Surname) == true)
                list = db.tblContacts.Where(t => t.FirstName.Contains(FirstName)).ToList();
            else
                if (string.IsNullOrEmpty(FirstName) == true && string.IsNullOrEmpty(Surname) == false)
                    list = db.tblContacts.Where(t => t.Surname.Contains(Surname)).ToList();
                else
                    if(string.IsNullOrEmpty(FirstName) == false && string.IsNullOrEmpty(Surname) == false)
                        list = db.tblContacts.Where(t => t.FirstName.Contains(FirstName) && t.Surname.Contains(Surname)).ToList();

            ViewBag.FirstName = FirstName;
            ViewBag.Surname = Surname;
            ViewBag.check = check;
            return PartialView("Elements/ListItem", list.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult EditContact()
        {
            return PartialView("EditContact");
        }

        public ActionResult AddContact(string ContactID)
        {
            string title = "";
            if (string.IsNullOrEmpty(ContactID))
                title = "Create New Contact";
            else
                title = "Update Contact";

            ViewBag.Title = title;
            return PartialView("AddContact");
        }

        public ActionResult DeleteContact()
        {
            return PartialView("EditContact");
        }
    }
}
