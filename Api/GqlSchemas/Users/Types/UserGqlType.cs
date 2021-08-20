using Geex.Common.Identity.Api.Aggregates.Users;
using HotChocolate.Types;

namespace Geex.Common.Identity.Api.GqlSchemas.Users.Types
{
    public class UserGqlType : ObjectType<IUser>
    {
        protected override void Configure(IObjectTypeDescriptor<IUser> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.ConfigEntity();
            descriptor.Field(x => x.UserName);
            descriptor.Field(x => x.IsEnable);
            descriptor.Field(x => x.Email);
            descriptor.Field(x => x.PhoneNumber);
            descriptor.Field(x => x.Roles);
            //descriptor.Ignore(x => x.Claims);
            //descriptor.Ignore(x => x.AuthorizedPermissions);
            base.Configure(descriptor);
        }
    }
}
