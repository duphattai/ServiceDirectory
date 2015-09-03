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
        ServiceDirectoryEntities database = new ServiceDirectoryEntities();

        static int PageNumber;
        int PageSize = 15;
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


        // page == -1 wil load pagenumber before
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
        public ActionResult Details_One(string OrgID = null)
        {
            tblOrganisation model = new tblOrganisation();
            if (!string.IsNullOrEmpty(OrgID)) // edit
            {
                int id = int.Parse(OrgID);
                model = database.tblOrganisations.Where(t => t.OrgID == id).SingleOrDefault();
            }
            else
            {
                model.OrgID = -1; // key to know add mode or edit mode
            }

            return PartialView("Elements/Details_one", model);
        }


        // listReference is reference data of organisation checked, listItem items of fields will be display
        private List<SelectListItem> MakeSelectedCheckbox(List<tblReferenceData> listReference, List<tblReferenceData> listItem)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            foreach (var item in listItem)
            {
                SelectListItem temp = new SelectListItem { Text = item.RefValue, Value = item.RefID.ToString() };
                // edit mode and make checked
                if (listReference.FindIndex(t => t.RefID.Equals(item.RefID)) != -1)
                    temp.Selected = true;

                list.Add(temp);
            }

            return list;
        }


        // get data and fill data into Details 2
        // if OrgId == "" add mode, otherwise edit mode
        public ActionResult Details_two(string OrgID = null)
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
            List<tblReferenceData> listReference = new List<tblReferenceData>();
            if (!string.IsNullOrEmpty(OrgID))
            {
                id = int.Parse(OrgID);
                listReference = database.tblOrganisations.Where(t => t.OrgID == id).SingleOrDefault().tblReferenceDatas.ToList();
            }


                
            // get list checkbox from reference of details 2
            List<tblReferenceData> temp = database.tblReferenceDatas.Where(org => org.RefCode == (int)GroupReference.organisation_specicalism).ToList();
            listOrgSpec = MakeSelectedCheckbox(listReference, temp); // make each item checked

            // get list checkbox of service personal....
            temp = database.tblReferenceDatas.Where(org => org.RefCode == (int)GroupReference.accreditation).ToList();
            listSerPers = MakeSelectedCheckbox(listReference, temp);

            // get list checkbox of service disabilities....
            temp = database.tblReferenceDatas.Where(org => org.RefCode == (int)GroupReference.service_disabilities_capabilities).ToList();
            listSerDis = MakeSelectedCheckbox(listReference, temp);

            // get list checkbox of service ethnicity....
            temp = database.tblReferenceDatas.Where(org => org.RefCode == (int)GroupReference.service_ethnicity_capabilities).ToList();
            listSerEth = MakeSelectedCheckbox(listReference, temp);

            // get list checkbox of service barriers
            temp = database.tblReferenceDatas.Where(org => org.RefCode == (int)GroupReference.service_barriers_capabilities).ToList();
            listSerBarri = MakeSelectedCheckbox(listReference, temp);

            // get list checkbox of  service benifit
            temp = database.tblReferenceDatas.Where(org => org.RefCode == (int)GroupReference.service_benefits_capabilities).ToList();
            listSerBene = MakeSelectedCheckbox(listReference, temp);

            // get list checkbox of Accreditation
            temp = database.tblReferenceDatas.Where(org => org.RefCode == (int)GroupReference.accreditation).ToList();
            listAccr = MakeSelectedCheckbox(listReference, temp);
                

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
        public ActionResult Details_three(string OrgID = null)
        {
            using (ServiceDirectoryEntities database = new ServiceDirectoryEntities())
            {
                int id;
                List<SelectListItem> listSer = new List<SelectListItem>();
                List<SelectListItem> listProg = new List<SelectListItem>();

                List<tblOrganisationService> listOrgLinkedSer = null;
                List<tblProgramme> listOrgLinkedProg = null;
                if (!string.IsNullOrEmpty(OrgID))
                {
                    id = int.Parse(OrgID);
                    // get service linked with organisation 
                    listOrgLinkedSer = database.tblOrganisationServices.Where(t => t.OrgID == id).ToList();
                    tblOrganisation linked = database.tblOrganisations.Where(t => t.OrgID == id).SingleOrDefault();
                    listOrgLinkedProg = linked.tblProgrammes.ToList(); // get linked programme with organisation
                }

                // create checkbox item and make it checked
                foreach (var item in database.tblServices.ToList())
                {
                    listSer.Add(new SelectListItem { Text = item.ServiceName, Value = item.ServiceID.ToString() });

                    if (!string.IsNullOrEmpty(OrgID) && listOrgLinkedSer.FindIndex(t => t.ServiceID == item.ServiceID) != -1)
                    {
                        listSer[listSer.Count - 1].Selected = true;
                    }
                }


                foreach (var item in database.tblProgrammes.ToList())
                {
                    listProg.Add(new SelectListItem { Text = item.ProgrammeName, Value = item.ProgrammeID.ToString() });

                    if (!string.IsNullOrEmpty(OrgID) && listOrgLinkedProg.FindIndex(t => t.ProgrammeID == item.ProgrammeID) != -1)
                    {
                        listProg[listProg.Count - 1].Selected = true;
                    }
                }

                ViewBag.listProg = listProg;
                ViewBag.listSer = listSer;

                return PartialView("Elements/Details_three");
            }
        }

        // get data and fill data into Details 4
        public ActionResult Details_four(string OrgID)
        {
            using (ServiceDirectoryEntities database = new ServiceDirectoryEntities())
            {
                List<tblPremis> listLinked;

                int id = int.Parse(OrgID);
                listLinked = database.tblOrganisations.Where(t => t.OrgID == id).SingleOrDefault().tblPremises.ToList();

                return PartialView("Elements/Details_four", listLinked);
            }
        }


        // show Listitem include in-active
        public string GetListOrganisationsFromCheckbox(bool IncludeInActive)
        {
            using (ServiceDirectoryEntities database = new ServiceDirectoryEntities())
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
        }



        // add reference data to organisation, before add - delete old reference data of organisation
        private void InsertRelationshipReferenceData(ref tblOrganisation model, string[]  list)
        {
            
            if (list != null)
            {
                foreach (var item in list)
                {
                    int id = int.Parse(item);

                    tblReferenceData temp = database.tblReferenceDatas.Where(t => t.RefID == id).SingleOrDefault();
                    if (temp != null)
                        model.tblReferenceDatas.Add(temp);
                }
            }
            
        }

        private void InsertRelationshipProgramme(ref tblOrganisation model, string[]  list)
        {
            if (list != null)
            {
                foreach (var item in list)
                {
                    int id = int.Parse(item);

                    tblProgramme temp = database.tblProgrammes.Where(t => t.ProgrammeID == id).SingleOrDefault();
                    if (temp != null)
                        model.tblProgrammes.Add(temp);
                }
            }
        }

        private void InsertRelationshipService(ref tblOrganisation model, string[] list)
        {
            if (list != null)
            {
                foreach (var item in list)
                {
                    int id = int.Parse(item);
                    tblOrganisationService temp = new tblOrganisationService { OrgID = model.OrgID, ServiceID = id };
                    database.tblOrganisationServices.Add(temp);
                }
            }
        }


        // remove relationship many - many between organisation - reference data
        //                     many - many between organisation - programme
        //                     many - many between organisation - service
        private void RemoveRelationShip(ref tblOrganisation model)
        {
            // remove old reference of organisation (it do for edit mode)
            if (model != null)
            {
                // delete many to many relationship between organisation - referennce data
                if (model.tblReferenceDatas.Count != 0)
                {
                    foreach (var item in model.tblReferenceDatas.ToList())
                    {
                        model.tblReferenceDatas.Remove(item);
                    }
                }

                // delete many to many relationship between organisation - programme
                if (model.tblProgrammes.Count != 0)
                {
                    foreach (var item in model.tblProgrammes.ToList())
                    {
                        model.tblProgrammes.Remove(item);
                    }
                }

                // delete many to many relationship between organisation - service
                int id = model.OrgID;
                List<tblOrganisationService> list = database.tblOrganisationServices.Where(t => t.OrgID == id).ToList();
                if (list.Count != 0)
                {
                    foreach (var item in list)
                    {
                        database.tblOrganisationServices.Remove(item);
                    }
                }
            }
            database.SaveChanges();
        }

        // listSerEth get checked list from Service Ethnicity Capabilities
        // listSerDis get checked list from Service Disabilities Capabilities
        // listOrgSpec get checked list .........

        [HttpPost]
        public ActionResult InsertUpdate_Organisation(tblOrganisation model, string[] listSerEth, string[] listSerDis, string[] listOrgSpec, string[] listSerPers, string[] listSerBarri, string[] listAccr, string[] listSerBene, string[] listSer, string[] listProg, string Expression_of_Interest)
        {
            model.IsActive = true;
            if (model.OrgID == -1) // add mode
            {
                // check organisation name
                int exists = database.tblOrganisations.Where(t => t.OrgName == model.OrgName).ToList().Count;

                if (exists != 0) // org name exist
                    return Content("Organisation name is existed. Please input another name!");

                // add reference data into organisation
                InsertRelationshipReferenceData(ref model, listSerEth);
                InsertRelationshipReferenceData(ref model, listSerDis);
                InsertRelationshipReferenceData(ref model, listOrgSpec);
                InsertRelationshipReferenceData(ref model, listSerPers);
                InsertRelationshipReferenceData(ref model, listSerBarri);
                InsertRelationshipReferenceData(ref model, listAccr);
                InsertRelationshipReferenceData(ref model, listSerBene);


                // if Expression_of_Interest checked, organisation will insert programme and service 
                if (!string.IsNullOrEmpty(Expression_of_Interest)) // it equal "on" will checked
                {
                    InsertRelationshipProgramme(ref model, listProg);
                    InsertRelationshipService(ref model, listSer);
                }

                
                if (model.Preferred == null) model.Preferred = false;

                database.tblOrganisations.Add(model);
            }
            else
            {
                int exists = database.tblOrganisations.Where(t => t.OrgName == model.OrgName && t.OrgID != model.OrgID).ToList().Count;

                if (exists != 0) // org name exist
                    return Content("Organisation name is existed. Please input another name!");

                tblOrganisation update = database.tblOrganisations.Where(t => t.OrgID == model.OrgID).SingleOrDefault();

                // add reference data into organisation
                RemoveRelationShip(ref update);

                InsertRelationshipReferenceData(ref update, listSerEth);
                InsertRelationshipReferenceData(ref update, listSerDis);
                InsertRelationshipReferenceData(ref update, listOrgSpec);
                InsertRelationshipReferenceData(ref update, listSerPers);
                InsertRelationshipReferenceData(ref update, listSerBarri);
                InsertRelationshipReferenceData(ref update, listAccr);
                InsertRelationshipReferenceData(ref update, listSerBene);

                if (!string.IsNullOrEmpty(Expression_of_Interest)) // it equal "on" will checked
                {
                    InsertRelationshipProgramme(ref update, listProg);
                    InsertRelationshipService(ref update, listSer);
                }

                database.Entry(update).CurrentValues.SetValues(model);
                database.Entry(update).Property(t => t.OrgID).IsModified = false;
                if (model.Preferred == null)
                    model.Preferred = false;
            }

            try
            {
                database.SaveChanges();
            }
            catch (Exception ex)
            {
                return Content("Cannot save organisation!");
            }

            return Content("Save organisation successfully");
            
        }


        public ActionResult Delete_ActionLink(string OrgID, int page)
        {
            int id = int.Parse(OrgID);
            tblOrganisation delete = database.tblOrganisations.Where(t => t.OrgID == id).SingleOrDefault();
            string message = "";
            if (delete != null)
            {
                try 
                {
                    database.tblOrganisations.Remove(delete);
                    database.SaveChanges();
                }
                catch
                {
                    message = "This organisation is using. Cannot delete it!";
                }
            }

            ViewBag.Message = message;
            return GetListOrganisations(page);
        }
    }
}
