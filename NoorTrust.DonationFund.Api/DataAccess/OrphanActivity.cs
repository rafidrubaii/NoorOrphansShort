using NoorTrust.DonationFund.Api.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoorTrust.DonationFund.Api.DataAccess
{

    public class OrphanActivity : Int32Identity
    {

     
        public int OrphanId { get; set; }      
        public Orphan Orphan { get; set; }

        public DateTime PostedDate { get; set; }


        public string ActivityName { get; set; }

        public string Description { get; set; }
        public string Author { get; set; }

       
       //// public virtual IList<ArchiveFile> ArchiveFiles { get; set; }

      //  public virtual Orphan Orphan { get; set; }
    }
}
