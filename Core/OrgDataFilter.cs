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
    public class OrgDataFilter : DataFilter<IOrgFilteredEntity>
    {
        public LazyFactory<ClaimsPrincipal> ClaimsPrincipal { get; }

        public override FilterDefinition<IOrgFilteredEntity> Apply(FilterDefinition<IOrgFilteredEntity> filter)
        {
            var ownedOrgs = ClaimsPrincipal.Value.FindOrgIds();
            if (ownedOrgs?.Any() == true)
            {
                Expression<Func<IOrgFilteredEntity, bool>> expression = (x) => ownedOrgs.Intersect(x.BelongedOrgs).Any();
                filter &= expression;
            }
            else
            {
                //没有组织架构的用户直接不返回数据
                filter = new ExpressionFilterDefinition<IOrgFilteredEntity>(x => false);
            }
            return filter;
        }

        public OrgDataFilter(LazyFactory<ClaimsPrincipal> claimsPrincipal)
        {
            ClaimsPrincipal = claimsPrincipal;
        }
    }

    public interface IOrgFilteredEntity : IEntity
    {
        public List<string> BelongedOrgs { get; set; }
    }
}
