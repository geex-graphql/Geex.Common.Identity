using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Geex.Common.Abstraction.Gql.Inputs;
using Geex.Common.Identity.Api.GqlSchemas.Roles.Inputs;
using Geex.Common.Identity.Core.Aggregates.Orgs;
using MediatR;
using MongoDB.Entities;

namespace Geex.Common.Identity.Core.Handlers
{
    public class OrgHandler :
        IRequestHandler<QueryInput<Org>, IQueryable<Org>>,
        IRequestHandler<CreateOrgInput, Org>
    {
        public DbContext DbContext { get; }

        public OrgHandler(DbContext dbContext)
        {
            DbContext = dbContext;
        }

        /// <summary>Handles a request</summary>
        /// <param name="request">The request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Response from the request</returns>
        public async Task<IQueryable<Org>> Handle(QueryInput<Org> request, CancellationToken cancellationToken)
        {
            return DbContext.Queryable<Org>();
        }

        /// <summary>Handles a request</summary>
        /// <param name="request">The request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Response from the request</returns>
        public async Task<Org> Handle(CreateOrgInput request, CancellationToken cancellationToken)
        {
            var entity = new Org(request.Code, request.Name);
            DbContext.Attach(entity);
            await entity.SaveAsync(cancellation: cancellationToken);
            return entity;
        }
    }
}
