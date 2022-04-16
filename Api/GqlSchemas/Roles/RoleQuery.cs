using System.Linq;
using System.Threading.Tasks;

using Geex.Common.Abstraction.Gql.Inputs;
using Geex.Common.Abstraction.Gql.Types;
using Geex.Common.Identity.Api.Aggregates.Roles;
using Geex.Common.Identity.Api.GqlSchemas.Roles.Inputs;
using Geex.Common.Identity.Api.GqlSchemas.Roles.Types;

using HotChocolate;
using HotChocolate.Types;

using MediatR;

using MongoDB.Entities;

namespace Geex.Common.Identity.Api.GqlSchemas.Roles
{
    public class RoleQuery : Query<RoleQuery>
    {
        protected override void Configure(IObjectTypeDescriptor<RoleQuery> descriptor)
        {
            descriptor.AuthorizeWithDefaultName();
            descriptor.ConfigQuery(x => x.Roles(default))
            .UseOffsetPaging<RoleGqlType>()
            .UseFiltering<Role>(x =>
            {
                x.BindFieldsExplicitly();
                x.Field(y => y.Name);
                x.Field(y => y.Users);
            })
            ;
            base.Configure(descriptor);
        }
        public async Task<IQueryable<Role>> Roles(
            [Service] IMediator mediator
            )
        {
            return await mediator.Send(new QueryInput<Role>());
        }
    }
}