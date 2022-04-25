using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Geex.Common.Abstraction;
using Geex.Common.Abstraction.MultiTenant;
using Geex.Common.BlobStorage.Api.Aggregates.BlobObjects;
using Geex.Common.Identity.Api.Aggregates.Roles;
using Geex.Common.Identity.Core.Aggregates.Orgs;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

using MongoDB.Entities;

namespace Geex.Common.Identity.Api.Aggregates.Users
{
    public interface IUser : IEntity, ITenantFilteredEntity
    {
        public const string SuperAdminName = "superAdmin";
        public const string SuperAdminId = "000000000000000000000001";
        string? PhoneNumber { get; set; }
        string Username { get; set; }
        string? Nickname { get; set; }
        string? Email { get; set; }
        LoginProviderEnum LoginProvider { get; set; }
        string? OpenId { get; set; }
        public bool IsEnable { get; set; }
        List<string> RoleNames { get; }
        Lazy<IBlobObject?> AvatarFile { get; }
        string? AvatarFileId { get; set; }
        List<UserClaim> Claims { get; set; }
        public IQueryable<Role> Roles { get; }
        IQueryable<Org> Orgs { get; }
        List<string> OrgCodes { get; set; }
        List<string> Permissions { get; }

        Task AddOrg(Org entity);
        Task AssignOrgs(List<string> orgs);
        Task AssignRoles(List<string> roles);
        void ChangePassword(string originPassword, string newPassword);
    }
}
