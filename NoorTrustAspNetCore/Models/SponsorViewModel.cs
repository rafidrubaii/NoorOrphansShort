using NoorTrust.DonationFund.Api.DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NoorTrust.DonationFund.WebUi.Models
{
    public class SponsorViewModel
    {
        public int Id { get; set; }

        public string ThumbProfileImagepath { get; set; }
        public string Title { get; set; }

        [Required]
        [MaxLength(100)]
        //   [Index, un]
        public string FirstName { get; set; }

        [MaxLength(100)]
        public string MiddleName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        [MaxLength(100)]
        public string ArTitle { get; set; }
        [MaxLength(100)]
        public string ArFirstName { get; set; }
        [MaxLength(100)]
        public string ArLastName { get; set; }
        [MaxLength(200)]
        public string Address1 { get; set; }
        [MaxLength(200)]
        public string Address2 { get; set; }
        // public int? CityId { get; set; }
        [MaxLength(100)]
        public string City { get; set; }
        [MaxLength(40)]
        public string County { get; set; }

        // Use a sensible display name for views:
        [Display(Name = "Postal Code")]
        [MaxLength(20)]
        public string PostalCode { get; set; }

        // public CountryEnum CountryId { get; set; }
        [UIHint("CountryId")]
        public int? Country { get; set; }

        [MaxLength(20)]
        public string HomePhone { get; set; }

        [MaxLength(20)]
        public string MobileNumber { get; set; }


        [MaxLength(50)]
        [EmailAddress]
        public string OtherEmail { get; set; }

        [Required(ErrorMessage = "A valid Email is required")]
        [EmailAddress]
        [MaxLength(50)]
        public string Email { get; set; }

        
       

        [DefaultValue("false")]
        public bool IsGiftAid { get; set; }
        [MaxLength(100)]
        public string GiftAidRef { get; set; }
        [MaxLength(100)]
        public string NameOnBankStatement { get; set; }
        [MaxLength(100)]
        public string PaymentName { get; set; }
        [DefaultValue("true")]
        public bool IsReceiveEmail { get; set; }
        [DefaultValue("true")]
        public bool IsReceiveMobile { get; set; }
        [DefaultValue("true")]
        public bool IsReceivePost { get; set; }
        // [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy hh:mm:ss}")]
        // [Index]
        public DateTime? RegisterDate { get; set; }
        // [Index]
        public bool IsActive { get; set; }
        public bool InActive
        {
            get
            {
                return !IsActive;
            }
        }
        public DateTime? DeactivatedDate { get; set; }

      
        public int? OrphanGenderChoice { get; set; }

        public int? OrphanCityChoiceId { get; set; }

        public short? NumberOfOrphans { get; set; }

        [DefaultValue(0)]
        public int CalculatedNumberOfOrphans { get; set; }
        //public DateTime SRegistarDate { get; set; }
        //[Index]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy hh:mm:ss}")]
        public DateTime? SStartDate { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy hh:mm:ss}")]
        public DateTime? SEndDate { get; set; }

        public string LanguagePref { get; set; }

        public int? EthnicityId { get; set; }

        public string ReferralType { get; set; }

        public string Notes { get; set; }



        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy hh:mm:ss}")]
        public DateTime? LastUpdated { get; set; }

        [MaxLength(128)]
        public string LastUpdatedBy { get; set; }

        public string Comments { get; set; }

      

        public virtual IEnumerable<Sponsor> Sponsors { get; set; }

        public virtual IEnumerable<Donation> Donations { get; set; }


        public virtual IEnumerable<UserLog> UserLogs { get; set; }

        public virtual IEnumerable<Payment> Payments { get; set; }
    }
}
