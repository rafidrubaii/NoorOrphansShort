using NoorTrust.DonationFund.Api.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace NoorTrust.DonationFund.Api.DataAccess
{
    public class Payment : Int32Identity
    {
        public int? SponsorId { get; set; }
        public Sponsor Sponsor { get; set; }
        //  [Index]
        public PaymentCategory PaymentCategory { get; set; }
      
       
        public int Units { get; set; }

       
        public decimal PricePerUnit { get; set; }

      
        public decimal TotalDue { get; set; }

        public decimal PaymentAmount { get; set; }

        // [Index]
        public DateTime PaymentGroup { get; set; }

       // [Index]
        public DateTime PaymentDate { get; set; }

        public DateTime PaymentDueDate { get; set; }
        //   [Index]
        public DateTime? PaymentEndDate { get; set; }
     //   [Index]
        public PaymentMethod PaymentMethod { get; set; }

        public string PaymentName { get; set; }

        public string Notes { get; set; }

        public bool IsPending { get; set; }
        //    [Index]
        public bool IsPaymentEnded { get; set; }
        //     [Timestamp]   //    public Byte[] RowVersion { get; set; }

        public DateTime? VerifiedUpToDate { get; set; }
        //public virtual PaymentCategory PaymentCategory { get; set; }

        //public virtual PaymentMethod PaymentMethod { get; set; }

       // public virtual Sponsor Sponsors { get; set; }

        // public virtual ICollection<Orphan> Orphans { get; set; }
    }
}
