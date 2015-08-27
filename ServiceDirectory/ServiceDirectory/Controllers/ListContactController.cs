using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ServiceDirectory.Controllers
{
    public class ListContactController : Controller
    {
        //
        // GET: /ListContact/

        public ActionResult Index()
        {
            return PartialView("ListContact");
        }

    }
}
