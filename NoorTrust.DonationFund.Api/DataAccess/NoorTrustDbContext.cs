using NoorTrust.DonationFund.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NoorTrust.DonationFund.Api.DataAccess
{
    public class NoorTrustDbContext : DbContext, INoorTrustDbContext
    {

        public NoorTrustDbContext(DbContextOptions options) :
            base(options)
        {

        }


        public DbSet<Sponsor> Sponsors { get; set; }
            public DbSet<Orphan> Orphans { get; set; }
                public DbSet<OrphanFile> OrphanFiles { get; set; }
                public DbSet<OrphanActivity> OrphanActivities { get; set; }
                public DbSet<Sibling> Siblings { get; set; }

            public DbSet<SponsorActivity> SponsorActivities { get; set; }

            public DbSet<Donation> Donations { get; set; }
            public DbSet<Payment> Payments { get; set; }
            public DbSet<Transaction> Transactions { get; set; }


        public DbSet<Report> Reports { get; set; }
        public DbSet<ReportFile> ReportFiles { get; set; }

        public DbSet<Relationship> Relationships { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<LogEntry> LogEntries { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }

        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<EducationLevel> EducationLevels { get; set; }



        // public DbSet<SponsorFact> SponsorFacts { get; set; }

        // public DbSet<DonationType> DonationCategories { get; set; }

        //  public DbSet<DonorActivity> DonorActivities { get; set; }
        // public DbSet<Ethnicity> Ethnicities { get; set; }






        //  public DbSet<PaymentCategory> PaymentCategories { get; set; }
        //   public DbSet<PaymentMethod> PaymentMethods { get; set; }




        //public DbSet<sysdiagram> sysdiagrams { get; set; }
        //   public DbSet<UserCategory> UserCategories { get; set; }
        //  public DbSet<UserLog> UserLogs { get; set; }
        //  public DbSet<ColorCode> ColorCodes { get; set; }
        // public DbSet<EducationLevel> EducationLevels { get; set; }

        //  public DbSet<EducationStatus> EducationStatuss { get; set; }



        public override int SaveChanges()
        {
            //  CleanupOrphanedSponsorFacts();
            //  CleanupOrphanedRelationships();

            return base.SaveChanges();
        }

        //private void CleanupOrphanedSponsorFacts()
        //{
        //    var deleteThese = new List<SponsorFact>();

        //    foreach (var deleteThis in SponsorFacts.Local.Where(pf => pf.Sponsor == null))
        //    {
        //        deleteThese.Add(deleteThis);
        //    }

        //    foreach (var deleteThis in deleteThese)
        //    {
        //        SponsorFacts.Remove(deleteThis);
        //    }
        //}

        private void CleanupOrphanedRelationships()
        {
            var deleteThese = new List<Relationship>();

            foreach (var deleteThis in Relationships.Local
                .Where(r => r.FromSponsor == null))
            {
                deleteThese.Add(deleteThis);
            }

            foreach (var deleteThis in deleteThese)
            {
                Relationships.Remove(deleteThis);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Sponsor>(entity =>
            //{
            //    entity.ToTable("Sponsor");
            //    //  .HasOne(a => a.Sponsor)
            //    // .WithOne(b => b.Sponsor).HasForeignKey(d => d.SponsorId);
            //});

            //   modelBuilder.Entity<SponsorFact>().ToTable("SponsorFact");



            modelBuilder.Entity<Feature>().ToTable("Feature");

            modelBuilder.Entity<LogEntry>().ToTable("LogEntry");

            modelBuilder.Entity<Relationship>(entity =>
            {
                entity.ToTable("Relationship");

                entity.Property(e => e.RelationshipType)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.FromSponsor)
                    .WithMany(p => p.Relationships)
                    .HasForeignKey(d => d.FromSponsorId)
                    .OnDelete(DeleteBehavior.Restrict);


                // THE PROBLEM IS HERE
                // SOMEHOW NEED TO MAP THE "TO" RELATIONSHIP
                entity.HasOne(d => d.ToSponsor);
            });
        }
    }
}
