using NoorTrust.DonationFund.Api.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoorTrust.DonationFund.Api.DataAccess
{
 
   public class SponsorActivity : Int32Identity
    {
        public int? SponsorId { get; set; }
        public Sponsor Sponsor { get; set; }
        //   public int SponsorActivityId { get; set; }
        //  [Index]
        //   public int SponsorId { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy hh:mm:ss}")]
        public DateTime PostedDate { get; set; }

        [MaxLength(128)]
        public string ActivityName { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }

        [MaxLength(128)]
        public string Author { get; set; }

       // public virtual Sponsor Sponsor { get; set; }
    }
}
