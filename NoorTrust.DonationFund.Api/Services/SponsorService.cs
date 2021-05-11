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
    public class SponsorService : ISponsorService
    {
        private IRepository<Sponsor> _RepositoryInstance;
        private IRepository<SponsorActivity> _RepositorySponsorActivityInstance;
     
        private IDaysInOfficeStrategy _DaysInOfficeStrategy;

        public SponsorService(
            IRepository<Sponsor> personRepositoryInstance,
           IRepository<SponsorActivity> repositorySponsorActivityInstance,
          
            IDaysInOfficeStrategy daysInOfficeStrategy)
        {
            if (personRepositoryInstance == null)
                throw new ArgumentNullException("personRepositoryInstance", "personRepositoryInstance is null.");
       
            _RepositoryInstance = personRepositoryInstance;
            _RepositorySponsorActivityInstance = repositorySponsorActivityInstance;
         
            _DaysInOfficeStrategy = daysInOfficeStrategy;
        }

        public void Save(Sponsor saveThis)
        {
            if (saveThis == null)
                throw new ArgumentNullException("saveThis", "saveThis is null.");


            try
            {

                _RepositoryInstance.Save(saveThis);
            }
            catch (Exception ex)
            {

                ErrorSignal.FromCurrentContext().Raise(ex);
            }



        }

        public void SaveSponsorActivity(SponsorActivity saveThis)
        {
            if (saveThis == null)
                throw new ArgumentNullException("saveThis", "saveThis is null.");


            try
            {

                _RepositorySponsorActivityInstance.Save(saveThis);
            }
            catch (Exception ex)
            {

                ErrorSignal.FromCurrentContext().Raise(ex);
            }



        }

        public void RemoveSponsorById(int id)
        {
            var match = _RepositoryInstance.GetById(id);

            if (match == null)
            {
                throw new InvalidOperationException(
                        $"Could not locate a person with an id of '{id}'."
                        );
            }
            else
            {
                _RepositoryInstance.Delete(match);
            }
        }


        public IList<Sponsor> GetSponsors()
        {
            var allPeople = _RepositoryInstance.GetAll();

            var peopleWhoWerePresident =
                (
                from temp in allPeople
          
                select temp
                ).ToList();

            return peopleWhoWerePresident;// ToPresidents(peopleWhoWerePresident);
        }
        public async Task<IList<Sponsor>> GetSponsorsAsync()
        {

            return await _RepositoryInstance.GetAllAsync();

        }
        public IList<SponsorActivity> GetSponsorActivitiesBySponsorId(int id)
        {
            var allPeople = _RepositorySponsorActivityInstance.GetAll().Where(x => x.SponsorId == id).ToList();


            return allPeople;
        }
        public SponsorActivity GetSponsorActivitiesById(int id)
        {
            var allPeople = _RepositorySponsorActivityInstance.GetById(id);//.Where(x => x.Id == id).FirstOrDefault();

            
            return allPeople;
        }
        public void RemoveSponsorActivitiesById(int id)
        {
            var match = _RepositorySponsorActivityInstance.GetById(id);

            if (match == null)
            {
                throw new InvalidOperationException(
                        $"Could not locate a SponsorActivity with an id of '{id}'."
                        );
            }
            else
            {
                _RepositorySponsorActivityInstance.Delete(match);
            }
        }
        private President ToPresident(Sponsor fromValue)
        {
            var toValue = new President();
         

            return toValue;
        }

        private IList<President> ToPresidents(IEnumerable<Sponsor> peopleWhoWerePresident)
        {
            var returnValues = new List<President>();

           

            foreach (var president in returnValues)
            {
                president.DaysInOffice =
                    _DaysInOfficeStrategy.GetDaysInOffice
                        (president.Terms);
            }

            return returnValues;
        }


        public IList<President> Search(string firstName, string lastName)
        {
            throw new NotImplementedException();
        }

        public int GetActiveSponsorsCount()
        {
            return _RepositoryInstance.CountActive();
        }

        public int GetActiveSponsoredCount()
        {

            return _RepositoryInstance.GetAll().Where(x => x.IsActive && x.Orphans.Count() > 0).Count();
        }

        public int GetActiveUnSponsoredCount()
        {
            return _RepositoryInstance.GetAll().Where(x => x.IsActive && x.Orphans.Count() == 0).Count();
        }

        public Sponsor GetSponsorById(int id)
        {
            throw new NotImplementedException();
        }

        public IList<Sponsor> GetAllwithOrphans()
        {
            throw new NotImplementedException();
        }

       
    }
}
