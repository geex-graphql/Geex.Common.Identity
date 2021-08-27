using Geex.Common.Identity.Core.Aggregates.Users;
using MongoDB.Entities;
using Entity = MongoDB.Entities.Entity;

namespace Geex.Common.Identity.Core.Aggregates.Orgs
{
    public class Org : Entity
    {
        public string Code { get; set; }

        public Org()
        {
        }
    }
}