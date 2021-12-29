using Geex.Common.Abstraction;
using Geex.Common.Abstraction.Bson;
using Geex.Common.Identity.Core.Aggregates.Users;

using MongoDB.Bson.Serialization;

namespace Geex.Common.Identity.Core.EntityMapConfigs.Users
{
    public class UserMapConfig : IEntityMapConfig<User>
    {
        public void Map(BsonClassMap<User> map)
        {
            map.AutoMap();
            map.MapProperty(x => x.LoginProvider).SetSerializer(new EnumerationSerializer<LoginProviderEnum, string>());
        }
    }
}
