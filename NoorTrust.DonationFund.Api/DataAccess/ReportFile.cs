using NoorTrust.DonationFund.Api.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoorTrust.DonationFund.Api.DataAccess
{
    public class ReportFile : Int32Identity

    {
        public int? ReportId { get; set; }
        public Report Report { get; set; }

        [MaxLength(128)]
        public string ReportFileName { get; set; }
        [MaxLength(128)]
        public string ReportFileOrginFileName { get; set; }
        [MaxLength(500)]
        public string ReportFileDescription { get; set; }

        public long ReportFileSize { get; set; }
        [MaxLength(5)]
        public string ReportFileExtension { get; set; }

        [DefaultValue("false")]
        public bool IsAttachment { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy hh:mm:ss}")]
        public DateTime? UploadedDate { get; set; }

        [MaxLength(128)]
        public string UpdatedBy { get; set; }
      
    }
}
