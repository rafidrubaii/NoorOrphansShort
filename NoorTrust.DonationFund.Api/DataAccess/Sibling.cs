using NoorTrust.DonationFund.Api.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoorTrust.DonationFund.Api.DataAccess
{
    public class Sibling : Int32Identity
    {
        public int? OrphanId { get; set; }
        public Orphan Orphan { get; set; }


        public string Title { get; set; }

        
        public string FirstName { get; set; }
        public string FatherName { get; set; }

        public string GrandFather { get; set; }
        
        public string LastName { get; set; }

        
        public string ArTitle { get; set; }

        
        public string ArFirstName { get; set; }

        public string ArFatherName { get; set; }

        public string ArGrandFather { get; set; }
        public string ArLastName { get; set; }
        [DefaultValue("true")]
        public bool Gender { get; set; }

        
        public DateTime? DOB { get; set; }

        
        public DateTime? AcademicYear { get; set; }

       // public virtual Orphan Orphan { get; set; }

       // public virtual Orphan Orphan1 { get; set; }
    }
}
