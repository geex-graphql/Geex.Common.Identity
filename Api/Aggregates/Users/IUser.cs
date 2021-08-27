using System.Linq;

using Geex.Common.Identity.Api.Aggregates.Roles;
using Geex.Common.Identity.Core.Aggregates.Orgs;

using MongoDB.Entities;

namespace Geex.Common.Identity.Api.Aggregates.Users
{
    public interface IUser : IEntity
    {
        string PhoneNumber { get; set; }
        string UserName { get; set; }
        public bool IsEnable { get; set; }
        string Email { get; set; }
        string[] RoleNames { get; }
        string Avatar { get; }
        UserClaim[] Claims { get; set; }
        public IQueryable<Role> Roles => DbContext.Queryable<Role>().Where(x => this.RoleNames.Contains(x.Id));
        IQueryable<Org> Orgs => DbContext.Queryable<Org>().Where(x => this.OrgIds.Contains(x.Id));
        string[] OrgIds { get; set; }
    }
}
