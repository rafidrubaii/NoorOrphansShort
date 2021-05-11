using System;
using System.Collections.Generic;
using System.Linq;
using NoorTrust.DonationFund.Api.Models;

namespace NoorTrust.DonationFund.Api.Services
{
    public interface IPresidentService
    {
        void Save(President saveThis);
        void DeletePresidentById(int id);
        President GetPresidentById(int id);
        IList<President> GetPresidents();
        IList<President> Search(
            string firstName, string lastName);
        IList<President> Search(string firstName,
            string lastName,
            string birthState,
            string deathState);
    }
}
