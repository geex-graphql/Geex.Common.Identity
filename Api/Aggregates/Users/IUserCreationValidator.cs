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
                    throw new BusinessException(GeexExceptionType.Conflict, message: "�û����Ѵ���, ��������, ����ϵ����Ա.");
                }
            }
            if (!user.Email.IsNullOrEmpty())
            {
                var emailConflict = DbContext.Queryable<User>().Any(o => o.Email == user.Email);
                if (emailConflict)
                {
                    throw new BusinessException(GeexExceptionType.Conflict, message: "ע��������Ѵ���, ��������, ����ϵ����Ա.");
                }
            }
            if (!user.PhoneNumber.IsNullOrEmpty())
            {
                var phoneConflict = DbContext.Queryable<User>().Any(o => o.PhoneNumber == user.PhoneNumber);
                if (phoneConflict)
                {
                    throw new BusinessException(GeexExceptionType.Conflict, message: "ע����ֻ����Ѵ���, ��������, ����ϵ����Ա.");
                }
            }
        }
    }
}