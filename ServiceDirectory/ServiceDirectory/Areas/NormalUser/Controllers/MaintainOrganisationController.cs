using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServiceDirectory.Models;

namespace ServiceDirectory.Areas.NormalUser.Controllers
{
    public class MaintainOrganisationController : Controller
    {
        static public ServiceDirectoryEntities database = new ServiceDirectoryEntities();

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

        public ActionResult List()
        {
            List<tblOrganisation> list;
            using(ServiceDirectoryEntities db = new ServiceDirectoryEntities())
            {
                list = db.tblOrganisations.Take(15).ToList();
                foreach(var item in list)
                {
                    item.tblContact = db.tblContacts.Where(t => t.ContactID == item.ContactID).SingleOrDefault();
                    item.tblAddress = db.tblAddresses.Where(t => t.AddressID == item.AddressID).SingleOrDefault();
                }
            }

            return PartialView("~/Areas/NormalUser/Views/MaintainOrganisation/List.cshtml", list);
        }

        // call view Add
        public ActionResult Add_ActionLink()
        {
            return PartialView("~/Areas/NormalUser/Views/MaintainOrganisation/Add.cshtml", new tblOrganisation());
        }

        // call view Edit
        public ActionResult Edit_ActionLink(string id)
        {
            ViewBag.OrgID = id;
            return PartialView("~/Areas/NormalUser/Views/MaintainOrganisation/Edit.cshtml");
        }


        // get data and fill data into Details 1
        // if OrgId == "" add mode, otherwise edit mode
        public ActionResult Details_One(string OrgID = "")
        {
            tblOrganisation model = new tblOrganisation();
            if(!string.IsNullOrEmpty(OrgID))
            {
                Guid guid = new Guid(OrgID);
                model = database.tblOrganisations.Where(t => t.OrgID.Equals(guid)).SingleOrDefault();
            }

            return PartialView("~/Areas/NormalUser/Views/MaintainOrganisation/Elements/Details_one.cshtml", model);
        }

        // get data and fill data into Details 2
        // if OrgId == "" add mode, otherwise edit mode
        public ActionResult Details_two(string OrgID = "")
        {
            Guid guid = Guid.Empty;

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
                guid = new Guid(OrgID);
                listReference = database.tblOrganisations.Where(t => t.OrgID.Equals(guid)).SingleOrDefault().tblReferenceDatas.ToList();
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
            return PartialView("~/Areas/NormalUser/Views/MaintainOrganisation/Elements/Details_two.cshtml");
        }

        // get data and fill data into Details 3
        // If OrgId == "" add mode, OrgID != "" edit mode 
        public ActionResult Details_three(string OrgID = "")
        {
            Guid guid = Guid.Empty;
            List<SelectListItem> listSer = new List<SelectListItem>();
            List<SelectListItem> listProg = new List<SelectListItem>();
            
            List<tblOrganisationService> listOrgLinkedSer = null;
            List<tblProgramme> listOrgLinkedProg = null;
            if(!string.IsNullOrEmpty(OrgID))
            {
                guid = new Guid(OrgID);
                listOrgLinkedSer = database.tblOrganisationServices.Where(t => t.OrgID.Equals(guid)).ToList();
                tblOrganisation linked = database.tblOrganisations.Where(t => t.OrgID.Equals(guid)).SingleOrDefault();
                listOrgLinkedProg = linked.tblProgrammes.ToList();
            }
            
            // create checkbox item
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

            return PartialView("~/Areas/NormalUser/Views/MaintainOrganisation/Elements/Details_three.cshtml");
        }

        // get data and fill data into Details 4
        public ActionResult Details_four(string OrgID)
        {
            List<tblPremis> listLinked;
            
            Guid guid = new Guid(OrgID);
            listLinked = database.tblOrganisations.Where(t => t.OrgID.Equals(guid)).SingleOrDefault().tblPremises.ToList();
            

            return PartialView("~/Areas/NormalUser/Views/MaintainOrganisation/Elements/Details_four.cshtml", listLinked);
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
