using System.Linq;
using System.Threading.Tasks;
using Geex.Common.Gql.Roots;
using Geex.Common.Identity.Api.Aggregates.Roles;
using Geex.Common.Identity.Api.GqlSchemas.Roles.Inputs;
using HotChocolate;
using MongoDB.Entities;

namespace Geex.Common.Identity.Api.GqlSchemas.Roles
{
    public class RoleQuery : QueryTypeExtension<RoleQuery>
    {
        public async Task<IQueryable<Role>> QueryRoles(
            [Service] DbContext dbContext,
            CreateRoleInput input)
        {
            return dbContext.Queryable<Role>();

        }
    }
}