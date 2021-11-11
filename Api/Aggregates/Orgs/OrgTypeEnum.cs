using Geex.Common.Abstractions;

namespace Geex.Common.Identity.Core.Aggregates.Orgs
{
    public class OrgTypeEnum : Enumeration<OrgTypeEnum, string>
    {
        public OrgTypeEnum(string value) : base(value)
        {

        }
        public static OrgTypeEnum Default { get; } = new OrgTypeEnum(nameof(Default));

    }
}