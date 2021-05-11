using NoorTrust.DonationFund.Api.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoorTrust.DonationFund.Api.DataAccess
{

    public class Orphan : Int32Identity
    {
        //[Key]
        //public int OrphanId { get; set; }
      //  [Index]
        public int? SponsorId { get; set; }
        public Sponsor Sponsor { get; set; }

        [MaxLength(10)]
        public string Title { get; set; }
        [MaxLength(100)]
        public string FirstName { get; set; }
        [MaxLength(100)]

        public string FatherName { get; set; }
        [MaxLength(100)]

        public string GrandFather { get; set; }
        [MaxLength(100)]
        public string LastName { get; set; }
        public string MotherName { get; set; }
        [MaxLength(100)]
        public string ArTitle { get; set; }
        public string ArFirstName { get; set; }
        [MaxLength(100)]
        public string ArFatherName { get; set; }
        [MaxLength(100)]
        public string ArGrandFather { get; set; }
        [MaxLength(100)]
        public string ArLastName { get; set; }
        [MaxLength(100)]
        public string ArMotherName { get; set; }


        [DefaultValue(0)]
        public int? OrphanFileId { get; set; }
        [DefaultValue("true")]
        public bool Gender { get; set; }

       // [Index]
        public DateTime? DOB { get; set; }
        [NotMapped]
        public int Age
        {
            get
            {
                DateTime today = DateTime.Today;

                var age = 0;
                var dob = today;
                if (DOB.HasValue)
                {
                    dob = (DateTime)DOB;

                    age = today.Year - dob.Year;

                    if (today.Month < dob.Month || (today.Month == dob.Month && today.Day < dob.Day))
                        age--;

                }


                return age;
            }
        }
       
        [MaxLength(100)]
        public string BirthPlace { get; set; }


        [MaxLength(100)]
        public string FatherPrevWork { get; set; }

        [MaxLength(500)]
        public string FatherDeath { get; set; }
        [MaxLength(100)]
        public string MotherWork { get; set; }

        public EducationLevelEnum EducationLevel { get; set; }

        public EducationStatus EducationStatus { get; set; }
        public DateTime? AcademicYear { get; set; }
        [MaxLength(100)]
        public string Guardian { get; set; }

        [MaxLength(100)]
        public string GuardianWork { get; set; }

        [MaxLength(100)]
        public string GuardianName { get; set; }

        [MaxLength(500)]
        public string Hobbies { get; set; }
        [DefaultValue("false")]
        public bool IsMotherDead { get; set; }


       

        [DefaultValue("false")]
        public bool IsSayed { get; set; }

        public Ethnicity Ethnicity { get; set; }

        [MaxLength(200)]
        public string Address { get; set; }

        // public CountryEnum CountryId { get; set; }
       
        public int? CountryId { get; set; }

        // [Index]
        public CityEnum CityId { get; set; }

        public DistrictEnum DistrictId { get; set; }

     
      //  public string MedicalState { get; set; }


        public DateTime? OrphanedDate { get; set; }
       // [Index]
        public DateTime? RegistarDate { get; set; }
       // [Index]
        public DateTime? SponsoredDate { get; set; }
        [DefaultValue("false")]
        public bool IsDisabled { get; set; }
        [DefaultValue("false")]
        public bool IsStudent { get; set; }
        [DefaultValue("false")]
        public bool IsException { get; set; }

        [MaxLength(500)]
        public string ExceptionReason { get; set; }
        public int? NoBrothers { get; set; }
        public int? NoSisters { get; set; }
        public int? NoSiblings { get; set; }

        [MaxLength(300)]
        public string LivingSituation { get; set; }

        public string Notes { get; set; }
        [DefaultValue("false")]
        public bool IsApproved { get; set; }
        // [Index]
        [DefaultValue("true")]
        public bool IsActive { get; set; }
      
        public DateTime? DeactivatedDate { get; set; }

        public DateTime? LastUpdated { get; set; }

        [MaxLength(128)]
        public string LastUpdatedBy { get; set; }

        //[Timestamp]
        //public byte[] RowVersion { get; set; }

        public virtual IList<OrphanFile> OrphanFiles { get; set; }

        //public virtual City City { get; set; }

        //public virtual Country Country { get; set; }

        //public virtual District District { get; set; }

        //public virtual Ethnicity Ethnicity { get; set; }

        //public virtual EducationLevel EducationLevel { get; set; }

        //public virtual EducationStatus EducationStatus { get; set; }

       

      //  [ForeignKey("SponsorId")]
      //  public virtual Sponsor Sponsor { get; set; }

        public virtual IList<Sibling> Siblings { get; set; }

        public virtual IList<OrphanActivity> OrphanActivities { get; set; }


      


       
    }
}
