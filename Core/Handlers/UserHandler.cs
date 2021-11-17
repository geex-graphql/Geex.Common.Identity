﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Geex.Common.Abstraction.Gql.Inputs;
using Geex.Common.Identity.Api.Aggregates.Roles;
using Geex.Common.Identity.Api.Aggregates.Users;
using Geex.Common.Identity.Api.GqlSchemas.Users.Inputs;
using Geex.Common.Identity.Core.Aggregates.Orgs;
using Geex.Common.Identity.Core.Aggregates.Users;

using Mediator;

using MediatR;

using Microsoft.AspNetCore.Identity;

using MongoDB.Bson;
using MongoDB.Entities;

namespace Geex.Common.Identity.Core.Handlers
{
    public class UserHandler :
        IRequestHandler<AssignRoleRequest, Unit>,
        IRequestHandler<AssignOrgRequest, Unit>,
        IRequestHandler<CreateUserRequest, Unit>,
        IRequestHandler<EditUserRequest, Unit>,
        IRequestHandler<RegisterUserRequest, IUser>,
        IRequestHandler<QueryInput<IUser>, IQueryable<IUser>>
    {
        public DbContext DbContext { get; }
        public IUserCreationValidator UserCreationValidator { get; }
        public IPasswordHasher<IUser> PasswordHasher { get; }

        public UserHandler(DbContext dbContext,
         IUserCreationValidator userCreationValidator,
            IPasswordHasher<IUser> passwordHasher)
        {
            DbContext = dbContext;
            UserCreationValidator = userCreationValidator;
            PasswordHasher = passwordHasher;
        }
        /// <summary>Handles a request</summary>
        /// <param name="request">The request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Response from the request</returns>
        public async Task<Unit> Handle(AssignRoleRequest request, CancellationToken cancellationToken)
        {
            var users = await Task.FromResult(DbContext.Queryable<User>().Where(x => request.UserIds.Contains(x.Id)).ToList());
            var roles = await Task.FromResult(DbContext.Queryable<Role>().Where(x => request.Roles.Contains(x.Name)).ToList());
            foreach (var user in users)
            {
                await user.AssignRoles(roles);
            }
            return Unit.Value;
        }

        /// <summary>Handles a request</summary>
        /// <param name="request">The request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Response from the request</returns>
        public async Task<Unit> Handle(EditUserRequest request, CancellationToken cancellationToken)
        {
            var user = await DbContext.Queryable<User>().OneAsync(request.Id.ToString(), cancellationToken);
            request.SetEntity(user, nameof(User.Password), nameof(user.RoleNames));
            await user.AssignRoles(request.RoleNames);
            await user.AssignOrgs(request.OrgCodes);
            if (!request.Password.IsNullOrEmpty())
            {
                user.SetPassword(request.Password);
            }
            return Unit.Value;
        }

        /// <summary>Handles a request</summary>
        /// <param name="request">The request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Response from the request</returns>
        public async Task<Unit> Handle(CreateUserRequest request, CancellationToken cancellationToken)
        {
            var identifier = request.PhoneNumber.IsNullOrEmpty() ? request.Email : request.PhoneNumber;
            var user = new User(this.UserCreationValidator, this.PasswordHasher, identifier, request.Password);
            request.SetEntity(user, nameof(User.Password));
            DbContext.Attach(user);
            return Unit.Value;
        }

        /// <summary>Handles a request</summary>
        /// <param name="request">The request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Response from the request</returns>
        public async Task<IUser> Handle(RegisterUserRequest request, CancellationToken cancellationToken)
        {
            var user = new User(UserCreationValidator, PasswordHasher, request.PhoneOrEmail, request.Password);
            return user;
        }

        /// <summary>Handles a request</summary>
        /// <param name="request">The request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Response from the request</returns>
        public async Task<IQueryable<IUser>> Handle(QueryInput<IUser> request, CancellationToken cancellationToken)
        {
            return DbContext.Queryable<User>();
        }

        /// <summary>Handles a request</summary>
        /// <param name="request">The request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Response from the request</returns>
        public async Task<Unit> Handle(AssignOrgRequest request, CancellationToken cancellationToken)
        {
            var users = DbContext.Queryable<User>().Where(x => request.UserIds.Contains(x.Id)).ToList();
            var orgs = DbContext.Queryable<Org>().Where(x => request.Orgs.Contains(x.Code)).ToList();
            foreach (var user in users)
            {
                await user.AssignOrgs(orgs);
            }
            return Unit.Value;
        }
    }
}
