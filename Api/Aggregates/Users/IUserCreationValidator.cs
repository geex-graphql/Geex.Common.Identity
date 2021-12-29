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
            if (!user.Username.IsNullOrEmpty())
            {
                var emailConflict = DbContext.Queryable<User>().Any(o => o.Username == user.Username);
                if (emailConflict)
                {
                    throw new BusinessException(GeexExceptionType.Conflict, message: "用户名已存在, 如有疑问, 请联系管理员.");
                }
            }
            if (!user.Email.IsNullOrEmpty())
            {
                var emailConflict = DbContext.Queryable<User>().Any(o => o.Email == user.Email);
                if (emailConflict)
                {
                    throw new BusinessException(GeexExceptionType.Conflict, message: "注册的邮箱已存在, 如有疑问, 请联系管理员.");
                }
            }
            if (!user.PhoneNumber.IsNullOrEmpty())
            {
                var phoneConflict = DbContext.Queryable<User>().Any(o => o.PhoneNumber == user.PhoneNumber);
                if (phoneConflict)
                {
                    throw new BusinessException(GeexExceptionType.Conflict, message: "注册的手机号已存在, 如有疑问, 请联系管理员.");
                }
            }
        }
    }
}