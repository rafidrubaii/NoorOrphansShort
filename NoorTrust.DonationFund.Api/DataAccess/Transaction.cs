using NoorTrust.DonationFund.Api.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace NoorTrust.DonationFund.Api.DataAccess
{
    public class Transaction : Int32Identity
    {
        public int? SponsorId { get; set; }
        public Sponsor Sponsor { get; set; }

       // [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      //  [Index]
      //  public int TransactionId { get; set; }
      //  [Key, Column(Order = 1)]
     //   public int? SponsorId { get; set; }
      //  [Key, Column(Order = 2)]
        public string PaymentName { get; set; }
      //  [Key, Column(Order = 3)]
        public decimal PaymentAmount { get; set; }

        // [Index]
      //  [Key, Column(Order = 4)]
        public DateTime PaymentDate { get; set; }
      //  [Key, Column(Order = 5)]
        public PaymentCategory PaymentCategory { get; set; }
      //  [Key, Column(Order = 6)]
        public PaymentMethod PaymentMethod { get; set; }

        public string Memo { get; set; }

        //public virtual PaymentCategory PaymentCategory { get; set; }

        //public virtual PaymentMethod PaymentMethod { get; set; }

    }




}

