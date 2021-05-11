using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NoorTrust.DonationFund.WebUi.Data
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        [Required]
        [MaxLength(30)]      
        public string FirstName { get; set; }

        [Required]
        [MaxLength(30)]
        public string LastName { get; set; }

        [MaxLength(50)]
        public string ThemeName { get; set; }
    }
}
