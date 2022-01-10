using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Geex.Common.Identity.Core.Aggregates.Users;

using MongoDB.Bson;
using MongoDB.Entities;


// ReSharper disable once CheckNamespace
namespace Geex.Common.Identity.Api.Aggregates.Users
{
    public static class GeexCommonIdentityApiAggregatesUsers
    {
        public static IUser? MatchUserIdentifier(this IQueryable<IUser> users, string userIdentifier)
        {
            if (ObjectId.TryParse(userIdentifier, out _))
            {
                return users.FirstOrDefault(x => x.Id == userIdentifier);
            }
            return users.FirstOrDefault(x => x.PhoneNumber == userIdentifier || x.Username == userIdentifier || x.Email == userIdentifier);
        }

        public static IQueryable<UserBrief>? AsBrief(this IQueryable<IUser> users)
        {
            return users.Select(x => new UserBrief(x.Email, x.Id, x.OpenId,
                x.LoginProvider, x.PhoneNumber, x.Username, x.Nickname));
        }

        internal static Find<User, User> MatchUserIdentifier(this Find<User> @this, string userIdentifier)
        {
            if ((ObjectId.TryParse(userIdentifier, out _)))
            {
                return @this.Match(x => x.Id == userIdentifier);
            }
            return @this.Match(x => x.Email == userIdentifier || x.Username == userIdentifier || x.PhoneNumber == userIdentifier);

        }
    }
}
