using NoorTrust.DonationFund.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NoorTrust.DataAccess;
using NoorTrust.DataAccess.SqlServer;

namespace NoorTrust.DonationFund.Api.DataAccess.SqlServer
{
    public class SqlEntityFrameworkFeatureRepository :
            SqlEntityFrameworkCrudRepositoryBase<Feature, NoorTrustDbContext>, IFeatureRepository
    {
        public SqlEntityFrameworkFeatureRepository(
            NoorTrustDbContext context) : base(context)
        {

        }

        protected override DbSet<Feature> EntityDbSet => Context.Features;

        public IList<Feature> GetByUsername(string username)
        {
            return (
                from temp in EntityDbSet
                where (temp.Username == username || temp.Username == String.Empty)
                select temp
                ).ToList();
        }
    }
}
