using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoorTrust.DonationFund.Api.Services;

namespace NoorTrust.DonationFund.WebUi.Controllers
{
    [Authorize(Roles = "Staff,Admin,Volunteer")]
    public class HomeController : Controller
    {
        private ISponsorService _SponsorService;
        private IOrphanService _OrphanService;

        public HomeController(ISponsorService Service, IOrphanService OrphanService)                 
        {

            _SponsorService = Service;
            _OrphanService = OrphanService;

        }
        public IActionResult Index()
        {
            Statistics();

            List<NotificationCategory> ls = null;

            ls = Notifications1();


            return View(ls);

        }


        public IActionResult Orphan()
        {
            // ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }



        public List<NotificationCategory> Notifications1()
        {

            var ls = new List<NotificationCategory>();
            NotificationCategory nc = new NotificationCategory();
           // var duePaymentsACount = "..";
            ////-----------------------------------------------------------
            nc = new NotificationCategory();
            var orphansOlderThanFive = _OrphanService.ActiveOrphansOlderThan5Count();
            nc.CategoryID = 4;
            nc.CategoryName = "Orphans Older than 5 years old (" + orphansOlderThanFive + ")";
            //    nc.Products = orphansOlderThanFive;
            ls.Add(nc);
            //-----------------------------------------------------------
            nc = new NotificationCategory();
            var orphansOlderThan17 = _OrphanService.ActiveOrphansOlderThan17Count();
            nc.CategoryID = 5;
            nc.CategoryName = "Orphans Older than 17 years old (" + orphansOlderThan17 + ")";
            // nc.Products = orphansOlderThan17;
            ls.Add(nc);
            //-----------------------------------------------------------
            nc = new NotificationCategory();
            var penddingSponsors = new List<NotificationSubCategory>();//Calculate_Pendding_Sponsors();
            nc.CategoryID = 3;
            nc.CategoryName = "Pendding Sponsors with no orphans (" + penddingSponsors.Count + ")";
            //    nc.Products = penddingSponsors;
            ls.Add(nc);
            //-----------------------------------------------------------
            nc = new NotificationCategory();
            var sponsorsNeedsOrphans = new List<NotificationSubCategory>();//Calculate_SponsorsNeedsOrphans();
            nc.CategoryID = 3;
            nc.CategoryName = "Sponsors who Need Orphans (" + sponsorsNeedsOrphans.Count + ")";
            //  nc.Products = sponsorsNeedsOrphans;
            ls.Add(nc);
            //-----------------------------------------------------------
         


            return ls;
        }



        //public List<NotificationSubCategory> Calculate_orphansOlderThanFive()

        //{

        //    var orphansOlderThanFive = _orphanRepository.GetOrphansOlderThanFive();


        //    var products = new List<NotificationSubCategory>();
        //    NotificationSubCategory ncc = null;


        //    foreach (var oot in orphansOlderThanFive)
        //    {
        //        ncc = new NotificationSubCategory();
        //        ncc.CategoryID = 1;
        //        ncc.ProductID = 2;
        //        ncc.ProductName = "Orphan ID " + oot.OrphanId + " " + oot.ArFirstName + " " + oot.ArFatherName + " " + oot.ArGrandFather;
        //        products.Add(ncc);
        //    }

        //    return products;
        //}

        //public List<NotificationSubCategory> Calculate_OrphansOlderThan17()

        //{

        //    var orphansOlderThan17 = _orphanRepository.GetOrphansOlderThan17();


        //    var products = new List<NotificationSubCategory>();
        //    NotificationSubCategory ncc = null;


        //    foreach (var oot in orphansOlderThan17)
        //    {
        //        ncc = new NotificationSubCategory();
        //        ncc.CategoryID = 1;
        //        ncc.ProductID = 2;
        //        ncc.ProductName = "Orphan ID " + oot.OrphanId + " " + oot.ArFirstName + " " + oot.ArFatherName + " " + oot.ArGrandFather;// 
        //                                                                                                                                 // ncc.ProductName = "@<Text>@Html.ActionLink('Index', 'Index', 'Admin')</Text>";
        //        products.Add(ncc);
        //    }

        //    return products;
        //}        

        //public List<NotificationSubCategory> Calculate_Pendding_Sponsors()
        //{


        //    var penddingSponsors = _sponsorRepository.GetActiveUnSponsored();


        //    var products = new List<NotificationSubCategory>();
        //    NotificationSubCategory ncc = null;


        //    foreach (var oot in penddingSponsors)
        //    {
        //        ncc = new NotificationSubCategory();
        //        ncc.CategoryID = 1;
        //        ncc.ProductID = 2;
        //        ncc.ProductName = "Sponsor.ID " + oot.SponsorId + " Name: " + oot.Users.FirstName + " " + oot.Users.LastName;
        //        products.Add(ncc);
        //    }

        //    return products;

        //}

        //public List<NotificationSubCategory> Calculate_SponsorsNeedsOrphans()
        //{

        //    var penddingSponsors = _sponsorRepository.GetSponsoresNeedOrphans();


        //    var products = new List<NotificationSubCategory>();
        //    NotificationSubCategory ncc = null;


        //    foreach (var oot in penddingSponsors)
        //    {
        //        ncc = new NotificationSubCategory();
        //        ncc.CategoryID = 1;
        //        ncc.ProductID = 2;
        //        ncc.ProductName = "Sponsor.ID " + oot.SponsorId + " Name: " + oot.Users.FirstName + " " + oot.Users.LastName;
        //        products.Add(ncc);
        //    }

        //    return products;
        //}




        public async void Statistics()
        {

            ViewData["ActiveSponsorsCount"] =  _SponsorService.GetActiveSponsorsCount();
            ViewData["GetActiveSponsoredCount"] =  _SponsorService.GetActiveSponsoredCount();
            ViewData["GetActiveUnSponsoredCount"] =  _SponsorService.GetActiveUnSponsoredCount();


            ViewData["ActiveOrphansCount"] =  _OrphanService.GetActiveOrphansCount();
            ViewData["GetActiveSponsoredOrphansCount"] = _OrphanService.GetActiveSponsoredOrphansCount();
            ViewData["GetActiveUnSponsoredOrphansCount"] = _OrphanService.GetActiveUnSponsoredOrphansCount();
        
            ViewData["UnApprovedOrphansCount"] =  _OrphanService.GetUnApprovedOrphansCount();

            ViewData["ActiveOrphansOlderThan17Count"] = _OrphanService.ActiveOrphansOlderThan17Count();
            ViewData["ActiveOrphansOlderThan5Count"] = _OrphanService.ActiveOrphansOlderThan5Count();

        }

    }


    public class NotificationCategory
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }

        public virtual IList<NotificationSubCategory> Products { get; set; }

    }
    public class NotificationSubCategory
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        //  [ForeignKey("NotificationCategory")]
        public int CategoryID { get; set; }


    }

}
