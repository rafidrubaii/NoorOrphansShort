using NoorTrust.DonationFund.Api.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoorTrust.DonationFund.Api.DataAccess
{

    public class OrphanFile : Int32Identity

    {
       
      //  [Index]
        public int? OrphanId { get; set; }

      //  public int? OrphanActivityId { get; set; }

        public Guid FileName { get; set; }

        public string FileType { get; set; }

        public int? FileSize { get; set; }
        public DateTime UploadedDate { get; set; }

        public string FileURL { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

    //    public virtual Orphan Orphan { get; set; }

      //  public virtual OrphanActivity OrphanActivity { get; set; }
    }
}
