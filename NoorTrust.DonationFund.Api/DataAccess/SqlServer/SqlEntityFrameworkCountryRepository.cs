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
    public class SqlEntityFrameworkCountryRepository : 
        SqlEntityFrameworkCrudRepositoryBase<Country, NoorTrustDbContext>, IRepository<Country>
    {
        public SqlEntityFrameworkCountryRepository(
            NoorTrustDbContext context) : base(context)
        {

        }

        protected override DbSet<Country> EntityDbSet
        {
            get
            {
                return Context.Countries;
            }
        }

        public IList<Country> GetAllwithOrphans()
        {
            return (
                from temp in EntityDbSet
                        // .Include(x => x.Orphans)
                    //   .Include(p => p.Facts)
                orderby temp.Id
                select temp
                ).ToList();
        }
        public override IList<Country> GetAll()
        {
            return (
                from temp in EntityDbSet
                // .Include(x => x.Orphans)
                //  .Include(p => p.Donations)
                    //     .Include(x => x.Relationships)
                    //   .Include(p => p.Facts)
                orderby temp.Id//.FirstName, temp.LastUpdated
                select temp
                ).ToList();
        }
        public override async Task<IList<Country>> GetAllAsync()
        {
            return await (
                from temp in EntityDbSet
                // .Include(x => x.Countrys)
               //   .Include(p => p.Donations)
                    //     .Include(x => x.Relationships)
                    //   .Include(p => p.Facts)
                orderby temp.Id //.FirstName, temp.LastUpdated
                select temp 
                ).ToListAsync();
        }
        public override Country GetById(int id)
        {
            return (
                from temp in EntityDbSet
                 //   .Include(x => x.Relationships)
                     //   .ThenInclude(r1 => r1.ToCountry)
                  //  .Include(x => x.Relationships)
                   //     .ThenInclude(r => r.FromCountry)
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
                  //  where temp.IsActive == true
                    select temp
               ).Count();

        }
        //public override int GetActiveCountryedOrphansCount()

        //{
        //    return EntityDbSet.Count();


        //}


        //public override int GetActiveUnCountryedOrphansCount()

        //{
        //    return EntityDbSet.Count();


        //}
    }
}
