using Geex.Common.Identity.Api.Aggregates.Users;
using MediatR;

namespace Geex.Common.Identity.Api.GqlSchemas.Users.Inputs
{
    public record RegisterUserRequest : IRequest<IUser>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PhoneOrEmail { get; set; }
    }
}