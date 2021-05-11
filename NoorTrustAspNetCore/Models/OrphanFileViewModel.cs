using Microsoft.AspNetCore.Mvc.Rendering;
using NoorTrust.DonationFund.Api.DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NoorTrust.DonationFund.WebUi.Models
{
       
    public class OrphanFileViewModel
    {
        public int Id { get; set; }
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
