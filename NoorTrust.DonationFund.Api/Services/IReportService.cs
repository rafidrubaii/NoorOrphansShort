using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NoorTrust.DonationFund.Api.DataAccess;
using NoorTrust.DonationFund.Api.Models;

namespace NoorTrust.DonationFund.Api.Services
{
    public interface IReportService
    {
        void Save(Report saveThis);
       // void DeleteReportById(int id);
        Report GetReportById(int id);
        IList<Report> GetReports();

        Task<IList<Report>> GetReportsAsync();
        Task<int> GetReportsCountAsync();
        Task<int> GetActiveReportsCountAsync();

        IList<Report> GetReportsById(int id);
        //Task<IList<Report>> GetReportsByIdAsync(int id);

        IList<ReportFile> GetReportFiles(int id);

        IList<ReportFile> GetReportFiles();
        Dictionary<string, string> GetReportFileNameAndFolderById(int reportId);
        


        //IList<President> Search(string firstName,
        //    string lastName,
        //    string birthState,
        //    string deathState);
    }
}
