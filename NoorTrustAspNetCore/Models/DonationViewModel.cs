using NoorTrust.DonationFund.Api.DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NoorTrust.DonationFund.WebUi.Models
{
    public class DonationViewModel
    {
        public int Id { get; set; }

        public int? SponsorId { get; set; }
        //   [ForeignKey("Persons")]
        //  public int? Id { get; set; }

        public DonationType DonationType { get; set; }
        // [Index]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy hh:mm:ss}")]
        public DateTime DonationDate { get; set; }

        public decimal DonationAmount { get; set; }
      //  [MaxLength(128)]
        public string DonationFor { get; set; }

        public PaymentCategory PaymentCategory { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public string Notes { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy hh:mm:ss}")]
        public DateTime? LastUpdated { get; set; }

        [MaxLength(128)]
        public string LastUpdatedBy { get; set; }
    }
}
