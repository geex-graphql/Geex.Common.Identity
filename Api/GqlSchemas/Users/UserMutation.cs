﻿using System.Threading.Tasks;
using Geex.Common.Abstraction.Gql.Types;
using Geex.Common.Identity.Api.GqlSchemas.Users.Inputs;

using HotChocolate;
using HotChocolate.Types;

using MediatR;

namespace Geex.Common.Identity.Api.GqlSchemas.Users
{
    public class UserMutation : Mutation<UserMutation>
    {
        protected override void Configure(IObjectTypeDescriptor<UserMutation> descriptor)
        {
            descriptor.AuthorizeWithDefaultName();
            base.Configure(descriptor);
        }

        public async Task<bool> AssignRoles([Service] IMediator mediator, AssignRoleRequest input)
        {
            var result = await mediator.Send(input);
            return true;
        }

        public async Task<bool> AssignOrgs([Service] IMediator mediator, AssignOrgRequest input)
        {
            var result = await mediator.Send(input);
            return true;
        }

        public async Task<bool> EditUser([Service] IMediator mediator, EditUserRequest input)
        {
            var result = await mediator.Send(input);
            return true;
        }
        public async Task<bool> CreateUser([Service] IMediator mediator, CreateUserRequest input)
        {
            var result = await mediator.Send(input);
            return true;
        }

        public async Task<bool> ResetUserPassword([Service] IMediator mediator, ResetUserPasswordRequest input)
        {
            var result = await mediator.Send(input);
            return true;
        }
    }
}