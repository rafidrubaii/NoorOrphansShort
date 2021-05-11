using NoorTrust.DonationFund.Api.Models;

namespace NoorTrust.DonationFund.Api.DataAccess
{
    public class EducationLevel : Int32Identity
    {
       
      
        public string EducationLevelName { get; set; }

      
    }
    public enum EducationLevelEnum
    {
        Unknown,
        Nursery,
        Year_1,
        Year_2,
        Year_3,
        Year_4,
        Year_5,
        Year_6,
        Year_7,
        Year_8,
        Year_9,
        Year_10,
        Year_11,
        Year_12
    }
}