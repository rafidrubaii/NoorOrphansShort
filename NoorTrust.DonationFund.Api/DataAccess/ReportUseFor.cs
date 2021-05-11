using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoorTrust.DonationFund.Api.DataAccess
{

    public class ReportUseFor
    {
        [Key]
        public int ReportUseForId { get; set; }

        public string Name { get; set; }
     
    }
}
