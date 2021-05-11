vusing System;
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

namespace NoorOrphanFund.Client.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class ReportTypeAdminController : Controller
    {
        private NoorOrphanFundDbContext db = new NoorOrphanFundDbContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ReportTypes_Read([DataSourceRequest]DataSourceRequest request)
        {
            IQueryable<ReportType> reporttypes = db.ReportTypes;
            DataSourceResult result = reporttypes.ToDataSourceResult(request, c => new ReportTypeViewModel 
            {
                ReportTypeId = c.ReportTypeId,
                Name = c.Name
            });

            return Json(result);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ReportTypes_Create([DataSourceRequest]DataSourceRequest request, ReportTypeViewModel reportType)
        {
            if (ModelState.IsValid)
            {
                var entity = new ReportType
                {
                    Name = reportType.Name
                };

                db.ReportTypes.Add(entity);
                db.SaveChanges();
                reportType.ReportTypeId = entity.ReportTypeId;
            }

            return Json(new[] { reportType }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ReportTypes_Update([DataSourceRequest]DataSourceRequest request, ReportTypeViewModel reportType)
        {
            if (ModelState.IsValid)
            {
                var entity = new ReportType
                {
                    ReportTypeId = reportType.ReportTypeId,
                    Name = reportType.Name
                };

                db.ReportTypes.Attach(entity);
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
