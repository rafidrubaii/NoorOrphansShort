using NoorTrust.DonationFund.Api.Models;
using System.ComponentModel.DataAnnotations;

namespace NoorTrust.DonationFund.Api.DataAccess
{
    public class Country : Int32Identity
    {

        [MaxLength(100)]
        public string CountryName { get; set; }
        [MaxLength(100)]
        public string CountryArName { get; set; }

        [MaxLength(16)]
        public string ColorCode { get; set; }


    }
    public enum CountryEnum
    {
        _,UK,Iraq
    }
}