using NoorTrust.DonationFund.Api.DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NoorTrust.DonationFund.WebUi.Models
{
    public class CountryViewModel
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }
        [Required]
        public string CountryName { get; set; }
        [Required]
        [MaxLength(100)]
        public string CountryArName { get; set; }
      
        [MaxLength(16)]
        public string ColorCode { get; set; }


    }
}
