using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoorTrust.DonationFund.Api.DataAccess
{

    public class Donor
    {
        [Key]
        public int DonorId { get; set; }
      
        [ForeignKey("Persons")]
        public int? PersonId { get; set; }

       // [Index]
        public DateTime? DonationDate { get; set; }

        public string Notes { get; set; }

        public DonationType DonationType { get; set; }

        public DateTime? LastUpdated { get; set; }

        [MaxLength(128)]
        public string LastUpdatedBy { get; set; }

        //[Timestamp]
        //public byte[] RowVersion { get; set; }

        public virtual Person Persons  { get; set; }


        //public virtual IList<Donation> Donations { get; set; }


        //public virtual IList<DonorActivity> DonorActivities { get; set; }
    }
}
