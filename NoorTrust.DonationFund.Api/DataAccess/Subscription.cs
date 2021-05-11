using NoorTrust.DonationFund.Api.Models;
using System;
using System.Linq;

namespace NoorTrust.DonationFund.Api.DataAccess
{
    public class Subscription : Int32Identity
    {
        public string Username { get; set; }
        public string SubscriptionLevel { get; set; }
    }
}
