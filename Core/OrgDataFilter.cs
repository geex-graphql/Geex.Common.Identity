using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using Geex.Common.Abstractions;

using MongoDB.Driver;
using MongoDB.Entities;
using MongoDB.Entities.Interceptors;

namespace Geex.Common.Identity.Core
{
    public class OrgDataFilter : ExpressionDataFilter<IOrgFilteredEntity>
    {
        private static bool OrgFilterMethod(LazyFactory<ClaimsPrincipal> x, IOrgFilteredEntity y)
        {
            var ownedOrgs = x.Value?.FindOrgIds();
            return ownedOrgs?.Any() == true && ownedOrgs.Intersect(y.OrgIds ?? new List<string>()).Any();
        }

        public OrgDataFilter(LazyFactory<ClaimsPrincipal> claimsPrincipal) : base(entity => OrgFilterMethod(claimsPrincipal, entity), true)
        {
        }
    }

    public interface IOrgFilteredEntity : IEntity
    {
        public List<string> OrgIds { get; }
    }
}
