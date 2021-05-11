using NoorTrust.DonationFund.Api.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoorTrust.DonationFund.Api.DataAccess
{

    public class Donation : Int32Identity
    {
        //[Key]
        //public int DonationId { get; set; }


        //[ForeignKey("Sponsors")]
        //public int? Id { get; set; }

        public int? SponsorId { get; set; }
        public Sponsor Sponsor { get; set; }

        public DonationType DonationType { get; set; }
        // [Index]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy hh:mm:ss}")]
        public DateTime DonationDate { get; set; }

        public decimal DonationAmount { get; set; }
        [MaxLength(128)]
        public string DonationFor { get; set; }      

        public PaymentCategory PaymentCategory { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public string Notes { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy hh:mm:ss}")]
        public DateTime? LastUpdated { get; set; }

        [MaxLength(128)]
        public string LastUpdatedBy { get; set; }
        // [Timestamp]
        // public Byte[] RowVersion { get; set; }
        //public virtual PaymentMethod PaymentMethod { get; set; }

        //public virtual PaymentCategory PaymentCategory { get; set; }


      //  public virtual Sponsor Sponsor { get; set; }

      
    }
}
