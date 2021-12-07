using System.Collections.Generic;
using System.Linq;

using Geex.Common.Abstraction;
using Geex.Common.BlobStorage.Api.Aggregates.BlobObjects;
using Geex.Common.Identity.Api.Aggregates.Roles;
using Geex.Common.Identity.Core.Aggregates.Orgs;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

using MongoDB.Entities;

namespace Geex.Common.Identity.Api.Aggregates.Users
{
    public interface IUser : IEntity
    {
        string PhoneNumber { get; set; }
        string Username { get; set; }
        public bool IsEnable { get; set; }
        string Email { get; set; }
        List<string> RoleNames { get; }
        IBlobObject AvatarFile { get; }
        string AvatarFileId { get; set; }
        UserClaim[] Claims { get; set; }
        public IQueryable<Role> Roles { get; }
        IQueryable<Org> Orgs { get; }
        List<string> OrgCodes { get; set; }
        List<string> Permissions { get; }
        void ChangePassword(string originPassword, string newPassword);
    }
}
