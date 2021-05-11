using NoorTrust.DonationFund.Api.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace NoorTrust.DonationFund.Common
{
    public interface INoorTrustDbContext
    {
         DbSet<Sponsor> Sponsors { get; set; }
         DbSet<Orphan> Orphans { get; set; }
         DbSet<OrphanFile> OrphanFiles { get; set; }
        DbSet<OrphanActivity> OrphanActivities { get; set; }
         DbSet<Sibling> Siblings { get; set; }

         DbSet<SponsorActivity> SponsorActivities { get; set; }

         DbSet<Donation> Donations { get; set; }
         DbSet<Payment> Payments { get; set; }
         DbSet<Transaction> Transactions { get; set; }


         DbSet<Report> Reports { get; set; }
         DbSet<ReportFile> ReportFiles { get; set; }

         DbSet<Relationship> Relationships { get; set; }
         DbSet<Feature> Features { get; set; }
         DbSet<LogEntry> LogEntries { get; set; }
         DbSet<Subscription> Subscriptions { get; set; }

         DbSet<Country> Countries { get; set; }
         DbSet<City> Cities { get; set; }
         DbSet<EducationLevel> EducationLevels { get; set; }

        EntityEntry Entry(object entity);
        int SaveChanges();
    }
}
