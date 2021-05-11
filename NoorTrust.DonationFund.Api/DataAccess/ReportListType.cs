using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoorTrust.DonationFund.Api.DataAccess
{

    //public class ReportListType
    //{
    //    [Key]
    //    public int ReportListTypeId { get; set; }

    //    public string Name { get; set; }

    //}

    public enum ReportListType { Unkown = 0, Sponsors = 1, Orphans = 2 }
}
