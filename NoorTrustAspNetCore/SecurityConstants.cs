using System;
using System.Linq;

namespace NoorTrust.DonationFund.WebUi
{
    public static class SecurityConstants
    {
        public const string RoleName_Admin = "Admin";
        public const string RoleName_Staff = "Staff";
        public const string RoleName_Volunteer = "Volunteer";

        public const string PermissionName_View = "View";
        public const string PermissionName_Edit = "Edit";

        public const string Username_Admin = "developer@noortrust.org";
        public const string Username_User1 = "user1@test.org";
        public const string Username_User2 = "user2@test.org";
        public const string Username_Subscriber1 = "subscriber1@test.org";
        public const string Username_Subscriber2 = "subscriber2@test.org";
        
        public const string DefaultPassword = "password";

        public const string PolicyName_Edit = "EditPolicy";

        public const string SubscriptionType_Basic = "Basic";
        public const string SubscriptionType_Ultimate = "Ultimate";

        public const string Claim_SubscriptionType = "SubscriptionType";
    }
}
