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
    public class SqlEntityFrameworkReportFileRepository : 
        SqlEntityFrameworkCrudRepositoryBase<ReportFile, NoorTrustDbContext>, IRepository<ReportFile>
    {
        public SqlEntityFrameworkReportFileRepository(
            NoorTrustDbContext context) : base(context)
        {

        }

        protected override DbSet<ReportFile> EntityDbSet
        {
            get
            {
                return Context.ReportFiles;
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
        public override IList<ReportFile> GetAll()
        {
            return ( from temp in EntityDbSet  orderby temp.UploadedDate   select temp ).ToList();
        }
        public override ReportFile GetById(int id)
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
