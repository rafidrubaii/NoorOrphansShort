using NoorTrust.DonationFund.Api.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoorTrust.DonationFund.Api.DataAccess
{
    public class Person :  Int32Identity
    {
        private static readonly DateTime DEFAULT_DATE_VALUE = DateTime.MinValue;

        public Person()
        {
            FirstName = String.Empty;
            MiddleName= String.Empty;
            LastName = String.Empty;
          //  this.Sponsors = new List<Sponsor>();
            this.Donations = new List<Donation>();
            this.Payments = new List<Payment>();
            //  Relationships = new List<Relationship>();
            //  Facts = new List<PersonFact>();
        }

     //// public Sponsor Sponsor { get; set; }
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

        public Country Country { get; set; }

        [MaxLength(20)]
        public string HomePhone { get; set; }

        [MaxLength(20)]
        public string MobileNumber { get; set; }


        [MaxLength(100)]
        public string URL { get; set; }

        [Required(ErrorMessage = "A valid Email is required")]
        [EmailAddress]
        [MaxLength(40)]
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
        // [ForeignKey("UserCategory")]
        //public int? UserCategoryId { get; set; }

        // [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy hh:mm:ss}")]
        // [Index]
        public DateTime? RegistarDate { get; set; }
        // [Index]
        public bool IsActive { get; set; }

        public DateTime? DeactivatedDate { get; set; }

        public int? OrphanGenderChoice { get; set; }

        public int? OrphanCityChoiceId { get; set; }

        public short? NumberOfOrphans { get; set; }

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

        //[Timestamp]
        //public byte[] RowVersion { get; set; }
        // [ForeignKey("UserId")]
        // public virtual Person Persons { get; set; }



        public virtual IList<Orphan> Orphans { get; set; }



        public virtual IList<SponsorActivity> SponsorActivities { get; set; }

    
        public virtual IEnumerable<Sponsor> Sponsors { get; set; }

        public virtual IEnumerable<Donation> Donations { get; set; }   


       

        public virtual IEnumerable<Payment> Payments { get; set; }



        public virtual List<Relationship> Relationships
        {
            get;
            set;
        }

        public void AddRelationship(string relationshipType, Person person)
        {
            if (string.IsNullOrEmpty(relationshipType))
                throw new ArgumentException("relationshipType is null or empty.", "relationshipType");
            if (person == null)
                throw new ArgumentNullException("person", "person is null.");

            var relationship = new Relationship();

            relationship.ToPerson = person;
            relationship.FromPerson = this;
            relationship.RelationshipType = relationshipType;

            Relationships.Add(relationship);
        }

        //public void AddFact(string factType, string factValue)
        //{
        //    AddFact(0, factType, factValue, DEFAULT_DATE_VALUE, DEFAULT_DATE_VALUE);
        //}

        //public void AddFact(string factType, DateTime factDate)
        //{
        //    AddFact(factType, factDate, factDate);
        //}

        //public void AddFact(int id, string factType, DateTime factStartDate, DateTime factEndDate)
        //{
        //    AddFact(id, factType, factType, factStartDate, factEndDate);
        //}

        //public void AddFact(string factType, DateTime factStartDate, DateTime factEndDate)
        //{
        //    AddFact(0, factType, factType, factStartDate, factEndDate);
        //}

        //private bool AllowDuplicateFactType(string factType)
        //{
        //    if (factType == PresidentsConstants.President)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //public void RemoveFact(int id)
        //{
        //    if (id == 0)
        //    {
        //        return;
        //    }
        //    else
        //    {
        //        var match = Facts.Where(fact => fact.Id == id).FirstOrDefault();

        //        if (match != null)
        //        {
        //            Facts.Remove(match);
        //        }
        //    }
        //}

        //private void UpdateExistingFactById(int id, string factType, string factValue, DateTime factStartDate, DateTime factEndDate)
        //{
        //    PersonFact fact = null;

        //    bool foundIt = false;

        //    if (id != 0)
        //    {
        //        // locate existing fact 
        //        fact = (from temp in Facts
        //                where temp.Id == id
        //                select temp).FirstOrDefault();
        //    }

        //    if (fact == null)
        //    {
        //        fact = new PersonFact();

        //        fact.Person = this;
        //        fact.Id = id;
        //    }
        //    else
        //    {
        //        foundIt = true;
        //    }

        //    fact.FactType = factType;
        //    fact.FactValue = factValue;
        //    fact.StartDate = factStartDate;
        //    fact.EndDate = factEndDate;

        //    if (foundIt == false)
        //    {
        //        Facts.Add(fact);
        //    }
        //}

        //private void UpdateExistingFactByFactType(int id, string factType, string factValue, DateTime factStartDate, DateTime factEndDate)
        //{
        //    bool foundIt = false;

        //    // locate existing fact 
        //    PersonFact fact = (from temp in Facts
        //                       where temp.FactType == factType
        //                       select temp).FirstOrDefault();

        //    if (fact == null)
        //    {
        //        fact = new PersonFact();

        //        fact.Person = this;
        //        fact.Id = id;
        //    }
        //    else
        //    {
        //        foundIt = true;
        //    }

        //    fact.FactType = factType;
        //    fact.FactValue = factValue;
        //    fact.StartDate = factStartDate;
        //    fact.EndDate = factEndDate;

        //    if (foundIt == false)
        //    {
        //        Facts.Add(fact);
        //    }
        //}

        //public void AddNewFact(int id,
        //    string factType,
        //    string factValue,
        //    DateTime factStartDate,
        //    DateTime factEndDate)
        //{
        //    var fact = new PersonFact();

        //    fact.Person = this;
        //    fact.Id = id;

        //    fact.FactType = factType;
        //    fact.FactValue = factValue;
        //    fact.StartDate = factStartDate;
        //    fact.EndDate = factEndDate;

        //    Facts.Add(fact);
        //}

        //public void AddFact(string factType, string factValue, DateTime factStartDate, DateTime factEndDate)
        //{
        //    AddFact(0, factType, factValue, factStartDate, factEndDate);
        //}

        //public void AddFact(int id,
        //    string factType,
        //    string factValue,
        //    DateTime factStartDate,
        //    DateTime factEndDate)
        //{
        //    if (string.IsNullOrEmpty(factType))
        //        throw new ArgumentException("factType is null or empty.", "factType");

        //    if (factValue == null)
        //    {
        //        throw new ArgumentNullException("factValue", "Argument cannot be null.");
        //    }

        //    if (id != 0)
        //    {
        //        UpdateExistingFactById(id, factType, factValue, factStartDate, factEndDate);
        //    }
        //    else if (AllowDuplicateFactType(factType) == false)
        //    {
        //        UpdateExistingFactByFactType(id, factType, factValue, factStartDate, factEndDate);
        //    }
        //    else
        //    {
        //        AddNewFact(id, factType, factValue, factStartDate, factEndDate);
        //    }
        //}
    }
}
