using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ServiceDirectory.Areas.NormalUser.Controllers
{
    public class MaintainTeamController : Controller
    {
        ServiceDirectory.Models.ServiceDirectoryEntities db = MaintainOrganisationController.database;
        //
        // GET: /NormalUser/MaintainTeam/

        public ActionResult Index()
        {
            return View();
        }

    }
}
