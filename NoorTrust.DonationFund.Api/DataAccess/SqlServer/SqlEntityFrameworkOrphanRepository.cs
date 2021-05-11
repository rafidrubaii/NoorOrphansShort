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
    public class SqlEntityFrameworkOrphanRepository : 
        SqlEntityFrameworkCrudRepositoryBase<Orphan, NoorTrustDbContext>, IRepository<Orphan>
    {
        public SqlEntityFrameworkOrphanRepository(
            NoorTrustDbContext context) : base(context)
        {

        }

        protected override DbSet<Orphan> EntityDbSet
        {
            get
            {
                return Context.Orphans;
            }
        }

        public override IList<Orphan> GetAll()
        {
            return (
                from temp in EntityDbSet
                 //   .Include(x => x.OrphanFiles)
                 //   .Include(p => p.Facts)
                orderby temp.LastName, temp.FirstName
                select temp
                ).ToList();
        }

        public override Orphan GetById(int id)
        {
            return (
                from temp in EntityDbSet
                 //   .Include(x => x.Relationships)
                     //   .ThenInclude(r1 => r1.ToOrphan)
                  //  .Include(x => x.Relationships)
                   //     .ThenInclude(r => r.FromOrphan)
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
        public override async Task<int> CountActiveAsync()
        {
            return await (from temp in
                EntityDbSet
                          where temp.IsActive==true select temp
               ).CountAsync();
        }
        public override  int CountActive()
        {
            return  (from temp in
                EntityDbSet
                          where temp.IsActive == true
                          select temp
               ).Count();
        }

        //public override int  GetActiveSponsoredOrphansCount()
        //{

        //    return (from temp in
        //     EntityDbSet
        //            where temp.IsActive == true && temp.SponsorId!=null
        //            select temp
        //     ).Count();

        //}
        //public override int GetActiveUnSponsoredOrphansCount()
        //{

        //    return (from temp in
        //     EntityDbSet
        //            where temp.IsActive == true && temp.SponsorId == null
        //            select temp
        //     ).Count();

        //}
    }
}
