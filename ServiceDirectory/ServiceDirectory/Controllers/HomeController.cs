using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServiceDirectory.Models;
using System.Net.Mail;
using System.Net;

namespace ServiceDirectory.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        ServiceDirectoryEntities db = new ServiceDirectoryEntities();
        public static tblUser user = null;
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Logon(string lg_username, string lg_password)
        {
            tblUser v = db.tblUsers.Where(t => t.Account == lg_username && t.UserPassword == lg_password).SingleOrDefault();

            if (v == null)
            {
                ViewBag.message = "Username or password not match, please input again!";
                return PartialView("Index");
            }
            else
            {
                user = v;
                return PartialView("AfterLogon");
            }
        }


        public ActionResult ForgotPassword()
        {
            return PartialView();
        }

        public ActionResult SignUp()
        {
            return PartialView();
        }


        [HttpPost]
        public ActionResult SendPassWordToEmail(string user, string email)
        {
            tblUser model = db.tblUsers.Where(t => t.Account == user && t.Email == email).SingleOrDefault();
            string message = "";
            if (model != null)
            {
                MailMessage mm = new MailMessage();
                mm.Subject = "Password Recovery";
                mm.Body = string.Format("Hi {0},<br /><br />Your password is {1}.<br /><br />Thank You.", model.Account, model.UserPassword);
                mm.IsBodyHtml = true;
                mm.From = new MailAddress("quockhin@gmail.com");
                mm.To.Add(model.Email);
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                smtp.Credentials = new System.Net.NetworkCredential("quockhin@gmail.com", "quoc27050711");
                smtp.Port = 587;
                smtp.Send(mm);

                message = "Password has been sent to your email address.";
                ViewBag.Message = message;
                return PartialView("Complete");
            }
            else
                message = "This email address does not match our records.";

            ViewBag.Message = message;
            return PartialView("ForgotPassword");
        }
        // signed up

        public ActionResult InsertUpdate_User(tblUser model)
        {
            int existAccount = db.tblUsers.Where(t => t.Account == model.Account).ToList().Count;
            int existEmail = db.tblUsers.Where(t => t.Email == model.Email).ToList().Count;

            if (existAccount != 0 || existEmail != 0) // cannot insert
            {
                string message = "";
                if (existAccount != 0) message = "Account exist. Please input another account";
                else if (existEmail != 0) message = "Email exist. Please input another email";

                ViewBag.Message = message;
                return PartialView("SignUp");
            }
            else // inserted
            {
                string message = "Signup success!";

                db.tblUsers.Add(model);
                db.SaveChanges();

                ViewBag.Message = message;
                return PartialView("Complete");
            }
        }
    }
}
