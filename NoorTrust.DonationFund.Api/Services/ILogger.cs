using NoorTrust.DonationFund.Common;
using NoorTrust.DonationFund.Api.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoorTrust.DonationFund.Api.Services
{
    public interface ILogger
    {
        void LogFeatureUsage(string featureName);
        void LogUsage(string featureName);
        void LogCustomerSatisfaction(string feedback);
    }
}
