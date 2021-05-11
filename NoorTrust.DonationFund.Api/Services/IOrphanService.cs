using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NoorTrust.DonationFund.Api.DataAccess;
using NoorTrust.DonationFund.Api.Models;

namespace NoorTrust.DonationFund.Api.Services
{
    public interface IOrphanService
    {
        void Save(Orphan saveThis);
       // void DeleteOrphanById(int id);
        Orphan GetOrphanById(int id);
        IList<Orphan> GetOrphans();

        Task<IList<Orphan>> GetOrphansAsync();
        Task<int> GetOrphansCountAsync();
        Task<int> GetActiveOrphansCountAsync();

        IList<Orphan> GetOrphansById(int id);
        //Task<IList<Orphan>> GetOrphansByIdAsync(int id);

        IList<OrphanFile> GetOrphanFiles(int orphanId);

        IList<OrphanFile> GetOrphanFiles();

        void AddOrphanFile(OrphanFile ofile);
        void RemoveOrphanFile(OrphanFile ofile);
        OrphanFile GetOrphanFileFromFileName(string fullName);
        OrphanFile GetOrphanFileById(int Id);

        IList<President> Search(
            string firstName, string lastName);


        Task<IList<OrphanFile>> GetOrphanFilesAsync();
        Task<IList<OrphanFile>> GetOrphanFilesAsync(int Id);//here

        int GetActiveOrphansCount();
        int GetActiveSponsoredOrphansCount();
        int GetActiveUnSponsoredOrphansCount();
        
        int GetUnApprovedOrphansCount();


        int ActiveOrphansOlderThan17Count();
        int ActiveOrphansOlderThan5Count();


        IList<OrphanActivity> GetOrphanActivitiesByOrphanId(int id);

        OrphanActivity GetOrphanActivitiesById(int id);
        void SaveOrphanActivity(OrphanActivity saveThis);
        void RemoveOrphanActivityById(int id);
        //IList<President> Search(string firstName,
        //    string lastName,
        //    string birthState,
        //    string deathState);
    }
}
