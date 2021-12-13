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
        public OrgDataFilter(LazyFactory<ClaimsPrincipal> claimsPrincipal) : base(null, PredicateBuilder.New<IOrgFilteredEntity>(entity => claimsPrincipal.Value.FindUserId() == "000000000000000000000001" || claimsPrincipal.Value.FindOrgCodes().Contains(entity.OrgCode)))
        {

        }
    }

    public interface IOrgFilteredEntity : IEntity
    {
        public string OrgCode { get; }
    }
}
