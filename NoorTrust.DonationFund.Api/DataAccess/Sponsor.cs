using NoorTrust.DonationFund.Api.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoorTrust.DonationFund.Api.DataAccess
{

    public class Sponsor : Int32Identity
    {

        private static readonly DateTime DEFAULT_DATE_VALUE = DateTime.MinValue;

        public Sponsor()
        {
            FirstName = String.Empty;
            MiddleName = String.Empty;
            LastName = String.Empty;
            this.Orphans = new List<Orphan>();
            this.Donations = new List<Donation>();
            this.Payments = new List<Payment>();
            //  Relationships = new List<Relationship>();
           
        }

        //// public Sponsor Sponsor { get; set; }
        [MaxLength(20)]
        public string Title { get; set; }

        [Required]
        [MaxLength(50)]
        //   [Index, un]
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string MiddleName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [MaxLength(20)]
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
        [MaxLength(50)]
        public string County { get; set; }
        // Use a sensible display name for views:
        [Display(Name = "Postal Code")]
        [MaxLength(20)]
        public string PostalCode { get; set; }

       //  public CountryEnum CountryId { get; set; }
       
        public int? Country { get; set; }


        [MaxLength(50)]
        public string HomePhone { get; set; }

        [MaxLength(50)]
        public string MobileNumber { get; set; }


        [MaxLength(100)]
        public string OtherEmail { get; set; }

        [Required(ErrorMessage = "A valid Email is required")]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }
        [DefaultValue("false")]
        public bool IsGiftAid { get; set; }
        [MaxLength(100)]
        public string GiftAidRef { get; set; }
        [MaxLength(100)]
        public string NameOnBankStatement { get; set; }
        [MaxLength(200)]
        public string PaymentName { get; set; }

        [DefaultValue("true")]
        public bool IsReceiveEmail { get; set; }
        [DefaultValue("true")]
        public bool IsReceiveMobile { get; set; }
        [DefaultValue("true")]
        public bool IsReceivePost { get; set; }     

      
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy hh:mm:ss}")]
      
        public DateTime? RegisterDate { get; set; }
      
        public bool IsActive { get; set; }

        public DateTime? DeactivatedDate { get; set; }

        public int? OrphanGenderChoice { get; set; }

        public int? OrphanCityChoiceId { get; set; }

        public short? NumberOfOrphans { get; set; }

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

        //[Timestamp]
        //public byte[] RowVersion { get; set; }
      
        public virtual IList<Orphan> Orphans { get; set; }

        public virtual IList<SponsorActivity> SponsorActivities { get; set; }
       

        public virtual IEnumerable<Donation> Donations { get; set; }

        public virtual IEnumerable<Payment> Payments { get; set; }


        public virtual List<Relationship> Relationships { get; set; }
        

        public void AddRelationship(string relationshipType, Sponsor person)
        {
            if (string.IsNullOrEmpty(relationshipType))
                throw new ArgumentException("relationshipType is null or empty.", "relationshipType");
            if (person == null)
                throw new ArgumentNullException("person", "person is null.");

            var relationship = new Relationship();

            relationship.ToSponsor = person;
            relationship.FromSponsor = this;
            relationship.RelationshipType = relationshipType;

            Relationships.Add(relationship);
        }
    }
}
