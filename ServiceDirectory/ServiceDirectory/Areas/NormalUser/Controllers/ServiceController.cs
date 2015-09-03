using ServiceDirectory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Data.Entity.Validation;
using System.Net;
using System.IO;
namespace ServiceDirectory.Areas.NormalUser.Controllers.MaintainServices
{
    public class ServiceController : Controller
    {
        //
        // GET: /NormalUser/Service/
        private ServiceDirectoryEntities db = new ServiceDirectoryEntities();
        static int PageNumber;
        int PageSize = 15;
        static bool InActive = false;

        enum GroupReference
        {
            Contract_Outcome = 8,
            Contract_Obligation = 9,
            Service_Sub_Type = 10,
            Service_Type = 11,
            Service_Benefits_Criterion = 12,
            Service_Barriers_Criterion = 13,
            Service_Ethnicity_Criterion = 14,
            Service_Disability_Criterion = 15,
            Service_Personal_Circumstance_Criterion = 16,
            Orther_Service_Participation_Criterion = 17,
            Client_Support_Process = 18,
            Client_Outcome = 19,
            Target_Client = 20,
            Referral_Sources = 21,
            Support_Centres = 22
        }

        private List<SelectListItem> GetListReferenceData(List<tblReferenceData> listReference, List<tblReferenceData> listItem)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            foreach (var item in listItem)
            {
                SelectListItem temp = new SelectListItem { Text = item.RefValue, Value = item.RefID.ToString() };

                if (listReference != null)
                {
                    if (listReference.FindIndex(t => t.RefID.Equals(item.RefID)) != -1)
                        temp.Selected = true;
                }

                list.Add(temp);
            }

            return list;

        }
        public ActionResult Index()
        {
            return PartialView("List");
        }

        public ActionResult GetList(int page = -1)
        {
            List<tblService> list = null;

            if (InActive == true)
                list = db.tblServices.ToList();
            else
                list = db.tblServices.Where(t => t.IsActive == true).ToList();

            PageNumber = page != -1 ? page : PageNumber;
            return PartialView("Elements/ListItem", list.ToPagedList(PageNumber, PageSize));
        }

        public string GetListInActive(bool InActive)
        {
            ServiceController.InActive = InActive;
            List<tblService> list = null;

            if (InActive == true)
                list = db.tblServices.ToList();
            else
                list = db.tblServices.Where(t => t.IsActive == true).ToList();

            string url = RenderPartialViewToString(this, "~/Areas/NormalUser/Views/Service/Elements/ListItem.cshtml", list.ToPagedList(PageNumber, PageSize));

            url = url.Replace("/Service/Edit", "/NormalUser/Service/Edit");
            url = url.Replace("/Service/GetList", "/NormalUser/Service/GetList");
            return url;
        }

        public ActionResult Add()
        {
            return PartialView();
        }

        public ActionResult Edit(string ServiceID)
        {
            ViewBag.ServiceID = ServiceID;
            return PartialView();
        }


        //Tai
        // add reference data to organisation, before add - delete old reference data of organisation
        private void InsertRelationship(ref tblService model, string[] list)
        {

            if (list != null)
            {
                foreach (var item in list)
                {
                    int id = int.Parse(item);

                    tblReferenceData temp = db.tblReferenceDatas.Where(t => t.RefID == id).SingleOrDefault();
                    if (temp != null)
                        model.tblReferenceDatas.Add(temp);
                }
            }

        }

        public static string RenderPartialViewToString(Controller controller, string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = controller.ControllerContext.RouteData.GetRequiredString("action");

            controller.ViewData.Model = model;

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }

        private void RemoveRelationShip(ref tblService model)
        {

            // remove old reference of organisation (it do for edit mode)
            if (model != null && model.tblReferenceDatas.Count != 0)
            {
                foreach (var item in model.tblReferenceDatas.ToList())
                {
                    model.tblReferenceDatas.Remove(item);
                }
            }
            db.SaveChanges();

        }
        //

        private void AddReferenceData(ref tblService model, string[] list)
        {
            if (model.tblReferenceDatas.Count != 0)
            {
                foreach (var item in model.tblReferenceDatas)
                {
                    model.tblReferenceDatas.Remove(item);
                }
            }

            if (list != null)
            {
                foreach (var item in list)
                {
                    int id = int.Parse(item);

                    tblReferenceData temp = db.tblReferenceDatas.Where(t => t.RefID == id).SingleOrDefault();
                    if (temp != null)
                        model.tblReferenceDatas.Add(temp);
                }
            }
            db.tblServices.Add(model);

        }

        [HttpPost]
        public ActionResult Add_Edit_Service(tblService model,
                                            string[] listBenefitsCriterion,
                                            string[] listDisabilityCriterion,
                                            string[] listBarriersCriterion,
                                            string[] listCircumstanceCriterion,
                                            string[] listEthnicityCriterion,
                                            string[] listParticipationCriterion,

                                            string[] listSupportProcess,
                                            string[] listClientOutcome,
                                            string[] listTargetClient,
                                            string[] listReferralSources,
                                            string[] listSupportCentres,

                                            string ProgrammeID,
                                            string SubType,
                                            string ServiceType
                                        )
        {
            model.IsActive = true;
            if (model.ServiceID == -1)
            {
                int exist = db.tblServices.Where(t => t.ServiceName == model.ServiceName).ToList().Count;

                if (exist != 0)
                    return Content("Service name is existed. Please input another name");

                int id = int.Parse(SubType);
                tblReferenceData temp = db.tblReferenceDatas.Where(t => t.RefID == id).SingleOrDefault();
                model.tblReferenceDatas.Add(temp);

                id = int.Parse(ServiceType);
                temp = db.tblReferenceDatas.Where(t => t.RefID == id).SingleOrDefault();
                model.tblReferenceDatas.Add(temp);

                id = int.Parse(ProgrammeID);
                tblProgramme pro = db.tblProgrammes.Where(t => t.ProgrammeID == id).SingleOrDefault();
                model.ProgrammeID = pro.ProgrammeID;

                InsertRelationship(ref model, listBenefitsCriterion);
                InsertRelationship(ref model, listDisabilityCriterion);
                InsertRelationship(ref model, listBarriersCriterion);
                InsertRelationship(ref model, listCircumstanceCriterion);
                InsertRelationship(ref model, listEthnicityCriterion);
                InsertRelationship(ref model, listParticipationCriterion);
                InsertRelationship(ref model, listSupportProcess);
                InsertRelationship(ref model, listClientOutcome);
                InsertRelationship(ref model, listTargetClient);
                InsertRelationship(ref model, listReferralSources);
                InsertRelationship(ref model, listSupportCentres);
                //InsertRelationship(ref model, listProgramme);

                db.tblServices.Add(model);
            }
            else //edit
            {
                int exist = db.tblServices.Where(t => t.ServiceName == model.ServiceName && t.ServiceID != model.ServiceID).ToList().Count;

                if (exist != 0)
                    return Content("Service name is existed. Please input another name");

                tblService update = db.tblServices.Where(t => t.ServiceID == model.ServiceID).SingleOrDefault();

                RemoveRelationShip(ref update); // xóa cai cũ

                int id = int.Parse(SubType);
                tblReferenceData temp = db.tblReferenceDatas.Where(t => t.RefID == id).SingleOrDefault();
                update.tblReferenceDatas.Add(temp);

                id = int.Parse(ServiceType);
                temp = db.tblReferenceDatas.Where(t => t.RefID == id).SingleOrDefault();
                update.tblReferenceDatas.Add(temp);

                id = int.Parse(ProgrammeID);
                tblProgramme pro = db.tblProgrammes.Where(t => t.ProgrammeID == id).SingleOrDefault();
                update.ProgrammeID = pro.ProgrammeID;

                db.Entry(update).CurrentValues.SetValues(model);
                db.Entry(update).Property(t => t.ServiceID).IsModified = false;


                InsertRelationship(ref update, listBenefitsCriterion);
                InsertRelationship(ref update, listDisabilityCriterion);
                InsertRelationship(ref update, listBarriersCriterion);
                InsertRelationship(ref update, listCircumstanceCriterion);
                InsertRelationship(ref update, listEthnicityCriterion);
                InsertRelationship(ref update, listParticipationCriterion);
                InsertRelationship(ref update, listSupportProcess);
                InsertRelationship(ref update, listClientOutcome);
                InsertRelationship(ref update, listTargetClient);
                InsertRelationship(ref update, listReferralSources);
                InsertRelationship(ref update, listSupportCentres);
                //InsertRelationship(ref update, listProgramme);
            }

            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }


            return Content("Save service successfully!");
        }

        public ActionResult Delete(string ServiceID, int page)
        {
            int id = int.Parse(ServiceID);
            tblService delete = db.tblServices.Where(t => t.ServiceID == id).SingleOrDefault();
            string message = "";
            if (delete != null)
            {
                try
                {
                    db.tblServices.Remove(delete);
                    db.SaveChanges();
                }
                catch
                {
                    message = "This service is using. Cannot delete it!";
                }
            }

            ViewBag.Message = message;
            return GetList(page);
        }


        public ActionResult Details_1(string ServiceID = null)
        {
            int id;
            tblService model = null;
            List<tblReferenceData> listRef = null;
            if (!string.IsNullOrEmpty(ServiceID))
            {
                id = int.Parse(ServiceID);
                model = db.tblServices.Where(t => t.ServiceID == id).SingleOrDefault();

                listRef = db.tblServices.Where(s => s.ServiceID == id).SingleOrDefault().tblReferenceDatas.ToList();
            }
            else
            {
                model = new tblService();
                model.ServiceID = -1;// key to know add mode
            }

            List<SelectListItem> listSubType = new List<SelectListItem>();
            List<SelectListItem> listServiceType = new List<SelectListItem>();

            using (ServiceDirectoryEntities db = new ServiceDirectoryEntities())
            {
                List<tblReferenceData> list = db.tblReferenceDatas.Where(t => t.RefCode == (int)GroupReference.Service_Sub_Type).ToList();
                foreach (var item in list)
                {
                    SelectListItem t = new SelectListItem { Text = item.RefValue, Value = item.RefID.ToString() };
                    listSubType.Add(t);
                }

                list = db.tblReferenceDatas.Where(t => t.RefCode == (int)GroupReference.Service_Type).ToList();
                foreach (var item in list)
                {
                    SelectListItem t = new SelectListItem { Text = item.RefValue, Value = item.RefID.ToString() };
                    listServiceType.Add(t);
                }
            }
            ViewBag.listSubType = listSubType;
            ViewBag.listServicetype = listServiceType;

            return PartialView("~/Areas/NormalUser/Views/Service/Elements/Details_1.cshtml", model);
        }

        public ActionResult Details_2(string ServiceID = null)
        {

            List<SelectListItem> listBenefitsCriterion = new List<SelectListItem>();
            List<SelectListItem> listDisabilityCriterion = new List<SelectListItem>();
            List<SelectListItem> listBarriersCriterion = new List<SelectListItem>();
            List<SelectListItem> listCircumstanceCriterion = new List<SelectListItem>();
            List<SelectListItem> listEthnicityCriterion = new List<SelectListItem>();
            List<SelectListItem> listParticipationCriterion = new List<SelectListItem>();

            int id;
            List<tblReferenceData> listRef = null;
            if (!string.IsNullOrEmpty(ServiceID))
            {
                id = int.Parse(ServiceID);
                listRef = db.tblServices.Where(s => s.ServiceID == id).SingleOrDefault().tblReferenceDatas.ToList();
            }

            using (ServiceDirectoryEntities db = new ServiceDirectoryEntities())
            {
                List<tblReferenceData> list = db.tblReferenceDatas.Where(t => t.RefCode == (int)GroupReference.Service_Benefits_Criterion).ToList();
                listBenefitsCriterion = GetListReferenceData(listRef, list);

                list = db.tblReferenceDatas.Where(t => t.RefCode == (int)GroupReference.Service_Disability_Criterion).ToList();
                listDisabilityCriterion = GetListReferenceData(listRef, list);

                list = db.tblReferenceDatas.Where(t => t.RefCode == (int)GroupReference.Service_Barriers_Criterion).ToList();
                listBarriersCriterion = GetListReferenceData(listRef, list);

                list = db.tblReferenceDatas.Where(t => t.RefCode == (int)GroupReference.Service_Personal_Circumstance_Criterion).ToList();
                listCircumstanceCriterion = GetListReferenceData(listRef, list);

                list = db.tblReferenceDatas.Where(t => t.RefCode == (int)GroupReference.Service_Ethnicity_Criterion).ToList();
                listEthnicityCriterion = GetListReferenceData(listRef, list);

                list = db.tblReferenceDatas.Where(t => t.RefCode == (int)GroupReference.Orther_Service_Participation_Criterion).ToList();
                listParticipationCriterion = GetListReferenceData(listRef, list);
            }
            ViewBag.listBenefitsCriterion = listBenefitsCriterion;
            ViewBag.listDisabilityCriterion = listDisabilityCriterion;
            ViewBag.listBarriersCriterion = listBarriersCriterion;
            ViewBag.listCircumstanceCriterion = listCircumstanceCriterion;
            ViewBag.listEthnicityCriterion = listEthnicityCriterion;
            ViewBag.listParticipationCriterion = listParticipationCriterion;
            return PartialView("~/Areas/NormalUser/Views/Service/Elements/Details_2.cshtml");
        }

        public ActionResult Details_3(string ServiceID = null)
        {

            List<SelectListItem> listSupportProcess = new List<SelectListItem>();
            List<SelectListItem> listClientOutcome = new List<SelectListItem>();
            List<SelectListItem> listTargetClient = new List<SelectListItem>();
            List<SelectListItem> listReferralSources = new List<SelectListItem>();
            List<SelectListItem> listSupportCentres = new List<SelectListItem>();
            List<SelectListItem> listParticipationCriterion = new List<SelectListItem>();

            List<tblProgramme> listProgramme = new List<tblProgramme>();
            //List<tblService> listOtherService = new List<tblService>();

            int id;
            List<tblReferenceData> listRef = null;
            if (!string.IsNullOrEmpty(ServiceID))
            {
                id = int.Parse(ServiceID);
                listRef = db.tblServices.Where(s => s.ServiceID == id).SingleOrDefault().tblReferenceDatas.ToList();
            }

            using (ServiceDirectoryEntities db = new ServiceDirectoryEntities())
            {
                List<tblReferenceData> list = db.tblReferenceDatas.Where(t => t.RefCode == (int)GroupReference.Client_Support_Process).ToList();
                listSupportProcess = GetListReferenceData(listRef, list);

                list = db.tblReferenceDatas.Where(t => t.RefCode == (int)GroupReference.Client_Outcome).ToList();
                listClientOutcome = GetListReferenceData(listRef, list);

                list = db.tblReferenceDatas.Where(t => t.RefCode == (int)GroupReference.Target_Client).ToList();
                listTargetClient = GetListReferenceData(listRef, list);

                list = db.tblReferenceDatas.Where(t => t.RefCode == (int)GroupReference.Referral_Sources).ToList();
                listReferralSources = GetListReferenceData(listRef, list);

                list = db.tblReferenceDatas.Where(t => t.RefCode == (int)GroupReference.Support_Centres).ToList();
                listSupportCentres = GetListReferenceData(listRef, list);

                listProgramme = db.tblProgrammes.Where(t => t.IsActive == true).ToList();
                //listOtherService = db.tblServices.Where(t => t.IsActive == true).SkipWhile(t => t.ServiceID == id).ToList();
            }
            ViewBag.listSupportProcess = listSupportProcess;
            ViewBag.listClientOutcome = listClientOutcome;
            ViewBag.listTargetClient = listTargetClient;
            ViewBag.listReferralSources = listReferralSources;
            ViewBag.listSupportCentres = listSupportCentres;

            ViewBag.listProgramme = listProgramme;
            //ViewBag.listOtherService = listOtherService;
            return PartialView("~/Areas/NormalUser/Views/Service/Elements/Details_3.cshtml");
        }

        public ActionResult Contract(string ServiceID = null)
        {
            List<SelectListItem> listContractOutcome = new List<SelectListItem>();
            List<SelectListItem> listContractObligation = new List<SelectListItem>();

            int id;
            List<tblReferenceData> listRef = null;
            if (!string.IsNullOrEmpty(ServiceID))
            {
                id = int.Parse(ServiceID);
                listRef = db.tblServices.Where(s => s.ServiceID == id).SingleOrDefault().tblReferenceDatas.ToList();
            }

            using (ServiceDirectoryEntities db = new ServiceDirectoryEntities())
            {
                List<tblReferenceData> list = db.tblReferenceDatas.Where(t => t.RefCode == (int)GroupReference.Contract_Outcome).ToList();
                listContractOutcome = GetListReferenceData(listRef, list);

                list = db.tblReferenceDatas.Where(t => t.RefCode == (int)GroupReference.Contract_Obligation).ToList();
                listContractObligation = GetListReferenceData(listRef, list);
            }

            ViewBag.listContractOutcome = listContractOutcome;
            ViewBag.listContractObligation = listContractObligation;
            return PartialView("~/Areas/NormalUser/Views/Service/Elements/Contract.cshtml");
        }

        public ActionResult Funding()
        {
            return PartialView("~/Areas/NormalUser/Views/Service/Elements/Funding.cshtml");
        }

        public ActionResult Organisations()
        {
            return PartialView("~/Areas/NormalUser/Views/Service/Elements/Organisations.cshtml");
        }

        public ActionResult Premises()
        {
            return PartialView("~/Areas/NormalUser/Views/Service/Elements/Premises.cshtml");
        }

        public ActionResult Back(string url)
        {
            return RedirectToAction(url);
        }

    }
}
