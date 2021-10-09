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
        string UserName { get; set; }
        public bool IsEnable { get; set; }
        string Email { get; set; }
        List<string> RoleNames { get; }
        IBlobObject AvatarFile { get; }
        string AvatarFileId { get; set; }
        UserClaim[] Claims { get; set; }
        public IQueryable<Role> Roles => DbContext.Queryable<Role>().Where(x => this.RoleNames.Contains(x.Id));
        IQueryable<Org> Orgs => DbContext.Queryable<Org>().Where(x => this.OrgCodes.Contains(x.Code));
        List<string> OrgCodes { get; set; }
        List<string> Permissions => DbContext.ServiceProvider.GetService<IMediator>().Send(new GetUserPermissionsRequest(this.Id)).Result.ToList();
    }
}
