using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Geex.Common.Abstraction.Gql.Inputs;
using Geex.Common.Gql.Roots;
using Geex.Common.Identity.Api.GqlSchemas.Orgs.Types;
using Geex.Common.Identity.Core.Aggregates.Orgs;

using HotChocolate;
using HotChocolate.Types;

using MediatR;

namespace Geex.Common.Identity.Api.GqlSchemas.Orgs
{
    public class OrgQuery : QueryTypeExtension<OrgQuery>
    {
        protected override void Configure(IObjectTypeDescriptor<OrgQuery> descriptor)
        {
            descriptor.AuthorizeWithDefaultName();
            descriptor.ConfigQuery(x => x.Orgs(default))
            .UseOffsetPaging<OrgGqlType>()
            .UseFiltering<Org>(x =>
            {
                x.BindFieldsExplicitly();
                x.Field(y => y.Name);
                x.Field(y => y.Code);
                x.Field(y => y.ParentOrgCode);
                x.Field(y => y.OrgType);
            })
            ;
            base.Configure(descriptor);
        }
        public async Task<IQueryable<Org>> Orgs(
            [Service] IMediator mediator
            )
        {
            return await mediator.Send(new QueryInput<Org>());
        }
    }
}
