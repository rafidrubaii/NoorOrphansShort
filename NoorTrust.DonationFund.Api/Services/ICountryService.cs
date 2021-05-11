using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NoorTrust.DonationFund.Api.DataAccess;
using NoorTrust.DonationFund.Api.Models;

namespace NoorTrust.DonationFund.Api.Services
{
    public interface ICountryService
    {
        void Save(Country saveThis);

        void DeleteCountryById(int id);

        Country GetCountryById(int id);

        IList<Country> GetCountries();      

        Task<IList<Country>> GetCountriesAsync();
       

        
    }
}
