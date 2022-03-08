using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geex.Common.Abstraction;
using Geex.Common.Identity.Api.Aggregates.Roles;
using MongoDB.Bson.Serialization;

namespace Geex.Common.Identity.Core.EntityMapConfigs.Roles
{
    public class RoleMapConfig : IEntityMapConfig<Role>
    {
        public void Map(BsonClassMap<Role> map)
        {
            map.AutoMap();
        }
    }
}
