
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
        private IDonationService _DonationService;

        public SponsorController(ISponsorService Service
            //, IValidatorStrategy<President> validator
            //  ITestDataUtility testDataUtility
            , IDonationService DonationService
            )
        {
            if (Service == null)
                throw new ArgumentNullException("service", "service is null.");

            //if (validator == null)
            //{
            //    throw new ArgumentNullException("validator", "Argument cannot be null.");
            //}

            //  _Validator = validator;
            _Service = Service;
            _DonationService = DonationService;
            //  _TestDataUtility = testDataUtility;
        }
        public IActionResult Index()
        {
          //  var s = _Service.GetSponsors();
            return View();
        }
        [HttpPost]
        public string Index(string tempDataValue)
        {
            TempData["sId"] = tempDataValue;
            //Session["sId"] = tempDataValue;
            return TempData["sId"].ToString();
        }
        public async Task<IActionResult> Sponsors_ReadAsync([DataSourceRequest]DataSourceRequest request)
        {
            var persons = await _Service.GetSponsorsAsync();
            DataSourceResult result = persons.ToDataSourceResult(request, sponsor => new SponsorViewModel
            {
                
                Id = sponsor.Id,
                PersonId = sponsor.PersonId,
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


            });

            return Json(result);
        }
        public  ActionResult Sponsors_ReadById( [DataSourceRequest]DataSourceRequest request)
        {
           var id = 1;
            var person =  _Service.GetSponsorById(id);

            var persons = new List<Sponsor>();
            persons.Add(person);

            DataSourceResult result = persons.ToDataSourceResult(request, sponsor => new SponsorViewModel
            {
                Id = sponsor.Id,
                PersonId = sponsor.PersonId,
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


            });

            return Json(result);
        }
        //[AllowAnonymous]
        //public ActionResult Index()
        //{
        //    var presidents = _Service.GetSponsors();

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
                var entity = new Sponsor
                {
                    PersonId = sponsor.PersonId,
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
                //db.SaveChanges();
                sponsor.Id = entity.Id;
            }

            return Json(new[] { sponsor }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Sponsors_CreateById(SponsorViewModel c)
        {
            //            if (c.StaffId > 0)
            {
                var entity = new Sponsor
                {
                    //  StaffId = 0,
                    Id = 1
                };

                _Service.Save(entity);
            }

            return View();// RedirectToAction("Index");
        }



        [HttpPost]
        //[AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Sponsors_CreateByIdx([DataSourceRequest]DataSourceRequest request, Sponsor sponsor)
        {
            if (ModelState.IsValid)
            {
                var entity = new Sponsor
                {
                   
                    PersonId = sponsor.PersonId,
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
                //db.SaveChanges();
                sponsor.Id = entity.Id;
            }

            return Json(new[] { sponsor }.ToDataSourceResult(request, ModelState));
        }

        // [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public ActionResult Sponsors_Update([DataSourceRequest]DataSourceRequest request, Sponsor sponsor)
        {
            if (ModelState.IsValid)
            {
                var entity = new Sponsor
                {
                   // Id = sponsor.Id,
                    PersonId = sponsor.PersonId,
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
                //db.Entry(entity).State = EntityState.Modified;
                //db.SaveChanges();
            }

            return Json(new[] { sponsor }.ToDataSourceResult(request, ModelState));
        }
        [HttpPost]
        public ActionResult Sponsors_UpdateById([DataSourceRequest]DataSourceRequest request, Sponsor sponsor)
        {
            if (ModelState.IsValid)
            {
                var entity = new Sponsor
                {
                   // Id = sponsor.Id,
                    PersonId = sponsor.PersonId,
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
                var entity = new Sponsor
                {
                    PersonId = sponsor.PersonId,
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
                // db.Sponsors.Remove(entity);
                // db.SaveChanges();
            }

            return Json(new[] { sponsor }.ToDataSourceResult(request, ModelState));
        }
        //----------------------------------------------------------------

        

             [HttpPost]
      
        public ActionResult Donations_Create([DataSourceRequest]DataSourceRequest request, Donation donation)
        {
            if (ModelState.IsValid)
            {
                var entity = new Donation
                {
                    Id = donation.Id,
                    DonationAmount = donation.DonationAmount,
                    DonationFor = donation.DonationFor,
                    DonationDate = donation.DonationDate,
                    //SStartDate = donation.SStartDate,
                    //SEndDate = donation.SEndDate,
                    //LanguagePref = donation.LanguagePref,
                    //EthnicityId = donation.EthnicityId,
                    //ReferralType = donation.ReferralType,
                    Notes = donation.Notes,
                    LastUpdated = donation.LastUpdated,
                    LastUpdatedBy = donation.LastUpdatedBy

                };

                _DonationService.Save(entity);
                //db.SaveChanges();
                donation.Id = entity.Id;
            }

            return Json(new[] { donation }.ToDataSourceResult(request, ModelState));
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

        //     var president = _Service.GetSponsorById(id.Value);

        //     if (president == null)
        //     {
        //         return NotFound();
        //     }

        //     return View(president);
        // }

        // [Route("/person/{last:alpha}/{first:alpha}")]
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