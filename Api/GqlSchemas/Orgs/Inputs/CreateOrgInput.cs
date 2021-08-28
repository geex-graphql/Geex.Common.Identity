using Geex.Common.Identity.Core.Aggregates.Orgs;

using MediatR;

namespace Geex.Common.Identity.Api.GqlSchemas.Roles.Inputs
{
    public class CreateOrgInput : IRequest<Org>
    {
        public string Name { get; set; }
        public string Code { get; set; }
    }
}