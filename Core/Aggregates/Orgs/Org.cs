using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

using Geex.Common.Abstraction.MultiTenant;
using Geex.Common.Abstraction.Storage;
using Geex.Common.Identity.Api.Aggregates.Orgs.Events;

namespace Geex.Common.Identity.Core.Aggregates.Orgs;

public class Org : Entity, ITenantFilteredEntity
{
    public Org()
    {
    }

    public Org(string code, string name, OrgTypeEnum orgTypeEnum = default)
    {
        Code = code;
        Name = name;
        OrgType = orgTypeEnum ?? OrgTypeEnum.Default;
    }

    private IQueryable<Org> _allSubOrgsQuery => DbContext.Queryable<Org>().Where(x => x.Code.StartsWith(Code + "."));

    private IQueryable<Org> _directSubOrgsQuery =>
        DbContext.Queryable<Org>().Where(x => new Regex($@"^{Code}\.\w+(?!\.)$").IsMatch(x.Code));

    /// <summary>
    ///     ���и���֯����
    /// </summary>
    public List<string> AllParentOrgCodes => ParentOrgCode.Split('.', StringSplitOptions.RemoveEmptyEntries).Aggregate(
        new List<string>(), (list, next) => list.Append(
            list.LastOrDefault().IsNullOrEmpty() ? next : string.Join('.', list.LastOrDefault(), next)).ToList());

    /// <summary>
    ///     ���и���֯
    /// </summary>
    public IQueryable<Org> AllParentOrgs => DbContext.Queryable<Org>().Where(x => AllParentOrgCodes.Contains(x.Code));

    /// <summary>
    ///     ��������֯����
    /// </summary>
    public List<string> AllSubOrgCodes => _allSubOrgsQuery.Select(x => x.Code).ToList();

    /// <summary>
    ///     ��������֯
    /// </summary>
    public IQueryable<Org> AllSubOrgs => _allSubOrgsQuery;

    /// <summary>
    ///     ֱϵ����֯����
    /// </summary>
    public List<string> DirectSubOrgCodes => _directSubOrgsQuery.Select(x => x.Code).ToList();

    /// <summary>
    ///     ֱϵ����֯
    /// </summary>
    public IQueryable<Org> DirectSubOrgs => _directSubOrgsQuery;

    /// <summary>
    ///     ����֯
    /// </summary>
    public Org ParentOrg => DbContext.Queryable<Org>().FirstOrDefault(x => ParentOrgCode == x.Code);

    /// <summary>
    ///     ����֯����
    /// </summary>
    public string ParentOrgCode => Code.Split('.').SkipLast(1).JoinAsString(".");

    /// <summary>
    ///     ��.��Ϊ�ָ��ߵı���
    /// </summary>
    public string Code { get; set; }

    public string Name { get; set; }

    /// <summary>
    ///     ��֯����
    /// </summary>
    public OrgTypeEnum OrgType { get; set; }

    /// <inheritdoc />
    public string? TenantCode { get; set; }

    /// <summary>
    ///     �޸���֯����
    /// </summary>
    /// <param name="newOrgCode"></param>
    public void SetCode(string newOrgCode)
    {
        var originCode = Code;

        var subOrgs = DirectSubOrgs.ToList();
        foreach (var subOrg in subOrgs) subOrg.SetCode(subOrg.Code.Replace(Code, newOrgCode));

        AddDomainEvent(new OrgCodeChangedEvent(originCode, newOrgCode));
    }
    public override async Task<ValidationResult> Validate(IServiceProvider sp, CancellationToken cancellation = default)
    {
        return ValidationResult.Success;
    }
}