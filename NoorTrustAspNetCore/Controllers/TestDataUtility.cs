
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using NoorTrust.DonationFund.Api.DataAccess;
using NoorTrust.DonationFund.Api.Services;
using NoorTrust.DonationFund.WebUi;

namespace NoorTrust.DonationFund.WebUI.Controllers
{
    public class TestDataUtility : ITestDataUtility
    {
        private ISponsorService _Service;
        private NoorTrustDbContext _DbContext;
        private UserManager<IdentityUser> _UserManager;
        private RoleManager<IdentityRole> _RoleManager;

        public TestDataUtility(ISponsorService service, NoorTrustDbContext dbContext,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            if (service == null)
                throw new ArgumentNullException("service", "service is null.");

            _Service = service;

            if (dbContext == null)
            {
                throw new ArgumentNullException("dbContext", "Argument cannot be null.");
            }

            _DbContext = dbContext;

            _UserManager = userManager;
            _RoleManager = roleManager;
        }

        public async Task CreateSponsorTestData()
        {
            //var xml = TestDataResource.UsSponsorsXml;

            //List<Sponsor> allSponsors = PopulateSponsorsFromXml(xml);

            //DeleteAll();

            //allSponsors.ForEach(x => _Service.Save(x));

            await InitializeSecurity();
        }

        public async Task VerifyDatabaseIsPopulated()
        {
            _DbContext.Database.EnsureCreated();

            var presidents = _Service.GetSponsors();

            if (presidents == null || presidents.Count() == 0)
            {
                await CreateSponsorTestData();
            }
        }

        private List<Sponsor> PopulateSponsorsFromXml(string xml)
        {
            var returnValue = new List<Sponsor>();

            //var root = XElement.Parse(xml);

            //var presidents = root.ElementsByLocalName("president");

            //Sponsor groverCleveland = null;

            //foreach (var fromElement in presidents)
            //{
            //    var currentSponsor = GetSponsorFromXml(fromElement);

            //    if (currentSponsor.LastName == "Cleveland")
            //    {
            //        // grover cleveland had two non-consecutive terms
            //        // only create one record for grover 
            //        // with two terms
            //        if (groverCleveland == null)
            //        {
            //            groverCleveland = currentSponsor;
            //            returnValue.Add(currentSponsor);
            //        }
            //        else
            //        {
            //            groverCleveland.Terms.Add(currentSponsor.Terms[0]);
            //        }
            //    }
            //    else
            //    {
            //        returnValue.Add(currentSponsor);
            //    }
            //}

            return returnValue;
        }

        private Sponsor GetSponsorFromXml(XElement fromValue)
        {
            Sponsor toValue = new Sponsor();

            //toValue.BirthCity = fromValue.AttributeValue("birthcity");
            //toValue.BirthState = fromValue.AttributeValue("birthstate");
            //toValue.BirthDate = SafeToDateTime(fromValue.AttributeValue("birthdate"));

            //toValue.DeathCity = fromValue.AttributeValue("deathcity");
            //toValue.DeathState = fromValue.AttributeValue("deathstate");
            //toValue.DeathDate = SafeToDateTime(fromValue.AttributeValue("deathdate"));

            //toValue.FirstName = fromValue.AttributeValue("firstname");
            //toValue.LastName = fromValue.AttributeValue("lastname");

            //toValue.ImageFilename = fromValue.AttributeValue("image-filename");

            //toValue.AddTerm(
            //    "Sponsor",
            //    SafeToDateTime(fromValue.AttributeValue("start")),
            //    SafeToDateTime(fromValue.AttributeValue("end")),
            //    SafeToInt32(fromValue.AttributeValue("id"))
            //    );

            return toValue;
        }

        private DateTime SafeToDateTime(string fromValue)
        {
            DateTime temp;

            if (DateTime.TryParse(fromValue, out temp) == true)
            {
                return temp;
            }
            else
            {
                return default(DateTime);
            }
        }

        private int SafeToInt32(string fromValue)
        {
            int temp;

            if (Int32.TryParse(fromValue, out temp) == true)
            {
                return temp;
            }
            else
            {
                return default(int);
            }
        }

        private void DeleteAll()
        {
            var allSponsors = _Service.GetSponsors();

            foreach (var item in allSponsors)
            {
                // _Service.DeleteSponsorById(item.Id);
            }
        }
        private async Task InitializeSecurity()
        {
            //  await DeleteAllRoles();
            //  await DeleteAllUsers();

            // create the roles
            await _RoleManager.CreateAsync(new IdentityRole(SecurityConstants.RoleName_Admin));
            await _RoleManager.CreateAsync(new IdentityRole(SecurityConstants.RoleName_Staff));
            await _RoleManager.CreateAsync(new IdentityRole(SecurityConstants.RoleName_Volunteer));


            // create users
            var admin = await CreateUser(SecurityConstants.Username_Admin);

            var aamal = await CreateUser("aamal@noororphansfund.org");
            var haider = await CreateUser("studio@gwdesigner.com");
            var rozmin = await CreateUser("info@noororphansfund.org");
            var betool = await CreateUser("betool@noortrust.org");

            //var user1 = await CreateUser(SecurityConstants.Username_User1);
            //var user2 = await CreateUser(SecurityConstants.Username_User2);
            //var user3 = await CreateUser(SecurityConstants.Username_Subscriber1);
            //var user4 = await CreateUser(SecurityConstants.Username_Subscriber2);

            await _UserManager.AddToRoleAsync(admin, SecurityConstants.RoleName_Admin);
            await _UserManager.AddToRoleAsync(aamal, SecurityConstants.RoleName_Staff);
            await _UserManager.AddToRoleAsync(haider, SecurityConstants.RoleName_Admin);
            await _UserManager.AddToRoleAsync(rozmin, SecurityConstants.RoleName_Staff);
            await _UserManager.AddToRoleAsync(betool, SecurityConstants.RoleName_Staff);
        }


        private async Task<IdentityUser> CreateUser(string username)
        {
            var user = new IdentityUser();

            user.UserName = username;
            user.Email = username;
            user.EmailConfirmed = true;

            var result = await _UserManager.CreateAsync(user, SecurityConstants.DefaultPassword);

            if (result.Succeeded == false)
            {
                throw new InvalidOperationException("Error while creating user." + Environment.NewLine + result.Errors.ToString());
            }
            else
            {
                Console.WriteLine();
            }

            return user;
        }

        private async Task DeleteAllUsers()
        {
            var users = _UserManager.Users.ToList();

            foreach (var deleteThisUser in users)
            {
                await _UserManager.DeleteAsync(deleteThisUser);
            }
        }

        private async Task DeleteAllRoles()
        {
            var roles = _RoleManager.Roles.ToList();

            foreach (var deleteThis in roles)
            {
                await _RoleManager.DeleteAsync(deleteThis);
            }
        }
    }

}    


