using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace NoorTrust.DonationFund.WebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SecuritySummaryController : Controller
    {
        public IActionResult Index()
        {
            var principal = User as ClaimsPrincipal;

            var identity = User.Identity;

            var claimsIdentityInstance = identity as ClaimsIdentity;

            if (claimsIdentityInstance == null)
            {
                return View(new List<Claim>());
            }
            else
            {
                return View(claimsIdentityInstance.Claims.ToList());
            }
        }
    }
}