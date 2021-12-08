﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Geex.Common.Authorization;

using JetBrains.Annotations;

namespace Geex.Common.Identity
{
    public class IdentityPermission : AppPermission<IdentityPermission>
    {
        public IdentityPermission([NotNull] string value) : base(value)
        {
        }
        public class UserPermission : IdentityPermission
        {
            public static UserPermission Query { get; } = new("query_user");
            public static UserPermission Create { get; } = new("mutation_createUser");
            public static UserPermission Edit { get; } = new("mutation_editUser");

            public UserPermission([NotNull] string value) : base(value)
            {
            }
        }
        public class RolePermission : IdentityPermission
        {
            public static RolePermission Query { get; } = new("query_role");
            public static RolePermission Create { get; } = new("mutation_createRole");
            public static RolePermission Edit { get; } = new("mutation_editRole");

            public RolePermission([NotNull] string value) : base(value)
            {
            }
        }
        public class OrgPermission : IdentityPermission
        {
            public static OrgPermission Query { get; } = new("query_org");
            public static OrgPermission Create { get; } = new("mutation_createOrg");
            public static OrgPermission Edit { get; } = new("mutation_editOrg");

            public OrgPermission([NotNull] string value) : base(value)
            {
            }
        }
    }
}