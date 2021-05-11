using System.Threading.Tasks;

namespace NoorTrust.DonationFund.WebUI.Controllers
{
    public interface ITestDataUtility
    {
        Task CreateSponsorTestData();
        Task VerifyDatabaseIsPopulated();
    }

}
