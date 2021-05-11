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
    public class SqlEntityFrameworkReportRepository : 
        SqlEntityFrameworkCrudRepositoryBase<Report, NoorTrustDbContext>, IRepository<Report>
    {
        public SqlEntityFrameworkReportRepository(
            NoorTrustDbContext context) : base(context)
        {

        }

        protected override DbSet<Report> EntityDbSet
        {
            get
            {
                return Context.Reports;
            }
        }

        //public override IList<Report> GetAll()
        //{
        //    return (
        //        from temp in EntityDbSet
        //      //      .Include(x => x.Relationships)
        //         //   .Include(p => p.Facts)
        //        orderby temp.LastName, temp.FirstName
        //        select temp
        //        ).ToList();
        //}
        public override IList<Report> GetAll()
        {
            return ( from temp in EntityDbSet

                      .Include(x => x.ReportFiles)

                     orderby temp.LastUpdatedDate   select temp ).ToList();
        }
        public override Report GetById(int id)
        {
            return (
                from temp in EntityDbSet
                 //   .Include(x => x.Relationships)
                     //   .ThenInclude(r1 => r1.ToReport)
                  //  .Include(x => x.Relationships)
                   //     .ThenInclude(r => r.FromReport)
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
    }
}
