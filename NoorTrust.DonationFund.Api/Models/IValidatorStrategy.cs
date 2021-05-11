using System;
using System.Collections.Generic;
using System.Text;

namespace NoorTrust.DonationFund.Api.Models
{
    public interface IValidatorStrategy<T>
    {
        bool IsValid(T validateThis);
    }
}
