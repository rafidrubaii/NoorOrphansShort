using NoorTrust.DonationFund.Api.Models;

namespace NoorTrust.DonationFund.Api.DataAccess
{
    public class City : Int32Identity
    {   

        public string CityName { get; set; }
        public string CityArName { get; set; }

        // [ForeignKey("Country")]
        public int? CountryId { get; set; }


        public string ColorCode { get; set; }

        public string LatLng { get; set; }
    }
    public enum CityEnum
    {
        _,
        Baghdad_بغداد,
        Kirkuk_كركوك,
        Deyala_ديالى,
        Salahdeen_صلاح_الدين,
        AlNajaf_النجف,
        AlQadisiya_القادسية,
        Karbala_كربلاء,
        Babil_بابل,
        Balad_بلد,
    }
}