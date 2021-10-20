using System;
using System.Linq;

using Geex.Common.Abstractions;
using Geex.Common.Identity.Core.Aggregates.Users;

using MongoDB.Entities;

namespace Geex.Common.Identity.Api.Aggregates.Users
{
    public interface IUserCreationValidator
    {
        public DbContext DbContext { get; }
        public void Check(User user)
        {
            if (DbContext.Queryable<User>().WhereIf(!user.Email.IsNullOrEmpty(), o => o.Email == user.Email).WhereIf(!user.PhoneNumber.IsNullOrEmpty(), o => o.PhoneNumber == user.PhoneNumber).Select(x => x.Id).Any())
                throw new BusinessException(GeexExceptionType.Conflict);
        }
    }
}