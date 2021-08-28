using System.Linq;
using System.Text.RegularExpressions;

using Geex.Common.Identity.Core.Aggregates.Users;

using MongoDB.Entities;

using Entity = MongoDB.Entities.Entity;

namespace Geex.Common.Identity.Core.Aggregates.Orgs
{
    public class Org : Entity
    {
        /// <summary>
        /// ��.��Ϊ�ָ��ߵı���
        /// </summary>
        public string Code { get; set; }
        public string Name { get; set; }
        public IQueryable<Org> AllSubOrgs => DbContext.Queryable<Org>().Where(x => x.Code.StartsWith(this.Code + "."));
        public IQueryable<Org> DirectSubOrgs => DbContext.Queryable<Org>().Where(x => new Regex($@"^{this.Code}\.\w+(?!\.)$").IsMatch(x.Code));

        public Org()
        {
        }

        public Org(string code, string name)
        {
            this.Code = code;
            this.Name = name;
        }
    }
}