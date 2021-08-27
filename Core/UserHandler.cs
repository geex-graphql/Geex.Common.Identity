using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Geex.Common.Abstraction.Gql.Inputs;
using Geex.Common.Abstractions.Enumerations;
using Geex.Common.Identity.Api.Aggregates.Roles;
using Geex.Common.Identity.Api.Aggregates.Users;
using Geex.Common.Identity.Api.GqlSchemas.Users.Inputs;
using Geex.Common.Identity.Core.Aggregates.Users;
using Mediator;
using MediatR;
using Microsoft.AspNetCore.Identity;
using MongoDB.Entities;

namespace Geex.Common.Identity.Core
{
    public class UserHandler : IRequestHandler<AssignRoleRequest, Unit>,
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
            var user = await DbContext.Find<User>().OneAsync(request.UserId.ToString(), cancellationToken);
            var roles = await DbContext.Find<Role>().ManyAsync(x => request.Roles.Contains(x.Id), cancellationToken);
            await user.AssignRoles(roles);
            return Unit.Value;
        }

        /// <summary>Handles a request</summary>
        /// <param name="request">The request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Response from the request</returns>
        public async Task<Unit> Handle(EditUserRequest request, CancellationToken cancellationToken)
        {
            var user = await DbContext.Find<User>().OneAsync(request.Id.ToString(), cancellationToken);
            request.SetEntity(user);
            await user.SaveAsync(cancellation: cancellationToken);
            return Unit.Value;
        }

        /// <summary>Handles a request</summary>
        /// <param name="request">The request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Response from the request</returns>
        public async Task<IUser> Handle(RegisterUserRequest request, CancellationToken cancellationToken)
        {
            var user = new User(UserCreationValidator, PasswordHasher, request.PhoneOrEmail, request.Password, request.UserName);
            await user.SaveAsync(cancellationToken);
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
    }
}
