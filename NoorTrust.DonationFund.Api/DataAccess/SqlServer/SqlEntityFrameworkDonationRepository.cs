using NoorTrust.DonationFund.Common;
using NoorTrust.DonationFund.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NoorTrust.DataAccess;
using NoorTrust.DataAccess.SqlServer;

namespace NoorTrust.DonationFund.Api.DataAccess.SqlServer
{
    public class SqlEntityFrameworkDonationRepository : 
        SqlEntityFrameworkCrudRepositoryBase<Donation, NoorTrustDbContext>, IRepository<Donation>
    {
        public SqlEntityFrameworkDonationRepository(
            NoorTrustDbContext context) : base(context)
        {

        }
        /// <summary>
        /// //////////////
        /// </summary>
        protected override DbSet<Donation> EntityDbSet
        {
            get
            {
                return Context.Donations;
            }
        }

        public override IList<Donation> GetAll()
        {
            return (
                from temp in EntityDbSet
              //      .Include(x => x.Relationships)
                 //   .Include(p => p.Facts)
                orderby temp.Id//.Sponsors.LastName, temp.Sponsors.FirstName
                select temp
                ).ToList();
        }

        public override Donation GetById(int id)
        {
            return (
                from temp in EntityDbSet
                 //   .Include(x => x.Relationships)
                     //   .ThenInclude(r1 => r1.ToSponsor)
                  //  .Include(x => x.Relationships)
                   //     .ThenInclude(r => r.FromSponsor)
                   // .Include(p => p.Facts)
                where temp.Id == id
                select temp
                ).FirstOrDefault();
        }
    }
}
