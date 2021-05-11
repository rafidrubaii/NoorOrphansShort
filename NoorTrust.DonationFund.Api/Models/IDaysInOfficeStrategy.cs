using System.Collections.Generic;

namespace NoorTrust.DonationFund.Api.Models
{
    public interface IDaysInOfficeStrategy
    {
         int GetDaysInOffice(IEnumerable<Term> terms);
    }
}