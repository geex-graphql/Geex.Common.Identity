using Geex.Common.Identity.Core.Aggregates.Users;
using MongoDB.Entities;
using Entity = MongoDB.Entities.Entity;

namespace Geex.Common.Identity.Core.Aggregates.Orgs
{
    public class Org : Entity
    {
        public string Code { get; set; }
        [OwnerSide]
        public Many<User> Users { get; set; }
        public Many<Org> SubOrgs { get; set; }

        public Org()
        {
            this.InitManyToMany(x => x.Users, user => user.Orgs);
        }
    }
}