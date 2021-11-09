using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Geex.Common.Abstraction;
using Geex.Common.Abstraction.Storage;
using Geex.Common.Identity.Core.Aggregates.Users;


namespace Geex.Common.Identity.Core.Aggregates.Orgs
{
    public class Org : Entity
    {

        private IQueryable<Org> _allSubOrgsQuery => DbContext.Queryable<Org>().Where(x => x.Code.StartsWith(this.Code + "."));
        private IQueryable<Org> _directSubOrgsQuery => DbContext.Queryable<Org>().Where(x => new Regex($@"^{this.Code}\.\w+(?!\.)$").IsMatch(x.Code));

        /// <summary>
        /// 以.作为分割线的编码
        /// </summary>
        public string Code { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// 所有子组织
        /// </summary>
        public IQueryable<Org> AllSubOrgs => _allSubOrgsQuery;
        /// <summary>
        /// 所有子组织编码
        /// </summary>
        public List<string> AllSubOrgCodes => _allSubOrgsQuery.Select(x => x.Code).ToList();
        /// <summary>
        /// 直系子组织
        /// </summary>
        public IQueryable<Org> DirectSubOrgs => _directSubOrgsQuery;
        /// <summary>
        /// 直系子组织编码
        /// </summary>
        public List<string> DirectSubOrgCodes => _directSubOrgsQuery.Select(x => x.Code).ToList();
        /// <summary>
        /// 所有父组织
        /// </summary>
        public IQueryable<Org> AllParentOrgs => DbContext.Queryable<Org>().Where(x => AllParentOrgCodes.Contains(x.Code));
        /// <summary>
        /// 父组织编码
        /// </summary>
        public string ParentOrgCode => this.Code.Split('.').SkipLast(1).JoinAsString(".");
        /// <summary>
        /// 所有父组织编码
        /// </summary>
        public List<string> AllParentOrgCodes => this.ParentOrgCode.Split('.', StringSplitOptions.RemoveEmptyEntries).Aggregate(new List<string>(), (list, next) => list.Append(
                 list.LastOrDefault().IsNullOrEmpty() ? next : string.Join('.', new[] { list.LastOrDefault(), next })).ToList());
        /// <summary>
        /// 组织类型
        /// </summary>
        public OrgTypeEnum OrgType { get; set; }
        public Org()
        {
        }

        public Org(string code, string name, OrgTypeEnum orgTypeEnum = default)
        {
            this.Code = code;
            this.Name = name;
            this.OrgType = orgTypeEnum ?? OrgTypeEnum.Default;
        }
    }
}