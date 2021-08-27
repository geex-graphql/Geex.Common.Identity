using System.Threading.Tasks;
using Geex.Common.Gql.Roots;
using Geex.Common.Identity.Api.GqlSchemas.Users.Inputs;
using HotChocolate;
using MediatR;

namespace Geex.Common.Identity.Api.GqlSchemas.Users
{
    public class UserMutation : MutationTypeExtension<UserMutation>
    {
        public async Task<bool> Register(
            [Service] IMediator mediator,
            RegisterUserRequest input)
        {
            var result = await mediator.Send(input);
            return true;
        }

        public async Task<bool> AssignRoles([Service] IMediator mediator, AssignRoleRequest input)
        {
            var result = await mediator.Send(input);
            return true;
        }

        public async Task<bool> EditUser([Service] IMediator mediator, EditUserRequest input)
        {
            var result = await mediator.Send(input);
            return true;
        }

    }
}