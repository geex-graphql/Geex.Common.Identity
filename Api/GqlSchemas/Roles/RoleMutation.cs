using System.Threading.Tasks;
using Autofac;
using Geex.Common.Gql.Roots;
using Geex.Common.Identity.Api.Aggregates.Roles;
using Geex.Common.Identity.Api.GqlSchemas.Roles.Inputs;
using HotChocolate;
using MongoDB.Entities;

namespace Geex.Common.Identity.Api.GqlSchemas.Roles
{
    public class RoleMutation : MutationTypeExtension<RoleMutation>
    {
        public async Task<bool> CreateRole(
            [Service] IComponentContext componentContext,
            CreateRoleInput input)
        {
            var role = new Role(input.RoleName);
            await role.SaveAsync();
            return true;
        }
    }
}
