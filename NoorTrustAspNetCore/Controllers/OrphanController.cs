
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using NoorTrust.DonationFund.Api.Services;
using NoorTrust.DonationFund.Api.Models;
using NoorTrust.DonationFund.WebUi;
using NoorTrust.DonationFund.Api;
using NoorTrust.DonationFund.Api.DataAccess;
using NoorTrust.DataAccess;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Microsoft.EntityFrameworkCore;
using NoorTrust.DonationFund.WebUi.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace NoorTrust.DonationFund.WebUI.Controllers
{
    [Authorize(Roles = "Staff,Admin,Volunteer")]
    // [Authorize(Roles = SecurityConstants.RoleName_Admin)]
    public class OrphanController : Controller
    {
     
        private const int ID_FOR_CREATE_NEW_PRESIDENT = 0;
        private IOrphanService _OrphanService;
        private IReportService _ReportService;

        private ILogger _Log;
     
        public IHostingEnvironment _HostingEnvironment { get; set; }


        public OrphanController(IOrphanService Service,
            IReportService ReportService,
            ILogger Log,
            IHostingEnvironment hostingEnvironment
          
            )
        {
            if (Service == null)
                throw new ArgumentNullException("service", "service is null.");

            _OrphanService = Service;
            _ReportService = ReportService;
            _Log = Log;
            _HostingEnvironment = hostingEnvironment;

         
        }
        public IActionResult Index()
        {
            BuildViewData(ViewData);
            return View();
        }


        [HttpPost]
        public ActionResult Index(int[] productID, int reportID)
        {
            string list = "";
            //using (var OrphanViewModel = new OrphanViewModel())

            if (productID != null && productID.Count() > 0)
            {
                var selectedOrphans = (from product in _OrphanService.GetOrphans()
                                       where productID.Contains(product.Id)
                                       select product).ToList();

                ViewData["selectedOrphans"] = selectedOrphans;
                if (selectedOrphans.Count > 0)
                {
                    foreach (var s in selectedOrphans)
                        list = list + s.Id.ToString() + ",";

                    // list.LastIndexOf(",")

                    list = list.Remove(list.LastIndexOf(","));
                }

                var dic = _ReportService.GetReportFileNameAndFolderById(reportID);

                var filename = dic.Keys.First();
                var foldername = dic.Values.First();

                return RedirectToAction("DisplayReport", "ReportView", new { filename = filename, fname = foldername, list = list });


            }

            return View();
        }


        public void BuildViewData(ViewDataDictionary viewDataDictionary)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (Report p in _ReportService.GetReports().Where(x => x.ReportListType == ReportListType.Orphans && x.IsActive == true))
            {
                items.Add(new SelectListItem { Value = p.Id.ToString(), Text = p.ReportName });
            }
            IEnumerable<SelectListItem> iitems = items;
         
            var selectList = new SelectList(iitems, "Value", "Text");
            //  ViewBag.SelectedReport = selectList;
            ViewData["SelectedReport"] = selectList;
          
        }
        public async Task<IActionResult> Orphans_ReadAsync([DataSourceRequest]DataSourceRequest request)
        {
            DataSourceResult result = null;
            try
            {

                var orphans = await _OrphanService.GetOrphansAsync();
                result = orphans.ToDataSourceResult(request, orphan => new OrphanViewModel
                {
                    Id = orphan.Id,
                    SponsorId = orphan.SponsorId,
                    Title = orphan.Title,
                    FirstName = orphan.FirstName,
                    FatherName = orphan.FatherName,
                    GrandFather = orphan.GrandFather,
                    LastName = orphan.LastName,
                    ArTitle = orphan.ArTitle,
                    ArFirstName = orphan.ArFirstName,
                    ArFatherName = orphan.ArFatherName,
                    ArGrandFather = orphan.ArGrandFather,
                    ArLastName = orphan.ArLastName,
                    MotherName = orphan.MotherName,
                    ArMotherName = orphan.ArMotherName,

                    OrphanFileId = !orphan.OrphanFileId.HasValue ? 0 : orphan.OrphanFileId,
                    //   OrphanFileName= files.Where(x => x.Value == null).FirstOrDefault() == null? "" :    files.Where(x=>x.Value==orphan.Id.ToString()).FirstOrDefault().Text,
                    Gender = orphan.Gender,
                    DOB = orphan.DOB,
                    BirthPlace = orphan.BirthPlace,

                    FatherPrevWork = orphan.FatherPrevWork,
                    FatherDeath = orphan.FatherDeath,
                    MotherWork = orphan.MotherWork,
                    IsMotherDead = orphan.IsMotherDead,

                    EducationLevel = orphan.EducationLevel,
                    EducationStatus = orphan.EducationStatus,
                    AcademicYear = orphan.AcademicYear,

                    Guardian = orphan.Guardian,
                    GuardianWork = orphan.GuardianWork,
                    GuardianName = orphan.GuardianName,
                    Hobbies = orphan.Hobbies,

                    IsSayed = orphan.IsSayed,
                    Ethnicity = orphan.Ethnicity,

                    Address = orphan.Address,
                    CityId = orphan.CityId,
                    DistrictId = orphan.DistrictId,
                    CountryId = orphan.CountryId,

                    OrphanedDate = orphan.OrphanedDate,
                    RegistarDate = orphan.RegistarDate,
                    SponsoredDate = orphan.SponsoredDate,

                    IsDisabled = orphan.IsDisabled,
                    IsStudent = orphan.IsStudent,
                    IsException = orphan.IsException,
                    ExceptionReason = orphan.ExceptionReason,
                    NoBrothers = orphan.NoBrothers,
                    NoSisters = orphan.NoSisters,
                    NoSiblings = orphan.NoSiblings,

                    LivingSituation = orphan.LivingSituation,
                    Notes = orphan.Notes,
                    IsApproved = orphan.IsApproved,
                    IsActive = orphan.IsActive,
                    DeactivatedDate = orphan.DeactivatedDate,
                    LastUpdated = orphan.LastUpdated,
                    LastUpdatedBy = orphan.LastUpdatedBy,
                    PortfoliofileImagepath = GetPortfolioImage(orphan.Id, orphan.Gender),
                    ThumbProfileImagepath = GetPortfolioThumbImage(orphan.Id, orphan.Gender),
                    UploadInitialFilesList = GenerateOrphanFileList(orphan.Id),
                });
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);

            }
            return Json(result);
        }

      
        private IList<OrphanFile> GenerateOrphanFileList(int id)
        {
            var result = _OrphanService.GetOrphanFiles(id);//.Where(x => x.OrphanId == id).ToList();

            return (result == null) ? new List<OrphanFile>() : result;
        }

        public IActionResult Orphans_ReadById([DataSourceRequest]DataSourceRequest request, int id)
        {
            var orphans = _OrphanService.GetOrphansById(id);
            DataSourceResult result = orphans.ToDataSourceResult(request, orphan => new OrphanViewModel
            {
                Id = orphan.Id,
                SponsorId = orphan.SponsorId,
                Title = orphan.Title,
                FirstName = orphan.FirstName,
                FatherName = orphan.FatherName,
                GrandFather = orphan.GrandFather,
                LastName = orphan.LastName,
                ArTitle = orphan.ArTitle,
                ArFirstName = orphan.ArFirstName,
                ArFatherName = orphan.ArFatherName,
                ArGrandFather = orphan.ArGrandFather,
                ArLastName = orphan.ArLastName,
                MotherName = orphan.MotherName,
                ArMotherName = orphan.ArMotherName,

                //   OrphanFileId = !orphan.OrphanFileId.HasValue ? 0 : orphan.OrphanFileId,
                Gender = orphan.Gender,
                DOB = orphan.DOB,
                BirthPlace = orphan.BirthPlace,

                FatherPrevWork = orphan.FatherPrevWork,
                FatherDeath = orphan.FatherDeath,
                MotherWork = orphan.MotherWork,
                IsMotherDead = orphan.IsMotherDead,

                EducationLevel = orphan.EducationLevel,
                EducationStatus = orphan.EducationStatus,
                AcademicYear = orphan.AcademicYear,

                Guardian = orphan.Guardian,
                GuardianWork = orphan.GuardianWork,
                GuardianName = orphan.GuardianName,
                Hobbies = orphan.Hobbies,

                IsSayed = orphan.IsSayed,
                Ethnicity = orphan.Ethnicity,

                Address = orphan.Address,
                CityId = orphan.CityId,
                DistrictId = orphan.DistrictId,
                CountryId = orphan.CountryId,

                OrphanedDate = orphan.OrphanedDate,
                RegistarDate = orphan.RegistarDate,
                SponsoredDate = orphan.SponsoredDate,

                IsDisabled = orphan.IsDisabled,
                IsStudent = orphan.IsStudent,
                IsException = orphan.IsException,
                ExceptionReason = orphan.ExceptionReason,
                NoBrothers = orphan.NoBrothers,
                NoSisters = orphan.NoSisters,
                NoSiblings = orphan.NoSiblings,

                LivingSituation = orphan.LivingSituation,
                Notes = orphan.Notes,
                IsApproved = orphan.IsApproved,
                IsActive = orphan.IsActive,
                DeactivatedDate = orphan.DeactivatedDate,
                LastUpdated = orphan.LastUpdated,
                LastUpdatedBy = orphan.LastUpdatedBy,
                //   PortfoliofileImagepath = GetPortfolioImage(orphan.Id, !orphan.OrphanFileId.HasValue ? 0 : orphan.OrphanFileId.Value, orphan.Gender)
            });

            return Json(result);
        }



        public ActionResult Orders_Read([DataSourceRequest]DataSourceRequest request)
        {
            var result = Enumerable.Range(0, 50).Select(i => new DisplayViewModel
            {
                OrderID = i,
                Freight = i * 10,
                OrderDate = new DateTime(2016, 9, 15).AddDays(i % 7),
                ShipName = "ShipName " + i,
                ShipCity = "ShipCity " + i
            });

            var dsResult = result.ToDataSourceResult(request);
            return Json(dsResult);
        }

        [HttpPost]

        public ActionResult Orphans_AssignToSponsor(string id)
        {
            if (!String.IsNullOrEmpty(id) && id.Split("|")[0] != "")
            {
                var sponsorId = id.Split("|")[1];

                var entity = _OrphanService.GetOrphanById(Int32.Parse(id.Split("|")[0]));
                if (entity != null)
                {
                    entity.SponsorId = Int32.Parse(sponsorId);
                    entity.SponsoredDate = DateTime.Now;
                    _OrphanService.Save(entity);
                }
            }
            return View();
        }
        [HttpPost]

        public ActionResult Orphans_AddOrphanActivity([DataSourceRequest]DataSourceRequest request, OrphanActivityViewModel orphan, string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                var entity = new OrphanActivity();


                entity.OrphanId = Int32.Parse(id);
                entity.PostedDate = DateTime.Now;
                entity.Author = User.Identity.Name;
                _OrphanService.SaveOrphanActivity(entity);


                _Log.LogUsage("Add new OrphanActivity, Id: " + entity.Id);


            }

             return Json(new[] { orphan }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult Orphans_RemoveFromSponsor([DataSourceRequest]DataSourceRequest request, int id)
        {
            var entity = _OrphanService.GetOrphanById(id);
            if (entity != null)
            {
                entity.SponsorId = null;
                entity.SponsoredDate = null;
                _OrphanService.Save(entity);
            }
            return View();
        }


        [HttpPost]
        //[AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Orphans_Create([DataSourceRequest]DataSourceRequest request, Orphan orphan)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var entity = new Orphan
                    {

                        Id = orphan.Id,
                        SponsorId = orphan.SponsorId,
                        Title = orphan.Title,
                        FirstName = orphan.FirstName,
                        FatherName = orphan.FatherName,
                        GrandFather = orphan.GrandFather,
                        LastName = orphan.LastName,
                        ArTitle = orphan.ArTitle,
                        ArFirstName = orphan.ArFirstName,
                        ArFatherName = orphan.ArFatherName,
                        ArGrandFather = orphan.ArGrandFather,
                        ArLastName = orphan.ArLastName,
                        MotherName = orphan.MotherName,
                        ArMotherName = orphan.ArMotherName,

                        OrphanFileId = !orphan.OrphanFileId.HasValue ? 0 : orphan.OrphanFileId,
                        Gender = orphan.Gender,
                        DOB = orphan.DOB,
                        BirthPlace = orphan.BirthPlace,

                        FatherPrevWork = orphan.FatherPrevWork,
                        FatherDeath = orphan.FatherDeath,
                        MotherWork = orphan.MotherWork,
                        IsMotherDead = orphan.IsMotherDead,

                        EducationLevel = orphan.EducationLevel,
                        EducationStatus = orphan.EducationStatus,
                        AcademicYear = orphan.AcademicYear,

                        Guardian = orphan.Guardian,
                        GuardianWork = orphan.GuardianWork,
                        GuardianName = orphan.GuardianName,
                        Hobbies = orphan.Hobbies,

                        IsSayed = orphan.IsSayed,
                        Ethnicity = orphan.Ethnicity,

                        Address = orphan.Address,
                        CityId = orphan.CityId,
                        DistrictId = orphan.DistrictId,
                        CountryId = orphan.CountryId,

                        OrphanedDate = orphan.OrphanedDate,
                        RegistarDate = orphan.RegistarDate,
                        SponsoredDate = orphan.SponsoredDate,

                        IsDisabled = orphan.IsDisabled,
                        IsStudent = orphan.IsStudent,
                        IsException = orphan.IsException,
                        ExceptionReason = orphan.ExceptionReason,
                        NoBrothers = orphan.NoBrothers,
                        NoSisters = orphan.NoSisters,
                        NoSiblings = orphan.NoSiblings,

                        LivingSituation = orphan.LivingSituation,
                        Notes = orphan.Notes,
                        IsApproved = orphan.IsApproved,
                        IsActive = orphan.IsActive,
                        DeactivatedDate = orphan.DeactivatedDate,
                        LastUpdated = DateTime.Now,
                        LastUpdatedBy = orphan.LastUpdatedBy
                    };
                    if (entity.CountryId == null) entity.CountryId = 2;

                    _OrphanService.Save(entity);
                    //db.SaveChanges();
                    orphan.Id = entity.Id;

                    _Log.LogUsage("Add new Orphan, Id: " + orphan.Id);

                }
                catch (Exception ex)
                {

                    ErrorSignal.FromCurrentContext().Raise(ex);
                }
            }

            return Json(new[] { orphan }.ToDataSourceResult(request, ModelState));
        }

        // [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public ActionResult Orphans_Update([DataSourceRequest]DataSourceRequest request, Orphan orphan)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var entity = new Orphan
                    {
                        Id = orphan.Id,
                        SponsorId = orphan.SponsorId,
                        Title = orphan.Title,
                        FirstName = orphan.FirstName,
                        FatherName = orphan.FatherName,
                        GrandFather = orphan.GrandFather,
                        LastName = orphan.LastName,
                        ArTitle = orphan.ArTitle,
                        ArFirstName = orphan.ArFirstName,
                        ArFatherName = orphan.ArFatherName,
                        ArGrandFather = orphan.ArGrandFather,
                        ArLastName = orphan.ArLastName,
                        MotherName = orphan.MotherName,
                        ArMotherName = orphan.ArMotherName,

                        OrphanFileId = !orphan.OrphanFileId.HasValue ? 0 : orphan.OrphanFileId,
                        Gender = orphan.Gender,
                        DOB = orphan.DOB,
                        BirthPlace = orphan.BirthPlace,

                        FatherPrevWork = orphan.FatherPrevWork,
                        FatherDeath = orphan.FatherDeath,
                        MotherWork = orphan.MotherWork,
                        IsMotherDead = orphan.IsMotherDead,

                        EducationLevel = orphan.EducationLevel,
                        EducationStatus = orphan.EducationStatus,
                        AcademicYear = orphan.AcademicYear,

                        Guardian = orphan.Guardian,
                        GuardianWork = orphan.GuardianWork,
                        GuardianName = orphan.GuardianName,
                        Hobbies = orphan.Hobbies,

                        IsSayed = orphan.IsSayed,
                        Ethnicity = orphan.Ethnicity,

                        Address = orphan.Address,
                        CityId = orphan.CityId,
                        DistrictId = orphan.DistrictId,
                        CountryId = orphan.CountryId,

                        OrphanedDate = orphan.OrphanedDate,
                        RegistarDate = orphan.RegistarDate,
                        SponsoredDate = orphan.SponsoredDate,

                        IsDisabled = orphan.IsDisabled,
                        IsStudent = orphan.IsStudent,
                        IsException = orphan.IsException,
                        ExceptionReason = orphan.ExceptionReason,
                        NoBrothers = orphan.NoBrothers,
                        NoSisters = orphan.NoSisters,
                        NoSiblings = orphan.NoSiblings,

                        LivingSituation = orphan.LivingSituation,
                        Notes = orphan.Notes,
                        IsApproved = orphan.IsApproved,
                        IsActive = orphan.IsActive,
                        DeactivatedDate = orphan.DeactivatedDate,
                        LastUpdated = DateTime.Now,
                        LastUpdatedBy = orphan.LastUpdatedBy

                        // !orphan.CityId.HasValue ? "" : _orphanRepository.GetCityName((int)orphan.CityId)
                        // CitySelected = !orphan.CityId.HasValue ? "" : _orphanRepository.GetCityName((int)orphan.CityId),

                        //  ProfileImagepath = _orphanRepository.GetPortfolioImage(orphan.OrphanId, !orphan.OrphanFileId.HasValue ? 0 : orphan.OrphanFileId.Value, orphan.Gender),
                        //  ThumbProfileImagepath = _orphanRepository.GetPortfolioThumbImage(orphan.OrphanId, !orphan.OrphanFileId.HasValue ? 0 : orphan.OrphanFileId.Value, orphan.Gender),

                        // EducationStatus = !orphan.EducationStatusId.HasValue ? "" : _orphanRepository.GetEducationStatusName((int)orphan.EducationStatusId),




                    };

                    _OrphanService.Save(entity);
                    //db.Entry(entity).State = EntityState.Modified;
                    //db.SaveChanges();

                    _Log.LogUsage("Update Orphan, Id: " + orphan.Id);

                }
                catch (Exception ex)
                {

                    ErrorSignal.FromCurrentContext().Raise(ex);
                }
            }

            return Json(new[] { orphan }.ToDataSourceResult(request, ModelState));
        }

        //  [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public ActionResult Orphans_Destroy([DataSourceRequest]DataSourceRequest request, Orphan orphan)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var entity = new Orphan
                    {
                        Id = orphan.Id,
                        SponsorId = orphan.SponsorId,
                        Title = orphan.Title,
                        FirstName = orphan.FirstName,
                        FatherName = orphan.FatherName,
                        GrandFather = orphan.GrandFather,
                        LastName = orphan.LastName,
                        ArTitle = orphan.ArTitle,
                        ArFirstName = orphan.ArFirstName,
                        ArFatherName = orphan.ArFatherName,
                        ArGrandFather = orphan.ArGrandFather,
                        ArLastName = orphan.ArLastName,
                        MotherName = orphan.MotherName,
                        ArMotherName = orphan.ArMotherName,

                        OrphanFileId = !orphan.OrphanFileId.HasValue ? 0 : orphan.OrphanFileId,
                        Gender = orphan.Gender,
                        DOB = orphan.DOB,
                        BirthPlace = orphan.BirthPlace,

                        FatherPrevWork = orphan.FatherPrevWork,
                        FatherDeath = orphan.FatherDeath,
                        MotherWork = orphan.MotherWork,
                        IsMotherDead = orphan.IsMotherDead,

                        EducationLevel = orphan.EducationLevel,
                        EducationStatus = orphan.EducationStatus,
                        AcademicYear = orphan.AcademicYear,

                        Guardian = orphan.Guardian,
                        GuardianWork = orphan.GuardianWork,
                        GuardianName = orphan.GuardianName,
                        Hobbies = orphan.Hobbies,

                        IsSayed = orphan.IsSayed,
                        Ethnicity = orphan.Ethnicity,

                        Address = orphan.Address,
                        CityId = orphan.CityId,
                        DistrictId = orphan.DistrictId,
                        CountryId = orphan.CountryId,

                        OrphanedDate = orphan.OrphanedDate,
                        RegistarDate = orphan.RegistarDate,
                        SponsoredDate = orphan.SponsoredDate,

                        IsDisabled = orphan.IsDisabled,
                        IsStudent = orphan.IsStudent,
                        IsException = orphan.IsException,
                        ExceptionReason = orphan.ExceptionReason,
                        NoBrothers = orphan.NoBrothers,
                        NoSisters = orphan.NoSisters,
                        NoSiblings = orphan.NoSiblings,

                        LivingSituation = orphan.LivingSituation,
                        Notes = orphan.Notes,
                        IsApproved = orphan.IsApproved,
                        IsActive = orphan.IsActive,
                        DeactivatedDate = orphan.DeactivatedDate,
                        LastUpdated = DateTime.Now,
                        LastUpdatedBy = orphan.LastUpdatedBy
                    };

                    _OrphanService.Save(entity);
                    // db.Orphans.Remove(entity);
                    // db.SaveChanges();


                    _Log.LogUsage("Delete Orphan, Id: " + orphan.Id);

                }
                catch (Exception ex)
                {

                    ErrorSignal.FromCurrentContext().Raise(ex);
                }
            }

            return Json(new[] { orphan }.ToDataSourceResult(request, ModelState));
        }


        //-------------------------------------------------


        public IActionResult OrphanActivity_ReadById([DataSourceRequest]DataSourceRequest request, int id)
        {

            var orphans = _OrphanService.GetOrphanActivitiesByOrphanId(id);

            DataSourceResult result = orphans.ToDataSourceResult(request, orphan => new OrphanActivityViewModel
            {
                Id = orphan.Id,
                OrphanId=orphan.OrphanId,
                PostedDate = orphan.PostedDate,
                ActivityName = orphan.ActivityName,
                Description = orphan.Description,
                Author = orphan.Author

            });



            return Json(result);

        }

        //not useds
        [HttpPost]
       
        public ActionResult OrphanActivity_CreateById([DataSourceRequest]DataSourceRequest request, OrphanActivityViewModel orphan, int id)
        {

        //    id = Int32.Parse(HttpContext.Session.GetString("aa"));



            if (ModelState.IsValid)
            {
                try
                {
                    var entity = new OrphanActivity
                    {

                        // Id = orphan.Id,
                        OrphanId = id,
                        PostedDate = orphan.PostedDate,
                        ActivityName = orphan.ActivityName,
                        Description = orphan.Description,
                        Author = User.Identity.Name
                    };

                    _OrphanService.SaveOrphanActivity(entity);

                    orphan.Id = entity.Id;

                    _Log.LogUsage("Add new OrphanActivity, Id: " + orphan.Id);

                }
                catch (Exception ex)
                {

                    ErrorSignal.FromCurrentContext().Raise(ex);
                }
            }

            return Json(new[] { orphan }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult OrphanActivity_UpdateById([DataSourceRequest]DataSourceRequest request, OrphanActivityViewModel orphanActivity, int id)
        {
            var modelStateErrors = this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors);

            if (modelStateErrors.Count() > 0)
            {
                foreach (var ert in modelStateErrors)
                {
                    ModelState.AddModelError("error -", ert.ErrorMessage);
                }
            }


            if (ModelState.IsValid)
            {
                //  var donations = _Service.GetDonationsById(id);

                var entity = _OrphanService.GetOrphanActivitiesById(id);

                entity.Id = orphanActivity.Id;
                entity.OrphanId = orphanActivity.OrphanId;
                entity.Description = orphanActivity.Description;
                entity.PostedDate = orphanActivity.PostedDate;
                entity.ActivityName = orphanActivity.ActivityName;
          //      entity.Author = orphanActivity.Author;
               
                _OrphanService.SaveOrphanActivity(entity);

            }

            return Json(new[] { orphanActivity }.ToDataSourceResult(request, ModelState));
        }
        [HttpDelete]
        public ActionResult OrphanActivity_DestroyById([DataSourceRequest]DataSourceRequest request, OrphanActivityViewModel orphanActivity, int id)
        {
            if (ModelState.IsValid)
            {
                try
                {


                    _OrphanService.RemoveOrphanActivityById(id);


                    _Log.LogUsage("delete SponsorActivity, Id: " + id);

                }
                catch (Exception ex)
                {

                    ErrorSignal.FromCurrentContext().Raise(ex);
                }
                // db.Sponsors.Remove(entity);
                // db.SaveChanges();
            }

            return Json(new[] { orphanActivity }.ToDataSourceResult(request, ModelState));
        }



        //----------------------------------------------------
        public async Task<JsonResult> ServerFiltering_GetOrphans(string text)
        {
            var donors = await _OrphanService.GetOrphansAsync();//.Where(x => x.IsActive == true);


            //var products = db.Select(product => new DonorViewModel
            //{
            //    ProductID = product.ProductID,
            //    ProductName = product.ProductName,
            //    UnitPrice = product.UnitPrice ?? 0,
            //    UnitsInStock = product.UnitsInStock ?? 0,
            //    UnitsOnOrder = product.UnitsOnOrder ?? 0,
            //    Discontinued = product.Discontinued
            //});

            var list = donors.Select(ct => new SelectListItem
            {
                Text = "ID: " + ct.Id + " " + ct.Age + " " + ct.ArGrandFather + " " + " " + ct.ArFatherName + " " + ct.ArFirstName,
                Value = ct.Id.ToString(),
                Selected = false
            });



            if (!string.IsNullOrEmpty(text))
            {
                list = list.Where(p => p.Text.Contains(text));
            }

            return Json(list);//, JsonRequestBehavior.AllowGet);
        }


        //-------------------------------------------------------------------------------

        public string GetPortfolioImage(int orphanid, bool gender)
        {
            string result = "";
            //  string FileURL = @"/assets/orphans/Original/";
            OrphanFile OrphanFile = null;



            var orphanfileid = _OrphanService.GetOrphans().Where(x => x.Id == orphanid).FirstOrDefault().OrphanFileId;

            if (orphanfileid.HasValue)
            {

                OrphanFile = _OrphanService.GetOrphanFiles().Where(x => x.Id == orphanfileid.Value).FirstOrDefault();
                if (OrphanFile != null)
                    result = OrphanFile.FileURL + OrphanFile.FileName + OrphanFile.FileType;
            }
            if (OrphanFile == null)
            {
                OrphanFile OrphanFile2 = new OrphanFile();

                OrphanFile2.Id = 2;
                if (gender)
                    OrphanFile2.FileName = System.Guid.Parse("70f1825c-3e3a-4afc-82b4-80051b808d72");
                else
                    OrphanFile2.FileName = System.Guid.Parse("70f1825c-3e3a-4afc-82b4-80051b808d73");
                OrphanFile2.FileType = ".jpg";
                OrphanFile2.FileURL = @"/assets/Orphans/default/";
                result = OrphanFile2.FileURL + OrphanFile2.FileName + OrphanFile2.FileType;

            }



            return result;// @OrphanFile.FileURL+ @OrphanFile.FileName+'.'+ @OrphanFile.FileType;
        }
        public string GetPortfolioThumbImage(int orphanid, bool gender)
        {
            string result = "";

            OrphanFile OrphanFile = null;

            var orphanfileid = _OrphanService.GetOrphans().Where(x => x.Id == orphanid).FirstOrDefault().OrphanFileId;

            if (orphanfileid.HasValue)
            {

                OrphanFile = _OrphanService.GetOrphanFiles().Where(x => x.Id == orphanfileid.Value).FirstOrDefault();
                if (OrphanFile != null)
                    result = OrphanFile.FileURL.Replace("/Original/", "/Thumbnail/") + OrphanFile.FileName + "_thumb" + OrphanFile.FileType;
            }
            if (OrphanFile == null)
            {
                OrphanFile OrphanFile2 = new OrphanFile();

                OrphanFile2.Id = 2;
                if (gender)
                    OrphanFile2.FileName = System.Guid.Parse("70f1825c-3e3a-4afc-82b4-80051b808d72");
                else
                    OrphanFile2.FileName = System.Guid.Parse("70f1825c-3e3a-4afc-82b4-80051b808d73");
                OrphanFile2.FileType = ".jpg";
                OrphanFile2.FileURL = @"/assets/orphans/default/";
                result = OrphanFile2.FileURL + OrphanFile2.FileName + OrphanFile2.FileType;

            }



            return result;// @OrphanFile.FileURL+ @OrphanFile.FileName+'.'+ @OrphanFile.FileType;
        }

        public async Task<ActionResult> SaveOrphanFilesAsync(IEnumerable<IFormFile> files, int id)
        {
            const int thumbImageSizeX = 32;
            const int thumbImageSizeY = 32;

            // The Name of the Upload component is "files"
            if (files != null)
            {
                foreach (var file in files)
                {
                    var fileContent = ContentDispositionHeaderValue.Parse(file.ContentDisposition);


                    var imageExtension = Path.GetExtension(fileContent.FileName.Value);
                    var newfilename = Guid.NewGuid().ToString() + imageExtension;


                    var workPath = @"/assets/Orphans/Files/Original/";

                    // Some browsers send file names with full path.
                    // We are only interested in the file name.
                    var fileName = Path.GetFileName(fileContent.FileName.ToString().Trim('"'));

                    var physicalPath = Path.Combine(_HostingEnvironment.WebRootPath, "assets\\Orphans\\Files\\Original", newfilename);
                    var thumbphysicalPath = physicalPath.Replace("\\Files\\Original", "\\Files\\Thumbnail").Replace(imageExtension, "_thumb" + imageExtension);


                    try
                    {

                        if ((imageExtension == ".png") || (imageExtension == ".jpg"))
                        {
                            using (var fileStream = new FileStream(physicalPath, FileMode.Create))
                            {
                                await file.CopyToAsync(fileStream);
                            }

                            OrphanFile ofile = new OrphanFile();
                            ofile.FileSize = int.Parse(file.Length.ToString());
                            ofile.FileName = Guid.Parse(newfilename.Replace(imageExtension, ""));
                            ofile.Title = fileName;
                            ofile.FileType = imageExtension;
                            ofile.Description = "";
                            ofile.UploadedDate = DateTime.Now;
                            ofile.FileURL = workPath;
                            ofile.OrphanId = int.Parse(id.ToString());


                            //  int width = 128;
                            //  int height = 128;
                            //  var file = args[0];
                            ////  Console.WriteLine($"Loading {file}");
                            //  using (FileStream pngStream = new FileStream(args[0], FileMode.Open, FileAccess.Read))
                            //  using (var image = new Bitmap(pngStream))
                            //  {
                            //      var resized = new Bitmap(width, height);
                            //      using (var graphics = Graphics.FromImage(resized))
                            //      {
                            //          graphics.CompositingQuality = CompositingQuality.HighSpeed;
                            //          graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            //          graphics.CompositingMode = CompositingMode.SourceCopy;
                            //          graphics.DrawImage(image, 0, 0, width, height);
                            //          resized.Save($"resized-{file}", ImageFormat.Png);
                            //          Console.WriteLine($"Saving resized-{file} thumbnail");
                            //      }
                            //  }


                            //attach the uploaded image to the object before saving to Database
                            //artwork.ImageMimeType = image.ContentLength;
                            //artwork.ImageData = new byte[image.ContentLength];
                            //image.InputStream.Read(artwork.ImageData, 0, image.ContentLength);

                            //Read image back from file and create thumbnail from it
                            var imageFile = physicalPath;
                            using (var srcImage = Image.FromFile(imageFile))
                            using (var newImage = new Bitmap(thumbImageSizeX, thumbImageSizeY))
                            using (var graphics = Graphics.FromImage(newImage))
                            using (var stream = new MemoryStream())
                            {
                                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                                graphics.DrawImage(srcImage, new Rectangle(0, 0, thumbImageSizeX, thumbImageSizeY));
                                if (imageExtension == ".png") newImage.Save(thumbphysicalPath, ImageFormat.Png);
                                if (imageExtension == ".jpg") newImage.Save(thumbphysicalPath, ImageFormat.Jpeg);
                                //var thumbNew = File(stream.ToArray(), "image/png");
                                //artwork.ArtworkThumbnail = thumbNew.FileContents;
                            }


                            _OrphanService.AddOrphanFile(ofile);


                        }


                        //  _orphanRepository.AddOrphanFile(ofile);
                        //  _orphanRepository.SaveChanges();




                    }



                    catch (Exception ex)
                    {
                        ErrorSignal.FromCurrentContext().Raise(ex);
                    }
                }
            }

            // Return an empty string to signify success
            return Content("");
        }
        //[HttpPost]
        //public ActionResult RemoveOrphanFile(int id)
        //{
        //  var  fileNames= _Service.GetOrphanFiles().Select(x=>x.FileName.ToString()).ToArray();

        //    if (fileNames != null)
        //    {
        //        foreach (var fullName in fileNames)
        //        {
        //            var fileName = Path.GetFileName(fullName);
        //            var imageExtension = Path.GetExtension(fileName);
        //            var physicalPath = Path.Combine(HostingEnvironment.WebRootPath, "assets\\Orphans\\Files\\Original", fileName);
        //            var thumbphysicalPath = physicalPath.Replace("\\Files\\Original", "\\Files\\Thumbnail").Replace(imageExtension, "_thumb" + imageExtension);

        //            // TODO: Verify user permissions

        //            if (System.IO.File.Exists(physicalPath) && System.IO.File.Exists(thumbphysicalPath))
        //            {
        //                var ofile = _Service.GetOrphanFileFromFileName(fullName);
        //                if (ofile != null)
        //                {
        //                    System.IO.File.Delete(physicalPath);
        //                    System.IO.File.Delete(thumbphysicalPath);

        //                    _Service.RemoveOrphanFile(ofile);
        //                }
        //            }
        //        }
        //    }

        //    // Return an empty string to signify success
        //    return Content("");
        //}
        public ActionResult RemoveOrphanFiles(string[] fileNames)
        {
            // The parameter of the Remove action must be called "fileNames"

            if (fileNames != null)
            {
                foreach (var fullName in fileNames)
                {
                    var fileName = Path.GetFileName(fullName);

                    var ofile = _OrphanService.GetOrphanFileFromFileName(fullName);
                    if (ofile != null)
                    {
                        var imageExtension = ofile.FileType;
                        var physicalPath = Path.Combine(_HostingEnvironment.WebRootPath, "assets\\Orphans\\Files\\Original", fileName) + imageExtension;
                        var thumbphysicalPath = physicalPath.Replace("\\Files\\Original", "\\Files\\Thumbnail").Replace(imageExtension, "_thumb" + imageExtension);


                        if (System.IO.File.Exists(physicalPath) && System.IO.File.Exists(thumbphysicalPath))
                        {
                            System.IO.File.Delete(physicalPath);
                            System.IO.File.Delete(thumbphysicalPath);
                        }
                        _OrphanService.RemoveOrphanFile(ofile);

                    }
                }
            }

            // Return an empty string to signify success
            return Content("");
        }

        //public ActionResult SaveAndPersistOrphanFiles(IEnumerable<HttpPostedFileBase> files, int id)
        //{
        //    // The Name of the Upload component is "files"
        //    if (files != null)
        //    {
        //        foreach (var file in files)
        //        {
        //            // Some browsers send file names with full path.
        //            // We are only interested in the file name.
        //            var orginFileName = Path.GetFileName(file.FileName);
        //            var fileName = id.ToString() + "__" + Path.GetFileNameWithoutExtension(file.FileName.Replace(" ", "_")) + "__" + Guid.NewGuid().ToString();
        //            var fileExtension = Path.GetExtension(file.FileName);
        //            var physicalPath = Path.Combine(Server.MapPath("~/App_Data/GifAidForms"), fileName + fileExtension);

        //            //// var fileSize = 0; 
        //            //// SessionUploadInitialFilesRepository.Add("Donors", new UploadInitialFile(fileName, file.ContentLength, fileExtension));

        //            try
        //            {
        //                // The files are saved 
        //                file.SaveAs(physicalPath);
        //                System.Threading.Thread.Sleep(1000);

        //                OrphanFile orphanFile = new OrphanFile();
        //                orphanFile.DonorID = id;// (int) TempData["DonorID"];

        //                orphanFile.GifAidFormOrginFileName = orginFileName;
        //                orphanFile.GifAidFormFileExtension = fileExtension;

        //                orphanFile.GifAidFormFileName = fileName + fileExtension;
        //                orphanFile.GifAidFormFileSize = new FileInfo(physicalPath).Length;
        //                orphanFile.GifAidFormFileDescription = "";
        //                orphanFile.UploadedDate = DateTime.Now;

        //                var UserId = User.Identity.GetUserId();
        //                    orphanFile.UpdatedBy = UserId;

        //                _donorRepository.AddGifAidFormFile(orphanFile);
        //                if (UserId != null)
        //                _donorRepository.SaveChanges();


        //                //Add user log

        //                if (UserId != null)
        //                    _donorRepository.AddUserLog(UserId, DateTime.Now, "gifAidFormFile " + orphanFile.GifAidFormFileID + " Record Created and file uploaded");

        //            }
        //            catch (Exception ex)
        //            {
        //                Session["Message"] = ex.Message.ToString();

        //                ErrorSignal.FromCurrentContext().Raise(ex);
        //                //   var text = @"Sheet name and column header must match the following: 'Donor Database' ";
        //                //  Session["Message"] =  Session["Message"] + text;
        //                ViewBag.Message = (string)Session["Message"];
        //                ViewData["Message"] = (string)Session["Message"];
        //                // System.Threading.Thread.Sleep(2000);

        //            }

        //            ////RedirectToAction("InitialFilesDonors");


        //        }
        //    }
        //    //if (Session["Message"] != null)
        //    //{
        //    //    //(string) Session["Message"]
        //    //    return Content(ViewBag.Message);
        //    //}
        //    //Session["Message"] = null;
        //    // Return an empty string to signify success
        //    return Content("");
        //}
        //public ActionResult RemoveAndPersistGifAidFormFiles(string[] fileNames, int id)
        //public ActionResult RemoveAndPersistGifAidFormFiles(string[] fileNames)
        //    {
        //        //   var fileNames = _donorRepository.GifAidFormFiles.Where(x => x.DonorID == id).Select(x => x.GifAidFormFileName).ToArray();

        //        // The parameter of the Remove action must be called "fileNames"
        //        if (fileNames != null)
        //        {
        //            foreach (var fullName in fileNames)
        //            {
        //                var fileName = Path.GetFileName(fullName);
        //                //var physicalPath = Path.Combine(Server.MapPath("~/App_Data/GifAidForms"), fileName);

        //                //// SessionUploadInitialFilesRepository.Remove(fileName);

        //                //if (System.IO.File.Exists(physicalPath))
        //                //{
        //                //    // The files are not actually removed in this demo
        //                //    System.IO.File.Delete(physicalPath);

        //                //    var gifAidFormFile = _donorRepository.GetGifAidFormFiles().Where(x => x.GifAidFormFileName == fullName).FirstOrDefault();
        //                //    if (gifAidFormFile != null)
        //                //    {
        //                //        _donorRepository.RemoveGifAidFormFile(gifAidFormFile);
        //                //        _donorRepository.SaveChanges();



        //                //        //Add user log
        //                //        var UserId = User.Identity.GetUserId();
        //                //        if (UserId != null)
        //                //            _donorRepository.AddUserLog(UserId, DateTime.Now, "gifAidFormFile " + gifAidFormFile.GifAidFormFileID + " Record and file deleted ");

        //                //    }
        //                //}

        //            }
        //        }

        //        // Return an empty string to signify success
        //        return Content("");
        //    }

        public FileStreamResult DownloadFile(string fileUploadString)
        {

            //var attachment = _donorRepository.GetGifAidFormFiles().Where(up => up.GifAidFormFileName == fileUploadString).FirstOrDefault();
            //if (attachment != null)
            //{
            //    var filePhysicalPath = Path.Combine(Server.MapPath("~/App_Data/GifAidForms"), attachment.GifAidFormFileName);

            //    if (System.IO.File.Exists(filePhysicalPath))
            //    {
            //        string contentType = "application/octetstream";
            //        var fileStreamResult = new FileStreamResult(new FileStream(filePhysicalPath, FileMode.Open, FileAccess.Read), contentType);

            //        fileStreamResult.FileDownloadName = attachment.DonorID.ToString() + "__" + attachment.GifAidFormOrginFileName;

            //        return fileStreamResult;
            //    }
            //}

            return null;
        }


        [HttpPost]
        public ActionResult Excel_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }




        // [AllowAnonymous]
        // [Route("/[controller]/[action]/{id}")]
        //// [Route("/president/{id}.aspx")]
        // public ActionResult Details(int? id)
        // {
        //     if (id == null || id.HasValue == false)
        //     {
        //         return new BadRequestResult();
        //     }

        //     var president = _Service.GetOrphanById(id.Value);

        //     if (president == null)
        //     {
        //         return NotFound();
        //     }

        //     return View(president);
        // }

        // [Route("/orphan/{last:alpha}/{first:alpha}")]
        // public ActionResult Details(string last, string first)
        // {
        //     if (String.IsNullOrWhiteSpace(last) == true ||
        //         String.IsNullOrWhiteSpace(first) == true)
        //     {
        //         return new BadRequestResult();
        //     }

        //     var president = _Service.Search(
        //         first, last).FirstOrDefault();

        //     if (president == null)
        //     {
        //         return NotFound();
        //     }

        //     return View("Details", president);
        // }

        // public ActionResult Create()
        // {
        //     return RedirectToAction("Edit", new { id = ID_FOR_CREATE_NEW_PRESIDENT });
        // }

        // // [Authorize(Roles = SecurityConstants.RoleName_Admin)]
        // [Authorize(Policy = SecurityConstants.PolicyName_EditPresident)]
        // public ActionResult Edit(int? id)
        // {
        //     if (id == null)
        //     {
        //         return new BadRequestResult();
        //     }

        //     President president;

        //     if (id.Value == ID_FOR_CREATE_NEW_PRESIDENT)
        //     {
        //         // create new
        //         president = new President();
        //         //president.AddTerm(PresidentsConstants.President,
        //         //    default(DateTime),
        //         //    default(DateTime), 0);
        //     }
        //     else
        //     {
        //         president = _Service.GetOrphanById(id.Value);
        //     }

        //     if (president == null)
        //     {
        //         return NotFound();
        //     }

        //     return View(president);
        // }

        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // // [Authorize(Roles = SecurityConstants.RoleName_Admin)]
        // [Authorize(Policy = SecurityConstants.PolicyName_EditPresident)]
        // public ActionResult Edit(Orphan president)
        // {
        //   //  if (_Validator.IsValid(president) == true)
        //     {
        //         bool isCreateNew = false;

        //         if (president.Id == ID_FOR_CREATE_NEW_PRESIDENT)
        //         {
        //             isCreateNew = true;
        //         }
        //         else
        //         {
        //             Orphan toValue =
        //                 _Service.GetOrphanById(president.Id);

        //             if (toValue == null)
        //             {
        //                 return new BadRequestObjectResult(
        //                     String.Format("Unknown president id '{0}'.", president.Id));
        //             }
        //         }

        //         _Service.Save(president);

        //         if (isCreateNew == true)
        //         {
        //             RedirectToAction("Edit", new { id = president.Id });
        //         }
        //         else
        //         {
        //             return RedirectToAction("Edit");
        //         }
        //     }

        //     return View(president);
        // }

        // //[AllowAnonymous]
        // public async Task<ActionResult> ResetDatabase()
        // {
        //   //  await _TestDataUtility.CreatePresidentTestData();

        //     return RedirectToAction("Index");
        // }

        // //[AllowAnonymous]
        // public ActionResult VerifyDatabaseIsPopulated()
        // {
        //   //  _TestDataUtility.VerifyDatabaseIsPopulated();

        //     return RedirectToAction("Index");
        // }
    }
}