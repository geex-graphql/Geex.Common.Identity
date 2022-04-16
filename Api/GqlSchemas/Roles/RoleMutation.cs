using System.Threading.Tasks;

using Autofac;
using Geex.Common.Abstraction.Gql.Types;
using Geex.Common.Identity.Api.Aggregates.Roles;
using Geex.Common.Identity.Api.GqlSchemas.Roles.Inputs;

using HotChocolate;
using HotChocolate.Types;
using MediatR;

using MongoDB.Entities;

namespace Geex.Common.Identity.Api.GqlSchemas.Roles
{
    public class RoleMutation : Mutation<RoleMutation>
    {
        protected override void Configure(IObjectTypeDescriptor<RoleMutation> descriptor)
        {
            descriptor.AuthorizeWithDefaultName();
            base.Configure(descriptor);
        }

        public async Task<Role> CreateRole(
            [Service] IMediator mediator,
            CreateRoleInput input)
        {
            return await mediator.Send(input);
        }
    }
}
