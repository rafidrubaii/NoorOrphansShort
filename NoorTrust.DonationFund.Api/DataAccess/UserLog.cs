using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoorTrust.DonationFund.Api.DataAccess
{
    public class UserLog
    {
        public int UserLogId { get; set; }

       
        public string UserId { get; set; }
        // [Index]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy hh:mm:ss}")]
        public DateTime TimeStamp { get; set; }

        public string ActionName { get; set; }

      //  [ForeignKey("UserId")]
      //  public virtual AspNetUser User { get; set; }
    }
}
