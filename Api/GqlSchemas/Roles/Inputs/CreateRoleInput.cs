using Geex.Common.Identity.Api.Aggregates.Roles;
using MediatR;

namespace Geex.Common.Identity.Api.GqlSchemas.Roles.Inputs
{
    public class CreateRoleInput : IRequest<Role>
    {
        public string RoleName { get; set; }
    }
}