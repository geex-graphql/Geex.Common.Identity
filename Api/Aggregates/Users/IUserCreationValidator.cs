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
            if (DbContext.Find<User>().Match(o => o.Email == user.Email || o.PhoneNumber == user.PhoneNumber).Project(x => x.Include(y => y.Id)).ExecuteAsync().Result.Any())
                throw new BusinessException(GeexExceptionType.Conflict);
        }
    }
}