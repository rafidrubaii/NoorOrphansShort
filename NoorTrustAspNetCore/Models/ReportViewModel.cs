
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using NoorTrust.DonationFund.Api.DataAccess;

namespace NoorTrust.DonationFund.WebUi.Models
{
    public class ReportViewModel
    {
        public int ReportId { get; set; }
        [Required]
        [MaxLength(128)]
        public string ReportName { get; set; }
        [MaxLength(128)]
        public string FolderName { get; set; }
        [Required]
        [MaxLength(600)]

        public string Description { get; set; }
        [AllowHtml]
        [MaxLength(4000)]
        public string Content { get; set; }
        public bool? IsSendWithAttachment { get; set; }
        public DateTime CreatedDate { get; set; }
        [DefaultValue(1)]
        public ReportType ReportType { get; set; }
        [DisplayName("Report List Type")]
        [UIHint("ReportListTypeId")]
        public ReportListType ReportListType { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        [MaxLength(128)]
        public string LastUpdatedBy { get; set; }
        public bool IsActive { get; set; }

        public string ReportFileOrginFileName { get; set; }
        public string ReportFileName { get; set; }
        public IList<ReportFile> UploadInitialFilesList { get; set; }

        private class AllowHtmlAttribute : Attribute
        {
        }
        //  [UIHint("ReportTypeViewModel")]
        //  public ReportTypeViewModel ReportType { get; set; }

    }
}

