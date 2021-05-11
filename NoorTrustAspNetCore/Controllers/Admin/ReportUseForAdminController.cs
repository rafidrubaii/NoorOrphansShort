using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using NoorOrphanFund.Core.Models;
using NoorOrphanFund.Data;
using NoorOrphanFund.Client.ViewModels.Client;
 
namespace NoorTrust.DonationFund.WebUI.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class ReportUseForAdminController : Controller
    {
        private NoorOrphanFundDbContext db = new NoorOrphanFundDbContext();

        public ActionResult Index()
        {
            return View();
        }

        //public ActionResult ReportUseFors_Read([DataSourceRequest]DataSourceRequest request)
        //{
        //    IQueryable<ReportUseFor> reporttypes = db.ReportUseFors;
        //    DataSourceResult result = reporttypes.ToDataSourceResult(request, c => new ReportUseForViewModel 
        //    {
        //        ReportUseForId = c.ReportUseForId,
        //        Name = c.Name
        //    });

        //    return Json(result);
        //}

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ReportUseFors_Create([DataSourceRequest]DataSourceRequest request, ReportUseForViewModel reportType)
        {
            if (ModelState.IsValid)
            {
                var entity = new ReportUseFor
                {
                    Name = reportType.Name
                };

                db.ReportUseFors.Add(entity);
                db.SaveChanges();
                reportType.ReportUseForId = entity.ReportUseForId;
            }

            return Json(new[] { reportType }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ReportUseFors_Update([DataSourceRequest]DataSourceRequest request, ReportUseForViewModel reportType)
        {
            if (ModelState.IsValid)
            {
                var entity = new ReportUseFor
                {
                    ReportUseForId = reportType.ReportUseForId,
                    Name = reportType.Name
                };

                db.ReportUseFors.Attach(entity);
                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
            }

            return Json(new[] { reportType }.ToDataSourceResult(request, ModelState));
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
