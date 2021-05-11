using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoorTrust.DonationFund.Api.Models;
using NoorTrust.DonationFund.Common;
using NoorTrust.DonationFund.Api.DataAccess;
using NoorTrust.DataAccess;

namespace NoorTrust.DonationFund.Api.Services
{
    public class CountryService : ICountryService
    {
        private IRepository<Country> _RepositoryInstance;
      
        public CountryService(
            IRepository<Country> personRepositoryInstance)
         
        {
            if (personRepositoryInstance == null)
                throw new ArgumentNullException("personRepositoryInstance", "personRepositoryInstance is null.");
            //if (validator == null)
            //    throw new ArgumentNullException("validator", "Argument cannot be null.");            //if (validator == null)
            //    throw new ArgumentNullException("validator", "Argument cannot be null.");

            _RepositoryInstance = personRepositoryInstance;
          
        }

        public void Save(Country saveThis)
        {
            if (saveThis == null)
                throw new ArgumentNullException("saveThis", "saveThis is null.");

            //if (_ValidatorInstance.IsValid(saveThis) == false)
            //{
            //    throw new InvalidOperationException("President is invalid.");
            //}

            try
            {

                _RepositoryInstance.Save(saveThis);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
            }




        }

      

        public IList<Country> GetCountries()
        {
            var allPeople = _RepositoryInstance.GetAll();

            var peopleWhoWerePresident =
                (
                from temp in allPeople
            //  where temp.Facts.GetFact(PresidentsConstants.President) != null
                select temp
                ).ToList();

            return peopleWhoWerePresident;// ToPresidents(peopleWhoWerePresident);
        }
        public async Task<IList<Country>> GetCountriesAsync()
        {

            return await _RepositoryInstance.GetAllAsync();


            //var peopleWhoWerePresident =
            //    (
            //    from temp in allPeople.Result
            //        //  where temp.Facts.GetFact(PresidentsConstants.President) != null
            //    select temp
            //    ).ToList();

            //return await allPeople;// ToPresidents(peopleWhoWerePresident);
        }
      

        public Country GetCountryById(int id)
        {
            return _RepositoryInstance.GetById(id);
        }

        public void DeleteCountryById(int id)
        {
            _RepositoryInstance.Delete(_RepositoryInstance.GetById(id));
        }
    }
}
