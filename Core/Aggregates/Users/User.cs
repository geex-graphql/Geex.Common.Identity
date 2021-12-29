using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Geex.Common.Abstraction;
using Geex.Common.Abstractions;
using Geex.Common.Abstractions.Enumerations;
using Geex.Common.BlobStorage.Api.Aggregates.BlobObjects;
using Geex.Common.BlobStorage.Core.Aggregates.BlobObjects;
using Geex.Common.Identity.Api.Aggregates.Orgs.Events;
using Geex.Common.Identity.Api.Aggregates.Roles;
using Geex.Common.Identity.Api.Aggregates.Users;
using Geex.Common.Identity.Api.Aggregates.Users.Events;
using Geex.Common.Identity.Core.Aggregates.Orgs;

using MediatR;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

using MongoDB.Entities;

using NetCasbin.Abstractions;
using Entity = Geex.Common.Abstraction.Storage.Entity;

namespace Geex.Common.Identity.Core.Aggregates.Users
{
    public partial class User : Abstraction.Storage.Entity, IUser
    {
        public string? PhoneNumber { get; set; }
        public bool IsEnable { get; set; } = true;
        public string Username { get; set; }
        public string? Email { get; set; }
        public string Password { get; set; }
        public UserClaim[] Claims { get; set; } = Enumerable.Empty<UserClaim>().ToArray();
        public IQueryable<Org> Orgs => DbContext.Queryable<Org>().Where(x => this.OrgCodes.Contains(x.Code));
        public List<string> OrgCodes { get; set; } = Enumerable.Empty<string>().ToList();
        public List<string> Permissions => DbContext.ServiceProvider.GetService<IMediator>().Send(new GetSubjectPermissionsRequest(this.Id)).Result.ToList();
        public void ChangePassword(string originPassword, string newPassword)
        {
            if (!this.CheckPassword(originPassword))
            {
                throw new BusinessException(GeexExceptionType.OnPurpose, message: "ԭ����У��ʧ��.");
            }
            this.SetPassword(newPassword);
        }

        public List<string> RoleNames => DbContext.ServiceProvider.GetService<IEnforcer>().GetRolesForUser(this.Id);
        public IBlobObject AvatarFile => DbContext.Queryable<BlobObject>().OneAsync(this.AvatarFileId).Result;
        public string AvatarFileId { get; set; }

        public IQueryable<Role> Roles => DbContext.Queryable<Role>().Where(x => this.RoleNames.Contains(x.Name));
        protected User()
        {
        }

        public static User New(IUserCreationValidator userCreationValidator, IPasswordHasher<IUser> passwordHasher, string username, string phoneNumber, string email, string password)
        {
            if (!email.IsValidEmail())
                throw new Exception("invalid input for email");
            else if (!phoneNumber.IsValidPhoneNumber())
                throw new Exception("invalid input for phoneNumber");
            //数字\字母\下划线
            if (!new Regex(@"\A[\w\d_]+\z").IsMatch(username))
                throw new Exception("invalid input for username");
            var result = new User()
            {
                Username = username,
                Email = email,
                PhoneNumber = phoneNumber,
                LoginProvider = LoginProviderEnum.Local
            };
            userCreationValidator.Check(result);
            result.Password = passwordHasher.HashPassword(result, password);
            return result;
        }

        public bool CheckPassword(string password)
        {
            var passwordHasher = this.ServiceProvider.GetService<IPasswordHasher<IUser>>();
            return passwordHasher!.VerifyHashedPassword(this, Password, password) != PasswordVerificationResult.Failed;
        }
        public async Task AssignRoles(List<Role> roles)
        {
            this.AddDomainEvent(new UserRoleChangedEvent(this.Id, roles.Select(x => x.Name).ToList()));
        }

        public async Task AssignOrgs(List<Org> orgs)
        {
            this.OrgCodes = orgs.Select(x => x.Code).ToList();
            this.AddDomainEvent(new UserOrgChangedEvent(this.Id, orgs.Select(x => x.Code).ToList()));
        }

        public async Task AssignRoles(List<string> roles)
        {
            this.AddDomainEvent(new UserRoleChangedEvent(this.Id, roles.ToList()));
        }

        public async Task AssignOrgs(List<string> orgs)
        {
            this.OrgCodes = orgs.ToList();
            this.AddDomainEvent(new UserOrgChangedEvent(this.Id, orgs.ToList()));
        }

        public User SetPassword(string? password)
        {
            Password = ServiceProvider.GetService<IPasswordHasher<IUser>>().HashPassword(this, password);
            return this;
        }

        public Task AssignRoles(params string[] roles)
        {
            return this.AssignRoles(roles.ToList());
        }

        public async Task AddOrg(Org entity)
        {
            var orgCodes = this.OrgCodes.ToList();
            orgCodes.Add(entity.Code);
            await this.AssignOrgs(orgCodes);
        }

        public static User NewExternal(IUserCreationValidator userCreationValidator, IPasswordHasher<IUser> passwordHasher, string openId, LoginProviderEnum loginProvider, string username, string? phoneNumber = default, string? email = default, string? password = default)
        {
            //if (!email.IsValidEmail())
            //    throw new Exception("invalid input for email");
            //else if (!phoneNumber.IsValidPhoneNumber())
            //    throw new Exception("invalid input for phoneNumber");
            //数字\字母\下划线
            if (!new Regex(@"\A[\w\d_]+\z").IsMatch(username))
                throw new Exception("invalid input for username");
            var result = new User()
            {
                Username = username,
                OpenId = openId,
                LoginProvider = loginProvider,
                PhoneNumber = phoneNumber,
                Email = email,
            };
            userCreationValidator.Check(result);
            if (!password.IsNullOrEmpty())
            {
                result.Password = passwordHasher.HashPassword(result, password);
            }
            return result;
        }

        public LoginProviderEnum LoginProvider { get; set; }

        public string? OpenId { get; set; }
    }
}