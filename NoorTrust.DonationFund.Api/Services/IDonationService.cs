using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NoorTrust.DonationFund.Api.DataAccess;
using NoorTrust.DonationFund.Api.Models;

namespace NoorTrust.DonationFund.Api.Services
{
    public interface IDonationService
    {
        void Save(Donation saveThis);
        void DeleteDonationById(int id);

        Donation GetDonationById(int id);

        IList<Donation> GetDonations();

        Task<IList<Donation>> GetDonationsAsync();

        IList<Donation> GetDonationsBySponsorId(int Id);

       // Task<Donation> GetDonationByIdAsync(int id);



        //IList<President> Search(
        //    string firstName, string lastName);
        //IList<President> Search(string firstName,
        //    string lastName,
        //    string birthState,
        //    string deathState);
    }
}
