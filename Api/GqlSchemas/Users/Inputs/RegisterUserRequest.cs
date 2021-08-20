using Geex.Common.Identity.Core.Aggregates.Users;
using MediatR;

namespace Geex.Common.Identity.Api.GqlSchemas.Users.Inputs
{
    public record RegisterUserRequest : IRequest<User>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PhoneOrEmail { get; set; }
    }
}