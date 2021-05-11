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
    public class ReportService : IReportService
    {
        private IRepository<Report> _RepositoryInstance;
        private IRepository<ReportFile> _RepositoryReportFileInstance;
      

        public ReportService(
            IRepository<Report> personRepositoryInstance,
            IRepository<ReportFile> RepositoryReportFileInstance
          
            )
        {
            if (personRepositoryInstance == null)
                throw new ArgumentNullException("personRepositoryInstance", "personRepositoryInstance is null.");
     
            _RepositoryInstance = personRepositoryInstance;
            _RepositoryReportFileInstance = RepositoryReportFileInstance;
        
        }

        public void Save(Report saveThis)
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


        public Report GetReportById(int id)
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

        public IList<Report> GetReports()
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

        public IList<Report> GetReportsById(int id)
        {
            var allPeople = _RepositoryInstance.GetAll().Where(x => x.Id == id);

            var peopleWhoWerePresident =
                (
                from temp in allPeople
                  
                select temp
                ).ToList();

            return peopleWhoWerePresident;// ToPresidents(peopleWhoWerePresident);
        }
        public async Task<IList<Report>> GetReportsAsync()
        {

            return await _RepositoryInstance.GetAllAsync();


        }
        public Dictionary<string, string> GetReportFileNameAndFolderById(int reportId)
        {
            var dic = new Dictionary<string, string>();

            var report = _RepositoryInstance.GetAll().Where(x => x.Id == reportId).FirstOrDefault();

            if (report != null)
            {
                foreach (var rep in report.ReportFiles)
                    dic.Add(rep.ReportFileName, report.FolderName);

            }
            return dic;


        }
        public async Task<int> GetReportsCountAsync()
        {

            return await _RepositoryInstance.CountAsync();

        }
        public async Task<int> GetActiveReportsCountAsync()
        {

            return await _RepositoryInstance.CountActiveAsync();

        }

        public IList<ReportFile> GetReportFiles(int id)
        {

           return _RepositoryReportFileInstance.GetAll().Where(x=>x.ReportId==id).ToList();

        }
        public IList<ReportFile> GetReportFiles()
        {

            return _RepositoryReportFileInstance.GetAll().ToList();

        }
      

        public IList<President> Search(string firstName, string lastName)
        {
            throw new NotImplementedException();
        }

        public int GetActiveReportsCount()
        {
            return 0;
        }

        public int GetActiveSponsoredReportsCount()
        {
            return 0;
        }

        public int GetActiveUnSponsoredReportsCount()
        {
            return 0;
        }

        public int ActiveReportsOlderThan17Count()
        {
            return 0;
        }

        public int GetUnApprovedReportsCount()
        {
            return 0;
        }

      
    }
}
