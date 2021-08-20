using System.Text.RegularExpressions;
using Geex.Common.Identity.Api.Aggregates.Roles;
using HotChocolate;
using HotChocolate.Types;

namespace Geex.Common.Identity.Api.GqlSchemas.Roles.Types
{
    public class RoleGqlType : ObjectType<Role>
    {
        protected override void Configure(IObjectTypeDescriptor<Role> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.ConfigEntity();
            //descriptor.Field(x => x.Users).Type<ListType<UserType>>().Resolve(x=>x.ToString());
            descriptor.Field(x => x.Name);
            base.Configure(descriptor);
        }
    }

    public abstract class RegexStringType : ScalarType
    {
        protected RegexStringType(NameString name, string pattern) : base(name)
        {
            this.Regex = new Regex(pattern, RegexOptions.Compiled);
        }

        public Regex Regex { get; set; }
    }
}
