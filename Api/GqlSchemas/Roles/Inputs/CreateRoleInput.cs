using Geex.Common.Identity.Api.Aggregates.Roles;
using HotChocolate;
using MediatR;

namespace Geex.Common.Identity.Api.GqlSchemas.Roles.Inputs
{
    public class CreateRoleInput : IRequest<Role>
    {
        public string RoleName { get; set; }
        public Optional<bool> IsDefault { get; set; } = false;
        public Optional<bool> IsStatic { get; set; } = false;
    }
}