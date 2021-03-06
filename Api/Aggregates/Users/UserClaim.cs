using System.Collections.Generic;

namespace Geex.Common.Identity.Api.Aggregates.Users
{
    public class UserClaim
    {
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }

        public UserClaim(string claimType, string claimValue)
        {
            this.ClaimType = claimType;
            ClaimValue = claimValue;
        }

    }
}