using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

using Geex.Common.Abstractions.Enumerations;
using Geex.Common.BlobStorage.Api.Aggregates.BlobObjects;
using Geex.Common.BlobStorage.Core.Aggregates.BlobObjects;
using Geex.Common.Identity.Api.Aggregates.Orgs.Events;
using Geex.Common.Identity.Api.Aggregates.Roles;
using Geex.Common.Identity.Api.Aggregates.Users;
using Geex.Common.Identity.Api.Aggregates.Users.Events;
using Geex.Common.Identity.Core.Aggregates.Orgs;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

using MongoDB.Entities;

using Entity = Geex.Common.Abstraction.Storage.Entity;

namespace Geex.Common.Identity.Core.Aggregates.Users
{
    public partial class User : Abstraction.Storage.Entity, IUser
    {
        public string? PhoneNumber { get; set; }
        public bool IsEnable { get; set; }
        public string UserName { get; set; }
        public string? Email { get; set; }
        public string Password { get; set; }
        public UserClaim[] Claims { get; set; } = Enumerable.Empty<UserClaim>().ToArray();
        public List<string> OrgCodes { get; set; } = Enumerable.Empty<string>().ToList();
        public List<string> RoleNames { get; set; } = Enumerable.Empty<string>().ToList();
        public IBlobObject AvatarFile => DbContext.Find<BlobObject>().OneAsync(this.AvatarFileId).Result;
        public string AvatarFileId { get; set; }

        public IQueryable<Role> Roles => DbContext.Queryable<Role>().Where(x => this.RoleNames.Contains(x.Name));
        protected User()
        {
        }
        public User(IUserCreationValidator userCreationValidator, IPasswordHasher<IUser> passwordHasher, string phoneOrEmail, string password)
        : this()
        {
            if (phoneOrEmail.IsValidEmail())
                Email = phoneOrEmail;
            else if (phoneOrEmail.IsValidPhoneNumber())
                PhoneNumber = phoneOrEmail;
            else
                throw new Exception("invalid input for phoneOrEmail");
            this.UserName = phoneOrEmail;
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
            this.RoleNames = roles.Select(x => x.Name).ToList();
            this.AddDomainEvent(new UserRoleChangedEvent(this.Id, roles.Select(x => x.Name).ToList()));
        }

        public async Task AssignOrgs(List<Org> orgs)
        {
            this.OrgCodes = orgs.Select(x => x.Code).ToList();
            this.AddDomainEvent(new UserOrgChangedEvent(this.Id, orgs.Select(x => x.Code).ToList()));
        }

        public User SetPassword(string? password)
        {
            Password = ServiceProvider.GetService<IPasswordHasher<IUser>>().HashPassword(this, password);
            return this;
        }
    }
}