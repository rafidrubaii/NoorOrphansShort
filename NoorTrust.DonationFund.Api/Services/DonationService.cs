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
    public class DonationService : IDonationService
    {
        private IRepository<Donation> _RepositoryInstance;
    

       
        private IDaysInOfficeStrategy _DaysInOfficeStrategy;

        public DonationService(
            IRepository<Donation> personRepositoryInstance)
        {
            if (personRepositoryInstance == null)
                throw new ArgumentNullException("personRepositoryInstance", "personRepositoryInstance is null.");
       

            _RepositoryInstance = personRepositoryInstance;
          
           
        }

        public void Save(Donation saveThis)
        {
            if (saveThis == null)
                throw new ArgumentNullException("saveThis", "saveThis is null.");

            try
            {

                _RepositoryInstance.Save(saveThis);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
            }


        }

       
        public Donation GetDonationById(int id)
        {
            var match = _RepositoryInstance.GetById(id);

            if (match == null)
            {
                return null;
            }
            else
            {
                return match;
            }
        }
        public IList<Donation> GetDonationsBySponsorId(int Id)
        {
            var allPeople = _RepositoryInstance.GetAll().Where(x=>x.SponsorId==Id);

            var peopleWhoWerePresident =
                (
                from temp in allPeople
                    
                select temp
                ).ToList();

            return peopleWhoWerePresident;
        }
        public Donation GetDonationsById(int Id)
        {
            var donation = _RepositoryInstance.GetAll().Where(x => x.Id == Id).FirstOrDefault();

           

            return donation;
        }
        public IList<Donation> GetDonations()
        {
            var allPeople = _RepositoryInstance.GetAll();

            var peopleWhoWerePresident =
                (
                from temp in allPeople
           
                select temp
                ).ToList();

            return peopleWhoWerePresident;// ToPresidents(peopleWhoWerePresident);
        }

        public void DeleteDonationById(int id)
        {

            var match = _RepositoryInstance.GetById(id);

            if (match == null)
            {
                throw new InvalidOperationException(
                        $"Could not locate a Donation with an id of '{id}'."
                        );
            }
            else
            {
                _RepositoryInstance.Delete(match);
            }

        }
        public async Task<IList<Donation>> GetDonationsAsync()
        {

            return await _RepositoryInstance.GetAllAsync();


        }

       
    }
}
