using System.Threading.Tasks;

using Autofac;

using Geex.Common.Gql.Roots;
using Geex.Common.Identity.Api.Aggregates.Roles;
using Geex.Common.Identity.Api.GqlSchemas.Roles.Inputs;

using HotChocolate;

using MediatR;

using MongoDB.Entities;

namespace Geex.Common.Identity.Api.GqlSchemas.Roles
{
    public class RoleMutation : MutationTypeExtension<RoleMutation>
    {
        public async Task<Role> CreateRole(
            [Service] IMediator mediator,
            CreateRoleInput input)
        {
            return await mediator.Send(input);
        }
    }
}
