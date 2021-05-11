
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
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace NoorTrust.DonationFund.WebUI.Controllers
{
    [Authorize(Roles = "Staff,Admin,Volunteer")]
    // [Authorize(Roles = SecurityConstants.RoleName_Admin)]
    public class SponsorController : Controller
    {
        //  readonly NoorTrustDbContext db = new NoorTrustDbContext((new DbContextOptions()).IsFrozen);

        // IRepository<Sponsor> _RepositoryInstance;
        private const int ID_FOR_CREATE_NEW_PRESIDENT = 0;
        private ISponsorService _Service;
        private IOrphanService _OrphanService;
        private ILogger _Log;
        // private IValidatorStrategy<President> _Validator;
        // private ITestDataUtility _TestDataUtility;

        public SponsorController(ISponsorService Service,
            IOrphanService OrphanService,
            ILogger Log
            //, IValidatorStrategy<President> validator
            //  ITestDataUtility testDataUtility
            )
        {
            //      if (Service == null)
            //          throw new ArgumentNullException("service", "service is null.");

            //if (validator == null)
            //{
            //    throw new ArgumentNullException("validator", "Argument cannot be null.");
            //}

            //  _Validator = validator;
            _Service = Service;
            _OrphanService = OrphanService;
            _Log = Log;
            //  _TestDataUtility = testDataUtility;
        }
        public IActionResult Index()
        {
            //   BuildViewData(ViewData);
            return View();
        }
        public void BuildViewData(ViewDataDictionary viewDataDictionary)
        {
            var donors = _OrphanService.GetOrphans();//.Where(x => x.IsActive == true);



            var list = donors.Select(ct => new SelectListItem
            {
                Text = "ID: " + ct.Id + " " + ct.Age + " " + ct.ArGrandFather + " " + " " + ct.ArFatherName + " " + ct.ArFirstName,
                Value = ct.Id.ToString(),
                Selected = false
            }).ToList();

            viewDataDictionary.Add("OrphansSelectList", list);//.ToList().AsEnumerable()

            List<SelectListItem> yesnoSelectList = new List<SelectListItem>()  {
                                    //new SelectListItem {Text = "Unknown", Value = "null"},
                                    new SelectListItem {Text = "Yes", Value = "true"},
                                    new SelectListItem {Text = "No", Value = "false"} };

            viewDataDictionary.Add("YesNoSelectList", yesnoSelectList);

            List<SelectListItem> genderSelectList = new List<SelectListItem>()  {
                                    new SelectListItem {Text = "Unknown", Value = "null"},
                                    new SelectListItem {Text = "Yes", Value = "true"},
                                    new SelectListItem {Text = "No", Value = "false"} };

            viewDataDictionary.Add("GenderSelectList", genderSelectList);

        }
        public async Task<IActionResult> Sponsors_ReadAsync([DataSourceRequest]DataSourceRequest request)
        {
            var persons = await _Service.GetSponsorsAsync();
            var orphans = await _OrphanService.GetOrphansAsync();

            var list = orphans.Select(ct => new SelectListItem
            {
                Text = "ID: " + ct.Id + " " + ct.Age + " " + ct.ArGrandFather + " " + " " + ct.ArFatherName + " " + ct.ArFirstName,
                Value = ct.Id.ToString(),
                Selected = false
            }).ToList();

            ViewData["OrphansSelectList"] = list;//.ToList().AsEnumerable()

            List<SelectListItem> yesnoSelectList = new List<SelectListItem>()  {
                                    //new SelectListItem {Text = "Unknown", Value = "null"},
                                    new SelectListItem {Text = "Yes", Value = "true"},
                                    new SelectListItem {Text = "No", Value = "false"} };

            ViewData["YesNoSelectList"] = yesnoSelectList;

            List<SelectListItem> genderSelectList = new List<SelectListItem>()  {
                                    new SelectListItem {Text = "Unknown", Value = "null"},
                                    new SelectListItem {Text = "Yes", Value = "true"},
                                    new SelectListItem {Text = "No", Value = "false"} };

            ViewData["GenderSelectList"] = genderSelectList;


            DataSourceResult result = null;
            try
            {
                result = persons.ToDataSourceResult(request, sponsor => new SponsorViewModel
                {
                    Id = sponsor.Id,
                    Title = sponsor.Title,
                    FirstName = sponsor.FirstName,
                    MiddleName = sponsor.MiddleName,
                    LastName = sponsor.LastName,
                    ArTitle = sponsor.ArTitle,
                    ArFirstName = sponsor.ArFirstName,
                    ArLastName = sponsor.ArLastName,
                    Address1 = sponsor.Address1,
                    Address2 = sponsor.Address2,
                    County = sponsor.County,
                    City = sponsor.City,
                    PostalCode = sponsor.PostalCode,
                    Country = sponsor.Country,

                    HomePhone = sponsor.HomePhone,
                    MobileNumber = sponsor.MobileNumber,
                    Email = sponsor.Email,
                    OtherEmail = sponsor.OtherEmail,

                    IsGiftAid = sponsor.IsGiftAid,
                    GiftAidRef = sponsor.GiftAidRef,
                    NameOnBankStatement = sponsor.NameOnBankStatement,
                    PaymentName = sponsor.PaymentName,

                    IsReceiveEmail = sponsor.IsReceiveEmail,
                    IsReceiveMobile = sponsor.IsReceiveMobile,
                    IsReceivePost = sponsor.IsReceivePost,

                    RegisterDate = sponsor.RegisterDate,
                    IsActive = sponsor.IsActive,
                    DeactivatedDate = sponsor.DeactivatedDate,

                    OrphanGenderChoice = sponsor.OrphanGenderChoice,
                    OrphanCityChoiceId = sponsor.OrphanCityChoiceId,
                    NumberOfOrphans = sponsor.NumberOfOrphans,
                    CalculatedNumberOfOrphans = orphans.Where(x => x.SponsorId == sponsor.Id).Count(),
                    SStartDate = sponsor.SStartDate,
                    SEndDate = sponsor.SEndDate,
                    LanguagePref = sponsor.LanguagePref,
                    EthnicityId = sponsor.EthnicityId,
                    ReferralType = sponsor.ReferralType,
                    Notes = sponsor.Notes,


                    Comments = (sponsor.NumberOfOrphans > orphans.Where(x => x.SponsorId == sponsor.Id).Count()) && (sponsor.IsActive) ? (sponsor.NumberOfOrphans - orphans.Where(x => x.SponsorId == sponsor.Id).Count()).ToString() + " Orphan(s) needed" : "",

                    LastUpdated = sponsor.LastUpdated,
                    LastUpdatedBy = sponsor.LastUpdatedBy


                });

            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);

            }
            return Json(result);
        }
        //[AllowAnonymous]
        //public ActionResult Index()
        //{


        //    return View();// presidents);
        //}
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
        //[AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Sponsors_Create([DataSourceRequest]DataSourceRequest request, Sponsor sponsor)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var entity = new Sponsor
                    {

                        Id = sponsor.Id,
                        Title = sponsor.Title,
                        FirstName = sponsor.FirstName,
                        MiddleName = sponsor.MiddleName,
                        LastName = sponsor.LastName,
                        ArTitle = sponsor.ArTitle,
                        ArFirstName = sponsor.ArFirstName,
                        ArLastName = sponsor.ArLastName,
                        Address1 = sponsor.Address1,
                        Address2 = sponsor.Address2,
                        County = sponsor.County,
                        City = sponsor.City,
                        PostalCode = sponsor.PostalCode,
                        Country = sponsor.Country,

                        HomePhone = sponsor.HomePhone,
                        MobileNumber = sponsor.MobileNumber,
                        Email = sponsor.Email,
                        OtherEmail = sponsor.OtherEmail,

                        IsGiftAid = sponsor.IsGiftAid,
                        GiftAidRef = sponsor.GiftAidRef,
                        NameOnBankStatement = sponsor.NameOnBankStatement,
                        PaymentName = sponsor.PaymentName,

                        IsReceiveEmail = sponsor.IsReceiveEmail,
                        IsReceiveMobile = sponsor.IsReceiveMobile,
                        IsReceivePost = sponsor.IsReceivePost,

                        RegisterDate = sponsor.RegisterDate,
                        IsActive = sponsor.IsActive,
                        DeactivatedDate = sponsor.DeactivatedDate,

                        OrphanGenderChoice = sponsor.OrphanGenderChoice,
                        OrphanCityChoiceId = sponsor.OrphanCityChoiceId,
                        NumberOfOrphans = sponsor.NumberOfOrphans,
                        SStartDate = sponsor.SStartDate,
                        SEndDate = sponsor.SEndDate,
                        LanguagePref = sponsor.LanguagePref,
                        EthnicityId = sponsor.EthnicityId,
                        ReferralType = sponsor.ReferralType,
                        Notes = sponsor.Notes,
                        LastUpdated = DateTime.Now,
                        LastUpdatedBy = User.Identity.Name

                    };
                    if (entity.Country == null) entity.Country = 1;

                    _Service.Save(entity);
                    //db.SaveChanges();
                    sponsor.Id = entity.Id;

                    _Log.LogUsage("Add new Sponsor, Id: " + sponsor.Id);

                }
                catch (Exception ex)
                {

                    ErrorSignal.FromCurrentContext().Raise(ex);
                }
            }

            return Json(new[] { sponsor }.ToDataSourceResult(request, ModelState));
        }

        // [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public ActionResult Sponsors_Update([DataSourceRequest]DataSourceRequest request, Sponsor sponsor)
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
                try
                {
                    var entity = new Sponsor
                    {
                        Id = sponsor.Id,
                        Title = sponsor.Title,
                        FirstName = sponsor.FirstName,
                        MiddleName = sponsor.MiddleName,
                        LastName = sponsor.LastName,
                        ArTitle = sponsor.ArTitle,
                        ArFirstName = sponsor.ArFirstName,
                        ArLastName = sponsor.ArLastName,
                        Address1 = sponsor.Address1,
                        Address2 = sponsor.Address2,
                        County = sponsor.County,
                        City = sponsor.City,
                        PostalCode = sponsor.PostalCode,
                        Country = sponsor.Country,

                        HomePhone = sponsor.HomePhone,
                        MobileNumber = sponsor.MobileNumber,
                        Email = sponsor.Email,
                        OtherEmail = sponsor.OtherEmail,

                        IsGiftAid = sponsor.IsGiftAid,
                        GiftAidRef = sponsor.GiftAidRef,
                        NameOnBankStatement = sponsor.NameOnBankStatement,
                        PaymentName = sponsor.PaymentName,

                        IsReceiveEmail = sponsor.IsReceiveEmail,
                        IsReceiveMobile = sponsor.IsReceiveMobile,
                        IsReceivePost = sponsor.IsReceivePost,

                        RegisterDate = sponsor.RegisterDate,
                        IsActive = sponsor.IsActive,
                        DeactivatedDate = sponsor.DeactivatedDate,

                        OrphanGenderChoice = sponsor.OrphanGenderChoice,
                        OrphanCityChoiceId = sponsor.OrphanCityChoiceId,
                        NumberOfOrphans = sponsor.NumberOfOrphans,
                        SStartDate = sponsor.SStartDate,
                        SEndDate = sponsor.SEndDate,
                        LanguagePref = sponsor.LanguagePref,
                        EthnicityId = sponsor.EthnicityId,
                        ReferralType = sponsor.ReferralType,
                        Notes = sponsor.Notes,

                        LastUpdated = DateTime.Now,
                        LastUpdatedBy = User.Identity.Name
                    };

                    _Service.Save(entity);


                    _Log.LogUsage("Update Sponsor, Id: " + entity.Id);

                }
                catch (Exception ex)
                {

                    ErrorSignal.FromCurrentContext().Raise(ex);
                }
                //db.Entry(entity).State = EntityState.Modified;
                //db.SaveChanges();
            }

            return Json(new[] { sponsor }.ToDataSourceResult(request, ModelState));
        }

        //  [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public ActionResult Sponsors_Destroy([DataSourceRequest]DataSourceRequest request, Sponsor sponsor)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var entity = new Sponsor
                    {
                        Id = sponsor.Id,
                        Title = sponsor.Title,
                        FirstName = sponsor.FirstName,
                        MiddleName = sponsor.MiddleName,
                        LastName = sponsor.LastName,
                        ArTitle = sponsor.ArTitle,
                        ArFirstName = sponsor.ArFirstName,
                        ArLastName = sponsor.ArLastName,
                        Address1 = sponsor.Address1,
                        Address2 = sponsor.Address2,
                        County = sponsor.County,
                        City = sponsor.City,
                        PostalCode = sponsor.PostalCode,
                        Country = sponsor.Country,

                        HomePhone = sponsor.HomePhone,
                        MobileNumber = sponsor.MobileNumber,
                        Email = sponsor.Email,
                        OtherEmail = sponsor.OtherEmail,

                        IsGiftAid = sponsor.IsGiftAid,
                        GiftAidRef = sponsor.GiftAidRef,
                        NameOnBankStatement = sponsor.NameOnBankStatement,
                        PaymentName = sponsor.PaymentName,

                        IsReceiveEmail = sponsor.IsReceiveEmail,
                        IsReceiveMobile = sponsor.IsReceiveMobile,
                        IsReceivePost = sponsor.IsReceivePost,

                        RegisterDate = sponsor.RegisterDate,
                        IsActive = !sponsor.IsActive,
                        DeactivatedDate = sponsor.DeactivatedDate == null ? DateTime.Now : DateTime.Now,

                        OrphanGenderChoice = sponsor.OrphanGenderChoice,
                        OrphanCityChoiceId = sponsor.OrphanCityChoiceId,
                        NumberOfOrphans = sponsor.NumberOfOrphans,
                        SStartDate = sponsor.SStartDate,
                        SEndDate = sponsor.SEndDate,
                        LanguagePref = sponsor.LanguagePref,
                        EthnicityId = sponsor.EthnicityId,
                        ReferralType = sponsor.ReferralType,
                        Notes = sponsor.Notes,

                        LastUpdated = sponsor.LastUpdated,
                        LastUpdatedBy = sponsor.LastUpdatedBy
                    };

                    _Service.Save(entity);


                    _Log.LogUsage("Disable Sponsor, Id: " + entity.Id);

                }
                catch (Exception ex)
                {

                    ErrorSignal.FromCurrentContext().Raise(ex);
                }
                // db.Sponsors.Remove(entity);
                // db.SaveChanges();
            }

            return Json(new[] { sponsor }.ToDataSourceResult(request, ModelState));
        }
        //----------------------------------------------------------------------------------------------------------


        public IActionResult SponsorActivity_ReadById([DataSourceRequest]DataSourceRequest request, int id)
        {

            var sponsorActivites = _Service.GetSponsorActivitiesBySponsorId(id);

            DataSourceResult result = sponsorActivites.ToDataSourceResult(request, sponsor => new SponsorActivityViewModel
            {
                Id = sponsor.Id,
                PostedDate = sponsor.PostedDate,
                ActivityName = sponsor.ActivityName,
                Description = sponsor.Description,
                Author = sponsor.Author

            });

            

            return Json(result);
        }

        public ActionResult SponsorActivity_CreateById([DataSourceRequest]DataSourceRequest request, SponsorActivityViewModel orphan, int id)
        {


            return Json(new[] { orphan }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult Sponsors_AddSponsorActivity([DataSourceRequest]DataSourceRequest request, SponsorActivityViewModel sponsor, string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                var entity = new SponsorActivity();


                entity.SponsorId = Int32.Parse(id);
                entity.PostedDate = DateTime.Now;
                entity.Author = User.Identity.Name;
                _Service.SaveSponsorActivity(entity);


                _Log.LogUsage("Add new SponsorActivity, Id: " + entity.Id);


            }

            return Json(new[] { sponsor }.ToDataSourceResult(request, ModelState));
        }
        [HttpPost]
        public ActionResult SponsorActivity_UpdateById([DataSourceRequest]DataSourceRequest request, SponsorActivityViewModel sponsorActivity, int id)
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
                try
                {
                   

                    var entity = _Service.GetSponsorActivitiesById(id);

                  //  entity.SponsorId = sponsorActivity.SponsorId;
                  //  entity.PostedDate = sponsorActivity.PostedDate;
                      entity.Author = User.Identity.Name;
                   
                    entity.Description = sponsorActivity.Description;
                    entity.ActivityName = sponsorActivity.ActivityName;

                    _Service.SaveSponsorActivity(entity);




                    _Log.LogUsage("Update SponsorActivity, Id: " + entity.Id);

                }
                catch (Exception ex)
                {

                    ErrorSignal.FromCurrentContext().Raise(ex);
                }
                //db.Entry(entity).State = EntityState.Modified;
                //db.SaveChanges();
            }

            return Json(new[] { sponsorActivity }.ToDataSourceResult(request, ModelState));
        }
        [HttpDelete]
        public ActionResult SponsorActivity_DestroyById([DataSourceRequest]DataSourceRequest request, SponsorActivityViewModel sponsorActivity, int id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                   

                    _Service.RemoveSponsorActivitiesById(id);


                    _Log.LogUsage("delete SponsorActivity, Id: " + id);

                }
                catch (Exception ex)
                {

                    ErrorSignal.FromCurrentContext().Raise(ex);
                }
                // db.Sponsors.Remove(entity);
                // db.SaveChanges();
            }

            return Json(new[] { sponsorActivity }.ToDataSourceResult(request, ModelState));
        }
        //-------------------------------------------------------------------------------

        //----------------------------------------------------------------------------------------------------------------------------------
        public JsonResult ServerFiltering_GetOrphans(string text)
        {
            var orphans = _OrphanService.GetOrphans().Where(x => x.IsActive == true && x.SponsorId == null).OrderBy(x => x.ArFirstName).ThenBy(x => x.ArFatherName).ThenBy(x => x.ArGrandFather);


            //var products = db.Select(product => new DonorViewModel
            //{
            //    ProductID = product.ProductID,
            //    ProductName = product.ProductName,
            //    UnitPrice = product.UnitPrice ?? 0,
            //    UnitsInStock = product.UnitsInStock ?? 0,
            //    UnitsOnOrder = product.UnitsOnOrder ?? 0,FirstName
            //    Discontinued = product.Discontinued
            //});

            var list = orphans.Select(ct => new SelectListItem
            {
                Text = "ID: " + ct.Id + " " + ct.ArGrandFather + " " + ct.ArFatherName + " " + ct.ArFirstName + " Age: " + ct.Age + " Sayed? " + (ct.IsSayed == true ? "Yes" : "No"),
                Value = ct.Id.ToString(),
                Selected = false
            });



            if (!string.IsNullOrEmpty(text))
            {
                list = list.Where(p => p.Text.Contains(text));
            }

            return Json(list);//, JsonRequestBehavior.AllowGet);


        }



        [HttpPost]
        public ActionResult Excel_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }








        //-------------------------------------


        // [AllowAnonymous]
        // [Route("/[controller]/[action]/{id}")]
        //// [Route("/president/{id}.aspx")]
        // public ActionResult Details(int? id)
        // {
        //     if (id == null || id.HasValue == false)
        //     {
        //         return new BadRequestResult();
        //     }

        //     var president = _Service.GetSponsorById(id.Value);

        //     if (president == null)
        //     {
        //         return NotFound();
        //     }

        //     return View(president);
        // }

        // [Route("/sponsor/{last:alpha}/{first:alpha}")]
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
        //         president = _Service.GetSponsorById(id.Value);
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
        // public ActionResult Edit(Sponsor president)
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
        //             Sponsor toValue =
        //                 _Service.GetSponsorById(president.Id);

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