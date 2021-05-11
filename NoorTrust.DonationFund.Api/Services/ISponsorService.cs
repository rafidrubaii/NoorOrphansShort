using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NoorTrust.DonationFund.Api.DataAccess;
using NoorTrust.DonationFund.Api.Models;

namespace NoorTrust.DonationFund.Api.Services
{
    public interface ISponsorService
    {
        void Save(Sponsor saveThis);
       // void DeleteSponsorById(int id);
        Sponsor GetSponsorById(int id);
        IList<Sponsor> GetSponsors();

        IList<Sponsor> GetAllwithOrphans();

        Task<IList<Sponsor>> GetSponsorsAsync();
        IList<President> Search(
            string firstName, string lastName);


        int GetActiveSponsorsCount();
        int GetActiveSponsoredCount();
        int GetActiveUnSponsoredCount();



        IList<SponsorActivity> GetSponsorActivitiesBySponsorId(int id);
       SponsorActivity GetSponsorActivitiesById(int id);

        void SaveSponsorActivity(SponsorActivity saveThis);

        void RemoveSponsorActivitiesById(int id);
        //IList<President> Search(string firstName,
        //    string lastName,
        //    string birthState,
        //    string deathState);
    }
}
