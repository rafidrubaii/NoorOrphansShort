using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace NoorTrust.DonationFund.WebUi.Models
{
    public class OrphanAssignViewModel
    {
        
        public int Id { get; set; }
        [UIHint("OrphanAssign")]
        public int? SponsorId { get; set; }
        public int? _SponsorId { get; set; }
    }
}