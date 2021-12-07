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

using Volo.Abp;

namespace Geex.Common.Identity.Core.Handlers
{
    public class UserHandler :
        IRequestHandler<AssignRoleRequest, Unit>,
        IRequestHandler<AssignOrgRequest, Unit>,
        IRequestHandler<CreateUserRequest, Unit>,
        IRequestHandler<EditUserRequest, Unit>,
        IRequestHandler<ResetUserPasswordRequest>,
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
            return Unit.Value;
        }

        /// <summary>Handles a request</summary>
        /// <param name="request">The request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Response from the request</returns>
        public async Task<Unit> Handle(CreateUserRequest request, CancellationToken cancellationToken)
        {
            var user = User.CreateInstance(this.UserCreationValidator, this.PasswordHasher, request.Username, request.PhoneNumber, request.Email, request.Password);
            request.SetEntity(user, nameof(User.Password));
            DbContext.Attach(user);
            return Unit.Value;
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

        /// <summary>Handles a request</summary>
        /// <param name="request">The request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Response from the request</returns>
        public async Task<Unit> Handle(ResetUserPasswordRequest request, CancellationToken cancellationToken)
        {
            var user = DbContext.Queryable<User>().FirstOrDefault(x => request.UserId == x.Id);
            Check.NotNull(user, nameof(user), "用户不存在.");
            user.SetPassword(request.Password);
            return Unit.Value;
        }
    }
}
