using NoorTrust.DonationFund.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NoorTrust.DonationFund.Api.DataAccess
{
    public class Relationship : Int32Identity
    {

        public Relationship()
        {

        }

        public int FromSponsorId { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.ForeignKey("FromSponsorId")]
        public virtual Sponsor FromSponsor { get; set; }

        public int ToSponsorId { get; set; }
        [System.ComponentModel.DataAnnotations.Schema.ForeignKey("ToSponsorId")]
        public virtual Sponsor ToSponsor { get; set; }

        public string RelationshipType { get; set; }
    }
}
