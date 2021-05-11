
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
    [Authorize(Roles = "Admin")]
    // [Authorize(Roles = SecurityConstants.RoleName_Admin)]
    public class CountryController : Controller
    {


        private ICountryService _Service;
        private ILogger _Log;

        public CountryController(ICountryService Service, ILogger Log
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

            return View();
        }

        public void BuildViewData(ViewDataDictionary viewDataDictionary)
        {

        }
        public async Task<IActionResult> Countries_ReadAsync([DataSourceRequest]DataSourceRequest request)
        {
            var donations = await _Service.GetCountriesAsync();
            DataSourceResult result = donations.ToDataSourceResult(request, country => new CountryViewModel
            {
                Id = country.Id,
                CountryName = country.CountryName,
                CountryArName = country.CountryArName,
                ColorCode = country.ColorCode

            });

            return Json(result);
        }

        public IActionResult Countries_ReadById([DataSourceRequest]DataSourceRequest request, int id)
        {
            var donations = _Service.GetCountries().Where(x=>x.Id==id);
            DataSourceResult result = donations.ToDataSourceResult(request, country => new CountryViewModel
            {

                Id = country.Id,
                CountryName = country.CountryName,
                CountryArName = country.CountryArName,
                ColorCode = country.ColorCode


            });

            return Json(result);
        }


        //[AllowAnonymous]
        //public ActionResult Index()
        //{
        //    var presidents = _Service.GetCountries();

        //    return View();// presidents);
        //}

        [HttpPost]
        //[AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Countries_Create([DataSourceRequest]DataSourceRequest request, Country country)
        {
            if (ModelState.IsValid)
            {
                var entity = new Country
                {

                    Id = country.Id,
                    CountryName = country.CountryName,
                    CountryArName = country.CountryArName,
                    ColorCode = country.ColorCode

                };

                _Service.Save(entity);
                //db.SaveChanges();
                country.Id = entity.Id;
            }

            return Json(new[] { country }.ToDataSourceResult(request, ModelState));
        }

        // [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public ActionResult Countries_Update([DataSourceRequest]DataSourceRequest request, Country country)
        {
            if (ModelState.IsValid)
            {
                var entity = new Country
                {

                    Id = country.Id,
                    CountryName = country.CountryName,
                    CountryArName = country.CountryArName,
                    ColorCode = country.ColorCode

                };

                _Service.Save(entity);
                //db.Entry(entity).State = EntityState.Modified;
                //db.SaveChanges();
            }

            return Json(new[] { country }.ToDataSourceResult(request, ModelState));
        }
        [HttpPost]
        public ActionResult Countries_UpdateById([DataSourceRequest]DataSourceRequest request, Country country, int id)
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
                //  var donations = _Service.GetCountriesById(id);

                var entity = _Service.GetCountryById(id);


                entity.Id = country.Id;
                entity.CountryName = country.CountryName;
                entity.CountryArName = country.CountryArName;
                entity.ColorCode = country.ColorCode;

                _Service.Save(entity);

            }

            return Json(new[] { country }.ToDataSourceResult(request, ModelState));
        }
        //  [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public ActionResult Countries_Destroy([DataSourceRequest]DataSourceRequest request, Country country)
        {
            if (ModelState.IsValid)
            {
              

                _Service.DeleteCountryById(country.Id);
               
            }

            return Json(new[] { country }.ToDataSourceResult(request, ModelState));
        }

        //----------------------------------------------------



        //public IActionResult Countries_CreateById([DataSourceRequest]DataSourceRequest request, int id)
        //{

        //    var donations = _Service.GetCountriesById(id);

        //    DataSourceResult result = donations.ToDataSourceResult(request, donation => new DonationViewModel
        //    {
        //        Id = donation.Id,
        //        DonationDate = DateTime.Now,


        //    });



        //    return Json(result);
        //}
        public ActionResult Countries_CreateById([DataSourceRequest]DataSourceRequest request, DonationViewModel orphan, int id)
        {


            return Json(new[] { orphan }.ToDataSourceResult(request, ModelState));
        }

        //[HttpPost]
        //public ActionResult Countries_AddNewDonation([DataSourceRequest]DataSourceRequest request, DonationViewModel donor, string id)
        //{
        //    if (!String.IsNullOrEmpty(id))
        //    {
        //        var entity = new Country();


        //        entity.SponsorId = Int32.Parse(id);
        //        entity.DonationDate = DateTime.Now;
        //        entity.DonationFor = "";
        //        entity.PaymentCategory = PaymentCategory.UnKnown;
        //        entity.PaymentMethod = PaymentMethod.UnKnown;
        //        entity.DonationAmount = 0;
        //        entity.LastUpdated = DateTime.Now;
        //        entity.LastUpdatedBy = User.Identity.Name;
        //        _Service.Save(entity);


        //        _Log.LogUsage("Add new Country, Id: " + entity.Id);


        //    }

        //    return Json(new[] { donor }.ToDataSourceResult(request, ModelState));
        //}
        //[HttpPost]
        //public ActionResult Countries_UpdateById([DataSourceRequest]DataSourceRequest request, Country donation, int id)
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
        public ActionResult Countries_DestroyById([DataSourceRequest]DataSourceRequest request, SponsorActivityViewModel sponsorActivity, int id)
        {
            if (ModelState.IsValid)
            {
                try
                {


                    _Service.DeleteCountryById(id);


                    _Log.LogUsage("delete Country, Id: " + id);

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

        [Authorize(Roles = "Admin,Staff,Volunteer")]
        public async Task<JsonResult> ServerFiltering_GetCountries(string text)
        {
            var counts = await _Service.GetCountriesAsync();//.Where(x => x.IsActive == true);


            var list = counts.Select(ct => new SelectListItem
            {
                Text = ct.CountryName,
                Value = ct.Id.ToString(),
                Selected = false
            });



            if (!string.IsNullOrEmpty(text))
            {
                list = list.Where(p => p.Text.Contains(text));
            }

            return Json(list);//, JsonRequestBehavior.AllowGet());
        }




    }
}