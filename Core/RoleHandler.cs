using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Geex.Common.Abstraction.Gql.Inputs;
using Geex.Common.Abstractions.Enumerations;
using Geex.Common.Identity.Api.Aggregates.Roles;
using Geex.Common.Identity.Api.Aggregates.Roles;
using Geex.Common.Identity.Api.GqlSchemas.Roles.Inputs;

using Mediator;

using MediatR;

using Microsoft.AspNetCore.Identity;

using MongoDB.Entities;

namespace Geex.Common.Identity.Core
{
    public class RoleHandler :
        IRequestHandler<QueryInput<Role>, IQueryable<Role>>,
        IRequestHandler<CreateRoleInput, Role>
    {
        public DbContext DbContext { get; }

        public RoleHandler(DbContext dbContext)
        {
            DbContext = dbContext;
        }

        /// <summary>Handles a request</summary>
        /// <param name="request">The request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Response from the request</returns>
        public async Task<IQueryable<Role>> Handle(QueryInput<Role> request, CancellationToken cancellationToken)
        {
            return DbContext.Queryable<Role>();
        }

        /// <summary>Handles a request</summary>
        /// <param name="request">The request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Response from the request</returns>
        public async Task<Role> Handle(CreateRoleInput request, CancellationToken cancellationToken)
        {
            var role = new Role(request.RoleName);
            DbContext.Attach(role);
            await role.SaveAsync(cancellation: cancellationToken);
            return role;
        }
    }
}
