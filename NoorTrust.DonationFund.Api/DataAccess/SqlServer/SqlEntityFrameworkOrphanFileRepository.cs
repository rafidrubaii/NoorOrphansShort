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
    public class SqlEntityFrameworkOrphanFileRepository : 
        SqlEntityFrameworkCrudRepositoryBase<OrphanFile, NoorTrustDbContext>, IRepository<OrphanFile>
    {
        public SqlEntityFrameworkOrphanFileRepository(
            NoorTrustDbContext context) : base(context)
        {

        }

        protected override DbSet<OrphanFile> EntityDbSet
        {
            get
            {
                return Context.OrphanFiles;
            }
        }

        public override IList<OrphanFile> GetAll()
        {
            return (
                from temp in EntityDbSet
                  //  .Include(x => x.OrphanFiles)
                 //   .Include(p => p.Facts)
                orderby temp.FileName, temp.Id
                select temp
                ).ToList();
        }

        public override OrphanFile GetById(int id)
        {
            return (
                from temp in EntityDbSet
                 //   .Include(x => x.Relationships)
                     //   .ThenInclude(r1 => r1.ToOrphanFile)
                  //  .Include(x => x.Relationships)
                   //     .ThenInclude(r => r.FromOrphanFile)
                   // .Include(p => p.Facts)
                where temp.Id == id
                select temp
                ).FirstOrDefault();
        }
    }
}
