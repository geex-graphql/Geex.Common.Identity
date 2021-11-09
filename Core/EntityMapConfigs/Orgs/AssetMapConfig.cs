using Geex.Common.Abstraction;
using Geex.Common.Abstraction.Bson;
using Geex.Common.Identity.Core.Aggregates.Orgs;

using MongoDB.Bson.Serialization;

namespace Geex.Common.Identity.Core.EntityMapConfigs.Orgs
{
    public class OrgMapConfig : EntityMapConfig<Org>
    {
        public override void Map(BsonClassMap<Org> map)
        {
            map.AutoMap();
            map.MapProperty(x => x.OrgType).SetSerializer(new EnumerationSerializer<OrgTypeEnum, string>());
        }
    }
}
