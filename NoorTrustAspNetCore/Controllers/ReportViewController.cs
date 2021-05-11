using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
//using System.Web.Mvc;
using Kendo.Mvc.UI;

using Kendo.Mvc.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoorTrust.DonationFund.Api.DataAccess;
using NoorTrust.DonationFund.Api.Services;
using Microsoft.AspNetCore.Http;
using NoorTrust.DonationFund.WebUi.Models;

namespace NoorTrust.DonationFund.WebUI.Controllers
{
    [Authorize(Roles = "Staff,Admin")]

    public class ReportViewController : Controller
    {
        private IReportService _Service;

        public ReportViewController(IReportService Service)
        {
            _Service = Service;
        }
        // GET: ReportView
        public ActionResult Index()
        {
            //  var par = Request.QueryString["list"];


            // this.Profile..reportViewer1.LocalReport.SetParameters(parameters);


            //_reportRepository.dbcities.Select(ct => new SelectListItem
            //{
            //    Text = ct.CityName,
            //    Value = ct.CityId.ToString(),
            //    Selected = false
            //}).ToList();



            return View();
        }
        public ActionResult Report()
        {
            return View();
        }
        public ActionResult DisplayReport()
        {
            var ss = Request.Query["d"];
            var url = Request.Query["list"];

            string decoded = System.Web.HttpUtility.UrlDecode(url);
            return View();
        }
        public ActionResult OrphanCards()
        {
            //    var url = Request.QueryString["orphanList"];

            //    string decoded = System.Web.HttpUtility.UrlDecode(url);
            //    return View();
            //}
            //public ActionResult SponsorLabels()
            //{

            //    var par = Request.QueryString["sponsorList"];

            var queryString = HttpContext.Request.Query;
            //StringValues someInt;
            //queryString.TryGetValue("someInt", out someInt);



            //var daRealInt = int.Parse(someInt);

            return View();//alTQzwWLGb7k6Z*yZn9;Y@edxKO65=Ii
        }

        public static string GetQuerystringValue(string key)
        {

            //if (null != HttpContext.Request..Current)
            //{
            //    return Request.Query["list"];
            //}
            return null;
        }
        public ActionResult Reports_Read([DataSourceRequest]DataSourceRequest request)
        {
            IList<Report> report = _Service.GetReports().Where(x => x.IsActive == true).OrderBy(k=>k.CreatedDate).ToList();
            try
            {
                if (report != null)
                {
                    DataSourceResult result = report.ToDataSourceResult(request, even =>   new ReportViewModel
                    {

                        ReportId = even.Id,
                        ReportName = even.ReportName,
                        Description = even.Description,
                        ReportListType = even.ReportListType,
                        FolderName = even.FolderName,
                        ReportFileName=even.ReportFiles[0].ReportFileName,
                        ReportFileOrginFileName=even.ReportFiles[0].ReportFileOrginFileName,
                        Content = even.Content,
                        //  ReportFiles = GetReportFiles(even.Id),//even.Ideven.ReportFiles != null ? even.ReportFiles.Where(x => x.ReportFileExtension == ".trdp" && x.ReportId == even.Id) : new List<ReportFile>(),
                        IsSendWithAttachment = even.IsSendWithAttachment,
                        LastUpdatedDate = even.LastUpdatedDate,
                        LastUpdatedBy = even.LastUpdatedBy,
                        ReportType = even.ReportType,
                        CreatedDate = even.CreatedDate,
                        IsActive = even.IsActive
                    });

                    return Json(result);
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);


            }
        
            return View();
    }

    private IList<ReportFile> GetReportFiles(int id)
    {
        return _Service.GetReportFiles(id).Where(x => x.ReportFileExtension == ".trdp").ToList();
        // even.Ideven.ReportFiles != null ? even.ReportFiles.Where(x => x.ReportFileExtension == ".trdp" && x.ReportId == even.Id)
    }
}
}