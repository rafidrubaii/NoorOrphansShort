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
    public class SqlEntityFrameworkSponsorRepository : 
        SqlEntityFrameworkCrudRepositoryBase<Sponsor, NoorTrustDbContext>, IRepository<Sponsor>
    {
        public SqlEntityFrameworkSponsorRepository(
            NoorTrustDbContext context) : base(context)
        {

        }

        protected override DbSet<Sponsor> EntityDbSet
        {
            get
            {
                return Context.Sponsors;
            }
        }

        public IList<Sponsor> GetAllwithOrphans()
        {
            return (
                from temp in EntityDbSet
                         .Include(x => x.Orphans)
                    //   .Include(p => p.Facts)
                orderby temp.LastName, temp.FirstName
                select temp
                ).ToList();
        }
        public override IList<Sponsor> GetAll()
        {
            return (
                from temp in EntityDbSet
                 .Include(x => x.Orphans)
                //  .Include(p => p.Donations)
                    //     .Include(x => x.Relationships)
                    //   .Include(p => p.Facts)
                orderby temp.FirstName, temp.LastUpdated
                select temp
                ).ToList();
        }
        public override async Task<IList<Sponsor>> GetAllAsync()
        {
            return await (
                from temp in EntityDbSet
                // .Include(x => x.Sponsors)
               //   .Include(p => p.Donations)
                    //     .Include(x => x.Relationships)
                    //   .Include(p => p.Facts)
                orderby temp.FirstName, temp.LastUpdated
                select temp 
                ).ToListAsync();
        }
        public override Sponsor GetById(int id)
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
        public override async Task<int> CountAsync()
        {
            return await (
                EntityDbSet
                 
               ).CountAsync();
        }
        public override int CountActive()
        {
            return (from temp in
               EntityDbSet
                    where temp.IsActive == true
                    select temp
               ).Count();

        }
        //public override int GetActiveSponsoredOrphansCount()

        //{
        //    return EntityDbSet.Count();


        //}


        //public override int GetActiveUnSponsoredOrphansCount()

        //{
        //    return EntityDbSet.Count();


        //}
    }
}
