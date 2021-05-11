using NoorTrust.DonationFund.Api.DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NoorTrust.DonationFund.WebUi.Models
{
    public class OrphanActivityViewModel
    {
        public int Id { get; set; }


        public int OrphanId { get; set; }
       
        public DateTime PostedDate { get; set; }


        public string ActivityName { get; set; }

        public string Description { get; set; }
        public string Author { get; set; }
    }
}
