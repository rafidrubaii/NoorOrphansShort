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

using System.IO;


using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoorTrust.DonationFund.Api.DataAccess;
using Microsoft.AspNetCore.Mvc.Rendering;
using NoorTrust.DonationFund.Api.Services;
using Microsoft.AspNetCore.Http;
using NoorTrust.DonationFund.WebUi.Models;

namespace NoorTrust.DonationFund.WebUI.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    
    public class ReportAdminController : Controller
    {
        // private NoorOrphanFundDbContext db = new NoorOrphanFundDbContext();
      
        private readonly IReportService _ReportService;
        public ReportAdminController(IReportService reportService)
        {


            _ReportService = reportService;
        }
        public ActionResult Index()
        {
            var list = ((ReportListType[]) Enum.GetValues(typeof(ReportListType))).Select(c => new SelectListItem { Value = c.GetHashCode().ToString(), Text = c.ToString() }).ToList();

            ViewData["ReportListTypesSelectList"] = list;

            return View();
        }

        public ActionResult SaveAndPersistReportFiles(IEnumerable<IFormFile> files, int id)
        {
            // The Name of the Upload component is "files"
            if (files != null)
            {
                var folderName = _reportServices.Repository.GetFolderNameFromReportId(id);

                foreach (var file in files)
                {
                    // Some browsers send file names with full path.
                    // We are only interested in the file name. id.ToString() + "__" +
                    var orginFileName = Path.GetFileName(file.FileName);
                    var fileName =  Path.GetFileNameWithoutExtension(file.FileName.Replace(" ", "_")) + "__" + Guid.NewGuid().ToString();
                    var fileExtension = Path.GetExtension(file.FileName);
                    var physicalPath = Path.Combine(Server.MapPath(SettingsManager.BaseReportPath), folderName + "\\" + fileName + fileExtension);

                    // var fileSize = 0; 
                    // SessionUploadInitialFilesRepository.Add("Donors", new UploadInitialFile(fileName, file.ContentLength, fileExtension));

                    try
                    {
                        // The files are saved 
                        file.SaveAs(physicalPath);
                        System.Threading.Thread.Sleep(1000);

                        ReportFile reportFile = new ReportFile();
                        reportFile.ReportId = id;// (int) TempData["DonorID"];

                        reportFile.ReportFileOrginFileName = orginFileName;
                        reportFile.ReportFileExtension = fileExtension;

                        reportFile.ReportFileName = fileName + fileExtension;
                        reportFile.ReportFileSize = new FileInfo(physicalPath).Length;
                        reportFile.ReportFileDescription = "";
                       
                        //  gifAidFormFile.UploadedDate = DateTime.Now;
                        //var UserId = User.Identity.GetUserId();
                        //if (UserId != null)
                        //    reportFile.UpdatedBy = UserId;

                        _reportRepository.AddReportFile(reportFile);
                        _reportRepository.SaveChanges();


                        //Add user log

                    //    var LoggedInUserId = User.Identity.GetUserId();
                    //    if (LoggedInUserId != null)
                    //        _userRepository.AddUserLog(LoggedInUserId, DateTime.Now, "ReportFile " + reportFile.ReportFileId + " Record Created and file uploaded for Report id " + reportFile.ReportId);

                    //}
                    catch (Exception ex)
                    {
                        Session["Message"] = ex.Message.ToString();

                     //   ErrorSignal.FromCurrentContext().Raise(ex);
                        //   var text = @"Sheet name and column header must match the following: 'Donor Database' ";
                        //  Session["Message"] =  Session["Message"] + text;
                        ViewBag.Message = (string) Session["Message"];
                        ViewData["Message"] = (string) Session["Message"];
                        // System.Threading.Thread.Sleep(2000);

                    }

                    //RedirectToAction("InitialFilesDonors");


                }
            }
            //if (Session["Message"] != null)
            //{
            //    //(string) Session["Message"]
            //    return Content(ViewBag.Message);
            //}
            //Session["Message"] = null;
            // Return an empty string to signify success
            return Content("");
        }
        //   public ActionResult RemoveAndPersistReportFiles(string[] fileNames, int id)
        public ActionResult RemoveAndPersistReportFiles(string[] fileNames)
        {
            //   var fileNames = _reportRepository.ReportFiles.Where(x => x.DonorID == id).Select(x => x.ReportFileName).ToArray();

            // The parameter of the Remove action must be called "fileNames"
            if (fileNames != null)
            {


                foreach (var fullName in fileNames)
                {
                    var folderName = _reportRepository.GetReporFolderNametfromFileName(fullName);
                    if (String.IsNullOrEmpty(folderName)) break;
                    var fileName = Path.GetFileName(fullName);
                    var physicalPath = Path.Combine(Server.MapPath(SettingsManager.BaseReportPath), folderName + "\\" + fileName);

                    // SessionUploadInitialFilesRepository.Remove(fileName);

                    if (System.IO.File.Exists(physicalPath))
                    {
                        // The files are not actually removed in this demo
                        System.IO.File.Delete(physicalPath);

                        var gifAidFormFile = _reportRepository.GetReporFiletfromFileName(fullName);
                        if (gifAidFormFile != null)
                        {
                            _reportRepository.RemoveReportFile(gifAidFormFile);
                            _reportRepository.SaveChanges();



                            ////Add user log
                            //var LoggedInUserId = User.Identity.GetUserId();
                            //if (LoggedInUserId != null)
                            //    _userRepository.AddUserLog(LoggedInUserId, DateTime.Now, "ReportFile " + gifAidFormFile.ReportId + " Record and file deleted ");

                        }
                    }

                }
            }

            // Return an empty string to signify success
            return Content("");
        }
        public FileStreamResult DownloadFile(string fileUploadString)
        {

            var attachment = _reportRepository.GetReporFiletfromFileName(fileUploadString);
            if (attachment != null)
            {
                var folderName = _reportRepository.GetReporFolderNametfromFileName(fileUploadString);
                if (String.IsNullOrEmpty(folderName)) return null;
                var filePhysicalPath = Path.Combine(Server.MapPath(SettingsManager.BaseReportPath), folderName + "\\" + attachment.ReportFileName);


                string contentType = "application/octetstream";
                var fileStreamResult = new FileStreamResult(new FileStream(filePhysicalPath, FileMode.Open, FileAccess.Read), contentType);

                fileStreamResult.FileDownloadName = attachment.ReportId.ToString() + "__" + attachment.ReportFileOrginFileName;

                return fileStreamResult;
            }

            return null;
        }


        public ActionResult Reports_Read([DataSourceRequest]DataSourceRequest request)
        {
            IQueryable<Report> reports = _reportRepository.GetAllReports();
            DataSourceResult result = reports.ToDataSourceResult(request, c => new ReportViewModel
            {
                ReportId = c.Id,
                ReportName = c.ReportName,
                Description = c.Description,
                Content = c.Content,
                FolderName = c.FolderName,
                IsSendWithAttachment = c.IsSendWithAttachment,

                ReportType = c.ReportType,
                ReportListType = c.ReportListType,
                CreatedDate = c.CreatedDate,
                LastUpdatedDate = c.LastUpdatedDate,
                LastUpdatedBy = c.LastUpdatedBy,
                IsActive = c.IsActive,
                UploadInitialFilesList = GenerateList(c.Id),
            });

            return Json(result);
        }
        public List<ReportFile> GenerateList(int? id)
        {
            //  if (Session["GetUploadInitialFilesList"] == null) Session["GetUploadInitialFilesList"] = new List<GifAidFormFile>();
            //  return  ((IList<GifAidFormFile>) Session["GetUploadInitialFilesList"]).Where(x => x.DonorID == id).ToList();
            if ((!id.HasValue) || (id == null))
                return new List<ReportFile>();

            var result = _reportRepository.GetReportFiles(id.Value).ToList();

            return (result == null) ? new List<ReportFile>() : result;
        }
        [HttpPost]
        public ActionResult Reports_CreatePrint([DataSourceRequest]DataSourceRequest request, ReportViewModel report)
        {
            if (ModelState.IsValid)
            {

                //  var orginFileName = Path.GetFileName(file.FileName);
                //   var fileName = id.ToString() + "__" + Path.GetFileNameWithoutExtension(file.FileName.Replace(" ", "_")) + "__" + Guid.NewGuid().ToString();
                //  var fileExtension = Path.GetExtension(file.FileName);

                var folderName = Guid.NewGuid().ToString();

                var physicalPath = Path.Combine(Server.MapPath(SettingsManager.BaseReportPath), folderName);


                if (!Directory.Exists(physicalPath))
                {
                    Directory.CreateDirectory(physicalPath);

                    var entity = new Report
                    {
                        ReportName = report.ReportName,
                        FolderName = folderName,
                        Description = report.Description,
                        Content = report.Content,

                        ReportType = ReportType.Print,
                        ReportListType = report.ReportListType,

                        CreatedDate = DateTime.Now,
                        LastUpdatedDate = DateTime.Now,

                        LastUpdatedBy = User.Identity.GetUserId(),
                        IsActive = report.IsActive
                    };

                    _reportRepository.Add(entity);
                    _reportRepository.SaveChanges();
                    report.ReportId = entity.Id;
                }
            }

            return Json(new[] { report }.ToDataSourceResult(request, ModelState));
        }
        [HttpPost]
        public ActionResult Reports_CreateEmail([DataSourceRequest]DataSourceRequest request, ReportViewModel report)
        {
            //if (ModelState.IsValid)
            //{
            //    var entity = new Report
            //    {
            //        ReportName = report.ReportName,
            //        Description = report.Description,
            //        Content = report.Content,
            //        //   Url = report.Url,
            //        ReportType = report.ReportType,
            //        ReportListType = report.ReportListType,
            //        //   UploadDate = report.UploadDate,
            //        LastUpdatedBy = report.LastUpdatedBy,
            //        IsActive = report.IsActive
            //    };

            //    _reportRepository.Add(entity);
            //    _reportRepository.SaveChanges();
            //    report.ReportId = entity.ReportId;
            //}

            return Json(new[] { report }.ToDataSourceResult(request, ModelState));
        }
        [HttpPost]
        public ActionResult Reports_Update([DataSourceRequest]DataSourceRequest request, ReportViewModel report)
        {
            if (ModelState.IsValid)
            {


                var rep = _reportRepository.GetReportById(report.ReportId);


                rep.ReportId = report.ReportId;
                rep.ReportName = report.ReportName;
                rep.Description = report.Description;
                rep.Content = report.Content;
                //   Url = report.Url,
                rep.ReportType = ReportType.Print;//. 1,// report.ReportTypeId,
                rep.ReportListType = report.ReportListType;

                rep.IsActive = report.IsActive;

                var LoggedInUserId = User.Identity.GetUserId();

                rep.LastUpdatedDate = DateTime.Now;
                rep.LastUpdatedBy = LoggedInUserId;
                //  _reportRepository.Update(entity);
                // _reportRepository.Entry(entity).State = EntityState.Modified;

                _reportRepository.SaveChanges();

                if (LoggedInUserId != null)
                    _userRepository.AddUserLog(LoggedInUserId, DateTime.Now, "ReportFile " + rep.ReportId + " Record updated ");


            }

            return Json(new[] { report }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult Reports_Destroy([DataSourceRequest]DataSourceRequest request, ReportViewModel report)
        {
            var count = _reportRepository.GetReportFiles(report.ReportId).Count();

            if (ModelState.IsValid && count == 0)
            {
                var entity = _reportRepository.GetReportById(report.ReportId);
                if (entity != null)
                {

                    _reportRepository.Remove(entity);

                    _reportRepository.SaveChanges();

                    var physicalPath = Path.Combine(Server.MapPath(SettingsManager.BaseReportPath), entity.FolderName);


                    if (Directory.Exists(physicalPath))
                    {
                        Directory.Delete(physicalPath);

                    }
                }
            }
            return Json(new[] { report }.ToDataSourceResult(request, ModelState));
        }

        protected override void Dispose(bool disposing)
        {
            // _reportRepository.Dispose(disposing);
            base.Dispose(disposing);
        }
    }
}
