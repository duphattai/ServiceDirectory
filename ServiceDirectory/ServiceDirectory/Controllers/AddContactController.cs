using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ServiceDirectory.Controllers
{
    public class AddContactController : Controller
    {
        //
        // GET: /AddContact/

        public ActionResult Index()
        {
            return PartialView("AddContact");
        }

    }
}
