
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

namespace NoorTrust.DonationFund.WebUI.Controllers
{
    [Authorize(Roles = "Staff,Admin,Volunteer")]
    // [Authorize(Roles = SecurityConstants.RoleName_Admin)]
    public class DonationController : Controller
    {


        private IDonationService _Service;
        private ILogger _Log;

        public DonationController(IDonationService Service, ILogger Log
            //, IValidatorStrategy<President> validator
            //  ITestDataUtility testDataUtility
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
            _Log = Log;
            //  _TestDataUtility = testDataUtility;
        }
        public IActionResult Index()
        {
            BuildViewData(ViewData);
            return View();
        }

        public void BuildViewData(ViewDataDictionary viewDataDictionary)
        {

            // viewDataDictionary.Add("DonorsSelectList", DonersSelectList);//.ToList().AsEnumerable()

            //List<SelectListItem> yesnoSelectList = new List<SelectListItem>()  {
            //                        //new SelectListItem {Text = "Unknown", Value = "null"},
            //                        new SelectListItem {Text = "Yes", Value = "true"},
            //                        new SelectListItem {Text = "No", Value = "false"} };

            //viewDataDictionary.Add("YesNoSelectList", yesnoSelectList);
        }
        public async Task<IActionResult> Donations_ReadAsync([DataSourceRequest]DataSourceRequest request)
        {
            var donations = await _Service.GetDonationsAsync();
            DataSourceResult result = donations.ToDataSourceResult(request, donation => new DonationViewModel
            {
                Id = donation.Id,
                SponsorId = donation.SponsorId,
                DonationAmount = donation.DonationAmount,
                DonationDate = donation.DonationDate,
                DonationFor = donation.DonationFor,
                DonationType = donation.DonationType,
                PaymentCategory = donation.PaymentCategory,
                PaymentMethod = donation.PaymentMethod,
                Notes = donation.Notes,
                LastUpdated = donation.LastUpdated,
                LastUpdatedBy = donation.LastUpdatedBy,

            });

            return Json(result);
        }

        public IActionResult Donations_ReadById([DataSourceRequest]DataSourceRequest request, int id)
        {
            var donations = _Service.GetDonationsBySponsorId(id);
            DataSourceResult result = donations.ToDataSourceResult(request, donation => new DonationViewModel
            {
                Id = donation.Id,
                SponsorId = donation.SponsorId,
                DonationAmount = donation.DonationAmount,
                DonationDate = donation.DonationDate,
                DonationFor = donation.DonationFor,
                DonationType = donation.DonationType,
                PaymentCategory = donation.PaymentCategory,
                PaymentMethod = donation.PaymentMethod,
                Notes = donation.Notes,
                LastUpdated = donation.LastUpdated,
                LastUpdatedBy = donation.LastUpdatedBy,

            });

            return Json(result);
        }


        //[AllowAnonymous]
        //public ActionResult Index()
        //{
        //    var presidents = _Service.GetDonations();

        //    return View();// presidents);
        //}

        [HttpPost]
        //[AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Donations_Create([DataSourceRequest]DataSourceRequest request, Donation donation)
        {
            if (ModelState.IsValid)
            {
                var entity = new Donation
                {

                    SponsorId = donation.SponsorId,
                    DonationAmount = donation.DonationAmount,
                    DonationDate = donation.DonationDate,
                    DonationFor = donation.DonationFor,
                    DonationType = donation.DonationType,
                    PaymentCategory = donation.PaymentCategory,
                    PaymentMethod = donation.PaymentMethod,
                    Notes = donation.Notes,
                    LastUpdated = donation.LastUpdated,
                    LastUpdatedBy = donation.LastUpdatedBy,
                };

                _Service.Save(entity);
                //db.SaveChanges();
                donation.Id = entity.Id;
            }

            return Json(new[] { donation }.ToDataSourceResult(request, ModelState));
        }

        // [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public ActionResult Donations_Update([DataSourceRequest]DataSourceRequest request, Donation donation)
        {
            if (ModelState.IsValid)
            {
                var entity = new Donation
                {
                    Id = donation.Id,
                    SponsorId = donation.SponsorId,
                    DonationAmount = donation.DonationAmount,
                    DonationDate = donation.DonationDate,
                    DonationFor = donation.DonationFor,
                    DonationType = donation.DonationType,
                    PaymentCategory = donation.PaymentCategory,
                    PaymentMethod = donation.PaymentMethod,
                    Notes = donation.Notes,
                    LastUpdated = donation.LastUpdated,
                    LastUpdatedBy = donation.LastUpdatedBy,


                };

                _Service.Save(entity);
                //db.Entry(entity).State = EntityState.Modified;
                //db.SaveChanges();
            }

            return Json(new[] { donation }.ToDataSourceResult(request, ModelState));
        }
        [HttpPost]
        public ActionResult Donations_UpdateById([DataSourceRequest]DataSourceRequest request, Donation donation, int id)
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

                var entity = _Service.GetDonationById(id);
               
                    entity.Id = donation.Id;
                    entity.SponsorId = donation.SponsorId;
                    entity.DonationAmount = donation.DonationAmount;
                    entity.DonationDate = donation.DonationDate;
                    entity.DonationFor = donation.DonationFor;
                    entity.DonationType = donation.DonationType;
                    entity.PaymentCategory = donation.PaymentCategory;
                    entity.PaymentMethod = donation.PaymentMethod;
                    entity.Notes = donation.Notes;
                    entity.LastUpdated = DateTime.Now;
                    entity.LastUpdatedBy = User.Identity.Name;               

                _Service.Save(entity);

            }

            return Json(new[] { donation }.ToDataSourceResult(request, ModelState));
        }
        //  [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public ActionResult Donations_Destroy([DataSourceRequest]DataSourceRequest request, Donation donation)
        {
            if (ModelState.IsValid)
            {
                var entity = new Donation
                {
                    Id = donation.Id,
                    SponsorId = donation.SponsorId,
                    DonationAmount = donation.DonationAmount,
                    DonationDate = donation.DonationDate,
                    DonationFor = donation.DonationFor,
                    DonationType = donation.DonationType,
                    PaymentCategory = donation.PaymentCategory,
                    PaymentMethod = donation.PaymentMethod,
                    Notes = donation.Notes,
                    LastUpdated = donation.LastUpdated,
                    LastUpdatedBy = donation.LastUpdatedBy,
                };

                _Service.Save(entity);
                // db.Donations.Remove(entity);
                // db.SaveChanges();
            }

            return Json(new[] { donation }.ToDataSourceResult(request, ModelState));
        }

        //----------------------------------------------------



        //public IActionResult Donations_CreateById([DataSourceRequest]DataSourceRequest request, int id)
        //{

        //    var donations = _Service.GetDonationsById(id);

        //    DataSourceResult result = donations.ToDataSourceResult(request, donation => new DonationViewModel
        //    {
        //        Id = donation.Id,
        //        DonationDate = DateTime.Now,


        //    });



        //    return Json(result);
        //}
        public ActionResult Donations_CreateById([DataSourceRequest]DataSourceRequest request, DonationViewModel orphan, int id)
        {


            return Json(new[] { orphan }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult Donations_AddNewDonation([DataSourceRequest]DataSourceRequest request, DonationViewModel donor, string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                var entity = new Donation();


                entity.SponsorId = Int32.Parse(id);
                entity.DonationDate = DateTime.Now;
                entity.DonationFor = "";
                entity.PaymentCategory = PaymentCategory.UnKnown;
                entity.PaymentMethod = PaymentMethod.UnKnown;
                entity.DonationAmount = 0;
                entity.LastUpdated = DateTime.Now;
                entity.LastUpdatedBy = User.Identity.Name;
                _Service.Save(entity);


                _Log.LogUsage("Add new Donation, Id: " + entity.Id);


            }

            return Json(new[] { donor }.ToDataSourceResult(request, ModelState));
        }
        //[HttpPost]
        //public ActionResult Donations_UpdateById([DataSourceRequest]DataSourceRequest request, Donation donation, int id)
        //{

        //    var modelStateErrors = this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors);

        //    if (modelStateErrors.Count() > 0)
        //    {
        //        foreach (var ert in modelStateErrors)
        //        {
        //            ModelState.AddModelError("error -", ert.ErrorMessage);
        //        }
        //    }



        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {


        //          //  var entity = _Service.GetSponsorActivitiesById(id);

        //            //  entity.SponsorId = sponsorActivity.SponsorId;
        //            //  entity.PostedDate = sponsorActivity.PostedDate;
        //            //    entity.Author = User.Identity.Name;

        //          //  entity.Description = sponsorActivity.Description;
        //         //   entity.ActivityName = sponsorActivity.ActivityName;

        //          //  _Service.SaveSponsorActivity(entity);




        //          //  _Log.LogUsage("Update SponsorActivity, Id: " + entity.Id);

        //        }
        //        catch (Exception ex)
        //        {

        //            Console.WriteLine(ex);
        //        }
        //        //db.Entry(entity).State = EntityState.Modified;
        //        //db.SaveChanges();
        //    }

        //    return Json(new[] { sponsorActivity }.ToDataSourceResult(request, ModelState));
        //}
        [HttpDelete]
        public ActionResult Donations_DestroyById([DataSourceRequest]DataSourceRequest request, SponsorActivityViewModel sponsorActivity, int id)
        {
            if (ModelState.IsValid)
            {
                try
                {


                    _Service.DeleteDonationById(id);


                    _Log.LogUsage("delete Donation, Id: " + id);

                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex);
                }
                // db.Sponsors.Remove(entity);
                // db.SaveChanges();
            }

            return Json(new[] { sponsorActivity }.ToDataSourceResult(request, ModelState));
        }

        //-------------------------------------------------


        public async Task<JsonResult> ServerFiltering_GetDonations(string text)
        {
            var donors = await _Service.GetDonationsAsync();//.Where(x => x.IsActive == true);


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
                Text = "ID: " + ct.Id + " " + ct.DonationFor + " " + ct.DonationType + " " + " " + ct.DonationDate + " " + ct.DonationAmount,
                Value = ct.Id.ToString(),
                Selected = false
            });



            if (!string.IsNullOrEmpty(text))
            {
                list = list.Where(p => p.Text.Contains(text));
            }

            return Json(list);//, JsonRequestBehavior.AllowGet);
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

        //     var president = _Service.GetDonationById(id.Value);

        //     if (president == null)
        //     {
        //         return NotFound();
        //     }

        //     return View(president);
        // }

        // [Route("/donation/{last:alpha}/{first:alpha}")]
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
        //         president = _Service.GetDonationById(id.Value);
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
        // public ActionResult Edit(Donation president)
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
        //             Donation toValue =
        //                 _Service.GetDonationById(president.Id);

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