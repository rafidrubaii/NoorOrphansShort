using NoorTrust.DataAccess;
using NoorTrust.DonationFund.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace NoorTrust.DonationFund.Api.Models
{
    public abstract class Int32Identity : IInt32Identity
    {
       
        public int Id { get; set; }
    }
}
