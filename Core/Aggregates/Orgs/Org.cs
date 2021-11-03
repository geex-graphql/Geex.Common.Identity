using System.Linq;
using System.Text.RegularExpressions;
using Geex.Common.Abstraction;
using Geex.Common.Abstraction.Storage;
using Geex.Common.Identity.Core.Aggregates.Users;


namespace Geex.Common.Identity.Core.Aggregates.Orgs
{
    public class Org : Entity
    {
        /// <summary>
        /// 以.作为分割线的编码
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