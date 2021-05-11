using NoorTrust.DonationFund.Api.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoorTrust.DonationFund.Api.DataAccess
{

    public class Report : Int32Identity
    {
      
        [MaxLength(128)]
        public string ReportName { get; set; }
        [MaxLength(128)]
        public string   FolderName { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }
       
        public string Content { get; set; }
       
        public bool? IsSendWithAttachment { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public string LastUpdatedBy { get; set; }
        public ReportType ReportType { get; set; }

        public ReportListType ReportListType { get; set; }
        [DefaultValue("true")]
        public bool IsActive { get; set; }

        //  public virtual ReportType ReportType { get; set; }

        //   public virtual ReportListType ReportListType { get; set; }

        public virtual List<ReportFile> ReportFiles { get; set; }
    }
}
