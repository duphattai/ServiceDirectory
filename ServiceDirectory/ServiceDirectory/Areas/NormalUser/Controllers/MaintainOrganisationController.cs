using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServiceDirectory.Models;
using PagedList;
namespace ServiceDirectory.Areas.NormalUser.Controllers
{
    public class MaintainOrganisationController : Controller
    {
        static public ServiceDirectoryEntities database = new ServiceDirectoryEntities();
        static int PageNumber;
        int PageSize = 5;
        static bool IncludeInActive = false;
        enum GroupReference
        {
            organisation_specicalism = 1,
            service_personal_circumstances_capabilities = 2,
            service_disabilities_capabilities = 3,
            service_ethnicity_capabilities = 4,
            service_barriers_capabilities = 5,
            accreditation = 6,
            service_benefits_capabilities = 7
        }
        //
        // GET: /NormalUser/MaintainOrganisation/

        public ActionResult Index()
        {
            return PartialView("List");
        }

        // call view Add
        public ActionResult Add_ActionLink()
        {
            return PartialView("Add", new tblOrganisation());
        }

        // call view Edit
        public ActionResult Edit_ActionLink(string OrgID)
        {
            ViewBag.OrgID = OrgID;
            return PartialView("Edit");
        }


        public ActionResult GetListOrganisations(int page = -1)
        {
            List<tblOrganisation> list = null;

            if (IncludeInActive == true)
                list = database.tblOrganisations.ToList();
            else
                list = database.tblOrganisations.Where(t => t.IsActive == true).ToList();

            PageNumber = page != -1 ? page : PageNumber;
            return PartialView("Elements/ListItem", list.ToPagedList(PageNumber, PageSize));
        }

        // get data and fill data into Details 1
        // if OrgId == "" add mode, otherwise edit mode
        public ActionResult Details_One(string OrgID = "")
        {
            tblOrganisation model = new tblOrganisation();
            if(!string.IsNullOrEmpty(OrgID))
            {
                int id = int.Parse(OrgID);
                model = database.tblOrganisations.Where(t => t.OrgID == id).SingleOrDefault();
            }

            return PartialView("Elements/Details_one", model);
        }

        // get data and fill data into Details 2
        // if OrgId == "" add mode, otherwise edit mode
        public ActionResult Details_two(string OrgID = "")
        {
            int id;

            List<SelectListItem> listSerPers = new List<SelectListItem>();
            List<SelectListItem> listSerDis = new List<SelectListItem>();
            List<SelectListItem> listSerEth = new List<SelectListItem>();
            List<SelectListItem> listSerBarri = new List<SelectListItem>();
            List<SelectListItem> listAccr = new List<SelectListItem>();
            List<SelectListItem> listSerBene = new List<SelectListItem>(); 
            List<SelectListItem> listOrgSpec = new List<SelectListItem>();

            // edit mode
            List<tblReferenceData> listReference = null;
            if(!string.IsNullOrEmpty(OrgID))
            {
                id = int.Parse(OrgID);
                listReference = database.tblOrganisations.Where(t => t.OrgID == id).SingleOrDefault().tblReferenceDatas.ToList();
            }
                

            using(ServiceDirectoryEntities db = new ServiceDirectoryEntities())
            {
                List<tblReferenceData> temp = db.tblReferenceDatas.Where(org => org.RefCode == (int)GroupReference.organisation_specicalism).ToList();
                foreach (var item in temp)
                {
                    listOrgSpec.Add(new SelectListItem { Text = item.RefValue, Value = item.RefID.ToString()});

                    // edit mode and make checked
                    if (!string.IsNullOrEmpty(OrgID) && listReference.FindIndex(t => t.RefID.Equals(item.RefID)) != -1)
                    {
                        listOrgSpec[listOrgSpec.Count - 1].Selected = true;
                    }
                }

                temp = db.tblReferenceDatas.Where(org => org.RefCode == (int)GroupReference.accreditation).ToList();
                foreach (var item in temp)
                {
                    listSerPers.Add(new SelectListItem { Text = item.RefValue, Value = item.RefID.ToString() });
                    // edit mode and make checked
                    if (!string.IsNullOrEmpty(OrgID) && listReference.FindIndex(t => t.RefID.Equals(item.RefID)) != -1)
                    {
                        listSerPers[listSerPers.Count - 1].Selected = true;
                    }
                }

                temp = db.tblReferenceDatas.Where(org => org.RefCode == (int)GroupReference.service_disabilities_capabilities).ToList();
                foreach (var item in temp)
                {
                    listSerDis.Add(new SelectListItem { Text = item.RefValue, Value = item.RefID.ToString() });
                    // edit mode and make checked
                    if (!string.IsNullOrEmpty(OrgID) && listReference.FindIndex(t => t.RefID.Equals(item.RefID)) != -1)
                    {
                        listSerDis[listSerDis.Count - 1].Selected = true;
                    }
                }
                
                temp = db.tblReferenceDatas.Where(org => org.RefCode == (int)GroupReference.service_ethnicity_capabilities).ToList();
                foreach (var item in temp)
                {
                    listSerEth.Add(new SelectListItem { Text = item.RefValue, Value = item.RefID.ToString() });
                    // edit mode and make checked
                    if (!string.IsNullOrEmpty(OrgID) && listReference.FindIndex(t => t.RefID.Equals(item.RefID)) != -1)
                    {
                        listSerEth[listSerEth.Count - 1].Selected = true;
                    }
                }
                
                temp = db.tblReferenceDatas.Where(org => org.RefCode == (int)GroupReference.service_barriers_capabilities).ToList();
                foreach (var item in temp)
                {
                    listSerBarri.Add(new SelectListItem { Text = item.RefValue, Value = item.RefID.ToString() });
                    // edit mode and make checked
                    if (!string.IsNullOrEmpty(OrgID) && listReference.FindIndex(t => t.RefID.Equals(item.RefID)) != -1)
                    {
                        listSerBarri[listSerBarri.Count - 1].Selected = true;
                    }
                }
                
                temp = db.tblReferenceDatas.Where(org => org.RefCode == (int)GroupReference.service_benefits_capabilities).ToList();
                foreach (var item in temp)
                {
                    listSerBene.Add(new SelectListItem { Text = item.RefValue, Value = item.RefID.ToString() });
                    // edit mode and make checked
                    if (!string.IsNullOrEmpty(OrgID) && listReference.FindIndex(t => t.RefID.Equals(item.RefID)) != -1)
                    {
                        listSerBene[listSerBene.Count - 1].Selected = true;
                    }
                }
            }

            ViewBag.listOrgSpec = listOrgSpec;
            ViewBag.listSerPers = listSerPers;
            ViewBag.listSerDis = listSerDis;
            ViewBag.listSerEth = listSerEth;
            ViewBag.listSerBarri = listSerBarri;
            ViewBag.listAccr = listAccr;
            ViewBag.listSerBene = listSerBene;

            return PartialView("Elements/Details_two");
        }

        // get data and fill data into Details 3
        // If OrgId == "" add mode, OrgID != "" edit mode 
        public ActionResult Details_three(string OrgID = "")
        {
            int id;
            List<SelectListItem> listSer = new List<SelectListItem>();
            List<SelectListItem> listProg = new List<SelectListItem>();
            
            List<tblOrganisationService> listOrgLinkedSer = null;
            List<tblProgramme> listOrgLinkedProg = null;
            if(!string.IsNullOrEmpty(OrgID))
            {
                id = int.Parse(OrgID);
                listOrgLinkedSer = database.tblOrganisationServices.Where(t => t.OrgID == id).ToList();
                tblOrganisation linked = database.tblOrganisations.Where(t => t.OrgID == id).SingleOrDefault();
                listOrgLinkedProg = linked.tblProgrammes.ToList();
            }
            
            // create checkbox item and make it checked
            foreach (var item in database.tblServices.ToList())
            {
                listSer.Add(new SelectListItem { Text = item.ServiceName, Value = item.ServiceID.ToString() });

                if (!string.IsNullOrEmpty(OrgID) && listOrgLinkedSer.FindIndex(t => t.ServiceID.Equals(item.ServiceID)) != -1)
                {
                    listSer[listSer.Count - 1].Selected = true;
                }
            }

            foreach(var item in database.tblProgrammes.ToList())
            {
                listProg.Add(new SelectListItem { Text = item.ProgrammeName, Value = item.ProgrammeID.ToString() });

                if (!string.IsNullOrEmpty(OrgID) && listOrgLinkedProg.FindIndex(t => t.ProgrammeID.Equals(item.ProgrammeID)) != -1)
                {
                    listProg[listProg.Count - 1].Selected = true;
                }
            }

            ViewBag.listProg = listProg;
            ViewBag.listSer = listSer;

            return PartialView("Elements/Details_three");
        }

        // get data and fill data into Details 4
        public ActionResult Details_four(string OrgID)
        {
            List<tblPremis> listLinked;
            
            int id  = int.Parse(OrgID);
            listLinked = database.tblOrganisations.Where(t => t.OrgID == id).SingleOrDefault().tblPremises.ToList();
            
            return PartialView("Elements/Details_four", listLinked);
        }


        public string GetListOrganisationsFromCheckbox(bool IncludeInActive)
        {
            MaintainOrganisationController.IncludeInActive = IncludeInActive;
            List<tblOrganisation> list = null;

            if (IncludeInActive == true)
                list = database.tblOrganisations.ToList();
            else
                list = database.tblOrganisations.Where(t => t.IsActive == true).ToList();

            string html = MaintainTeamController.RenderPartialViewToString(this, "~/Areas/NormalUser/Views/MaintainOrganisation/Elements/ListItem.cshtml", list.ToPagedList(PageNumber, PageSize));

            // fix href missing name of ares
            html = html.Replace("/MaintainOrganisation/Edit_ActionLink", "/NormalUser/MaintainOrganisation/Edit_ActionLink");
            html = html.Replace("/MaintainOrganisation/GetListOrganisations", "/NormalUser/MaintainOrganisation/GetListOrganisations");
            return html;
        }


        // listSerEth get checked list from Service Ethnicity Capabilities
        // listSerDis get checked list from Service Disabilities Capabilities
        // listOrgSpec get checked list .........
        [HttpPost]
        public ActionResult Add_Organisation(tblOrganisation model, string[] listSerEth, string[] listSerDis, string[] listOrgSpec, string[] listSerPers, string[] listSerBarri, string[] listAccr, string[] listSerBene)
        {
            
            return PartialView();
        }

        private void EditOrganisation(tblOrganisation model, string[] listSerEth, string[] listSerDis, string[] listOrgSpec, string[] listSerPers, string[] listSerBarri, string[] listAccr, string[] listSerBene)
        {
            tblOrganisation org = database.tblOrganisations.Where(t => t.OrgID == model.OrgID).SingleOrDefault();

            if(org != null)
            {
               
                

            }
        }
    }
}
