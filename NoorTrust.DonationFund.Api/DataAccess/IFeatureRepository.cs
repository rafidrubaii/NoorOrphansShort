using NoorTrust.DataAccess;
using NoorTrust.DonationFund.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NoorTrust.DonationFund.Api.DataAccess
{
    public interface IFeatureRepository : IRepository<Feature>
    {
        IList<Feature> GetByUsername(string username);
    }
}
