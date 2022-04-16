using Geex.Common.Abstraction;
using Geex.Common.Abstraction.Bson;
using Geex.Common.Identity.Core.Aggregates.Users;

using MongoDB.Bson.Serialization;

namespace Geex.Common.Identity.Core.EntityMapConfigs.Users
{
    public class UserMapConfig : EntityMapConfig<User>
    {
        public override void Map(BsonClassMap<User> map)
        {
            map.SetIsRootClass(true);
            map.AutoMap();
        }
    }
}
