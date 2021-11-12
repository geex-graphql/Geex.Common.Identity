using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Geex.Common.Authorization;

using JetBrains.Annotations;

namespace Geex.Common.Identity
{
    public class IdentityPermissions : AppPermission
    {
        public IdentityPermissions([NotNull] string value) : base(value)
        {
        }
        public class UserPermissions : IdentityPermissions
        {
            public static UserPermissions Query { get; } = new("query_user");
            public static UserPermissions Create { get; } = new("mutation_createUser");
            public static UserPermissions Edit { get; } = new("mutation_editUser");

            public UserPermissions([NotNull] string value) : base(value)
            {
            }
        }
        public class RolePermissions : IdentityPermissions
        {
            public static RolePermissions Query { get; } = new("query_role");
            public static RolePermissions Create { get; } = new("mutation_createRole");
            public static RolePermissions Edit { get; } = new("mutation_editRole");

            public RolePermissions([NotNull] string value) : base(value)
            {
            }
        }
        public class OrgPermissions : IdentityPermissions
        {
            public static OrgPermissions Query { get; } = new("query_org");
            public static OrgPermissions Create { get; } = new("mutation_createOrg");
            public static OrgPermissions Edit { get; } = new("mutation_editOrg");

            public OrgPermissions([NotNull] string value) : base(value)
            {
            }
        }
    }
}
