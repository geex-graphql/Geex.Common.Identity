using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Geex.Common.Abstractions.Enumerations;
using Geex.Common.Authorization.Abstraction;
using Geex.Common.Identity.Api.Aggregates.Roles;
using Geex.Common.Identity.Api.Aggregates.Users;
using Geex.Common.Identity.Api.Aggregates.Users.Events;
using Geex.Common.Identity.Core.Aggregates.Orgs;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

using MongoDB.Entities;

using Entity = Geex.Common.Abstractions.Entity;

namespace Geex.Common.Identity.Core.Aggregates.Users
{
    public partial class User : Entity, IUser
    {
        public string? PhoneNumber { get; set; }
        public bool IsEnable { get; set; }
        public string UserName { get; set; }
        public string? Email { get; set; }
        public string Password { get; set; }
        public UserClaim[] Claims { get; set; } = Enumerable.Empty<UserClaim>().ToArray();
        public string[] OrgIds { get; set; } = Enumerable.Empty<string>().ToArray();
        public string[] RoleNames { get; set; } = Enumerable.Empty<string>().ToArray();
        public string Avatar => this.Claims.FirstOrDefault(x => x.ClaimType == GeexClaimType.Avatar)?.ClaimValue;
        public IQueryable<Role> Roles => DbContext.Queryable<Role>().Where(x => this.RoleIds.Contains(x.Id));
        public string[] RoleIds { get; set; } = Enumerable.Empty<string>().ToArray();
        public List<AppPermission> AuthorizedPermissions { get; set; }
        protected User()
        {
        }
        public User(IUserCreationValidator userCreationValidator, IPasswordHasher<IUser> passwordHasher, string phoneOrEmail, string password, string username)
        : this()
        {
            if (phoneOrEmail.IsValidEmail())
                Email = phoneOrEmail;
            else if (phoneOrEmail.IsValidPhoneNumber())
                PhoneNumber = phoneOrEmail;
            else
                throw new Exception("invalid input for phoneOrEmail");
            this.UserName = username;
            userCreationValidator.Check(this);
            Password = passwordHasher.HashPassword(this, password);
        }
        public bool CheckPassword(string password)
        {
            var passwordHasher = this.ServiceProvider.GetService<IPasswordHasher<IUser>>();
            return passwordHasher!.VerifyHashedPassword(this, Password, password) == PasswordVerificationResult.Success;
        }
        public async Task AssignRoles(List<Role> roles)
        {
            this.RoleIds.RemoveAll(this.Roles.Select(x => x.Id));
            await roles.SaveAsync((this as IEntity).DbContext?.Session);
            this.AddDomainEvent(new UserRoleChangedEvent(this.Id, roles.Select(x => x.Id).ToList()));
        }
    }
}