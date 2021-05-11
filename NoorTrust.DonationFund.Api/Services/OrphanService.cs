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
    public class OrphanService : IOrphanService
    {
        private IRepository<Orphan> _RepositoryInstance;
        private IRepository<OrphanFile> _RepositoryOrphanFileInstance;
        private IRepository<OrphanActivity> _RepositoryOrphanActivityInstance;
      

        public OrphanService(
            IRepository<Orphan> personRepositoryInstance
          , IRepository<OrphanFile> RepositoryOrphanFileInstance,
            IRepository<OrphanActivity> RepositoryOrphanActivityInstance
            
            )
        {
            if (personRepositoryInstance == null)
                throw new ArgumentNullException("personRepositoryInstance", "personRepositoryInstance is null.");
           

            _RepositoryInstance = personRepositoryInstance;
            _RepositoryOrphanFileInstance = RepositoryOrphanFileInstance;
            _RepositoryOrphanActivityInstance = RepositoryOrphanActivityInstance;
         
        }

        public void Save(Orphan saveThis)
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

        public void SaveOrphanActivity(OrphanActivity saveThis)
        {
            if (saveThis == null)
                throw new ArgumentNullException("saveThis", "saveThis is null.");


            try
            {

                _RepositoryOrphanActivityInstance.Save(saveThis);
            }
            catch (Exception ex)
            {

                ErrorSignal.FromCurrentContext().Raise(ex);
            }



        }
        public void RemoveOrphanActivityById(int id)
        {
            var match = _RepositoryOrphanActivityInstance.GetById(id);

            if (match == null)
            {
                throw new InvalidOperationException(
                        $"Could not locate an OrphanActivity with an id of '{id}'."
                        );
            }
            else
            {
                _RepositoryOrphanActivityInstance.Delete(match);
            }
        }
        public IList<OrphanFile> GetOrphanFiles(int orphanId)
        {

            var orphanFiles = _RepositoryOrphanFileInstance.GetAll();

            return orphanFiles.Where(x => x.OrphanId == orphanId).ToList();//.FirstOrDefault();//.Select(x => x.OrphanFiles).Where(x=>x.Count() >0 && x.or).;

           

        }
        public IList<OrphanFile> GetOrphanFiles()
        {
            var orphanFiles = _RepositoryOrphanFileInstance.GetAll();
            return orphanFiles;
        }
      

        public Orphan GetOrphanById(int id)
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

        public IList<Orphan> GetOrphans()
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

        public IList<Orphan> GetOrphansById(int id)
        {
            var allPeople = _RepositoryInstance.GetAll().Where(x => x.SponsorId == id);

            var peopleWhoWerePresident =
                (
                from temp in allPeople
                    //  where temp.Facts.GetFact(PresidentsConstants.President) != null
                select temp
                ).ToList();

            return peopleWhoWerePresident;// ToPresidents(peopleWhoWerePresident);
        }
        public IList<OrphanActivity> GetOrphanActivitiesByOrphanId(int id)
        {
            var allPeople = _RepositoryOrphanActivityInstance.GetAll().Where(x => x.OrphanId == id).ToList();


            return allPeople;
        }
        public OrphanActivity GetOrphanActivitiesById(int id)
        {

            var one = _RepositoryOrphanActivityInstance.GetById(id);//.Where(x => x.Id == id).FirstOrDefault();

            return one;

        }


        public async Task<IList<Orphan>> GetOrphansAsync()
        {

            return await _RepositoryInstance.GetAllAsync();


        }


        public async Task<int> GetOrphansCountAsync()
        {

            return await _RepositoryInstance.CountAsync();

        }
        public async Task<int> GetActiveOrphansCountAsync()
        {

            return await _RepositoryInstance.CountActiveAsync();

        }
      
        public void AddOrphanFile(OrphanFile ofile)
        {

            _RepositoryOrphanFileInstance.Save(ofile);

            var orphan = _RepositoryInstance.GetById(ofile.OrphanId.Value);
            orphan.OrphanFileId = ofile.Id;

            _RepositoryInstance.Save(orphan);

        }
        public void RemoveOrphanFile(OrphanFile ofile)
        {

            _RepositoryOrphanFileInstance.Delete(ofile);

            var orphan = _RepositoryInstance.GetById(ofile.OrphanId.Value);
            orphan.OrphanFileId = null;

            _RepositoryInstance.Save(orphan);

        }
        public OrphanFile GetOrphanFileFromFileName(string fullName)
        {

            return _RepositoryOrphanFileInstance.GetAll().Where(x => x.FileName.ToString() == fullName).FirstOrDefault();


        }
        public OrphanFile GetOrphanFileById(int Id)
        {

            return _RepositoryOrphanFileInstance.GetById(Id);

        }
        public async Task<IList<OrphanFile>> GetOrphanFilesAsync()
        {

            return await _RepositoryOrphanFileInstance.GetAllAsync();
        }
       

        public IList<President> Search(string firstName, string lastName)
        {
            throw new NotImplementedException();
        }

        public int GetActiveOrphansCount()
        {
            return _RepositoryInstance.CountActive();
        }

        public int GetActiveSponsoredOrphansCount()
        {
            return _RepositoryInstance.GetAll().Where(x => x.IsActive == true && x.SponsorId != null).Count();
        }

        public int GetActiveUnSponsoredOrphansCount()
        {
            return _RepositoryInstance.GetAll().Where(x => x.IsActive == true && x.SponsorId == null).Count();
        }

        //--------------------------------------------------------------------
        public int ActiveOrphansOlderThan17Count()
        {
            DateTime today = DateTime.Today;
            DateTime min = today.AddYears(-(100 + 1));
            DateTime max = today.AddYears(-17);


            return _RepositoryInstance.GetAll().Where(x => x.IsActive == true && x.DOB != null && x.Age > 16).Count();
        }
        public int ActiveOrphansOlderThan5Count()
        {
            DateTime today = DateTime.Today;
            DateTime min = today.AddYears(5);
            DateTime max = today.AddYears(6);


            return _RepositoryInstance.GetAll().Where(x => x.IsActive == true && x.DOB != null && x.Age >= 5 && x.Age < 6).Count();
        }
        //----------------------------------------------------------------------
        public int GetUnApprovedOrphansCount()
        {
            return 0;
        }

        public Task<IList<OrphanFile>> GetOrphanFilesAsync(int Id)
        {
            throw new NotImplementedException();
        }

    }
}
