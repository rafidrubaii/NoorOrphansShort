using NoorTrust.DonationFund.Api.DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NoorTrust.DonationFund.WebUi.Models
{
    public class PersonActivityViewModel
    {
        public int Id { get; set; }
        //  [Index]
        public int SponsorId { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy hh:mm:ss}")]
        public DateTime PostedDate { get; set; }

        [MaxLength(128)]
        public string ActivityName { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }

        [MaxLength(128)]
        public string Author { get; set; }
    }
}
