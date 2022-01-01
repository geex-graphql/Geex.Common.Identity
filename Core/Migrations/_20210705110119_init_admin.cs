using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Geex.Common.Abstraction;
using Geex.Common.Abstraction.Authorization;
using Geex.Common.Authorization;
using Geex.Common.Identity.Api.Aggregates.Roles;
using Geex.Common.Identity.Api.Aggregates.Users;
using Geex.Common.Identity.Core.Aggregates.Orgs;
using Geex.Common.Identity.Core.Aggregates.Users;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

using MongoDB.Entities;

using NetCasbin.Abstractions;

namespace Geex.Core.Authentication.Migrations
{
    public class _20210705110119_init_admin : IMigration
    {
        public async Task UpgradeAsync(DbContext dbContext)
        {
            await dbContext.SaveChanges();
            var superAdmin = User.New(dbContext.ServiceProvider.GetService<IUserCreationValidator>(), dbContext.ServiceProvider.GetService<IPasswordHasher<IUser>>(), "superAdmin", "15055555555", "superAdmin@fms.kuanfang.com", "superAdmin");
            dbContext.Attach(superAdmin);
            superAdmin.Id = "000000000000000000000001";
            await dbContext.SaveChanges();
            var adminRole = new Role("admin");
            var userRole = new Role("user");
            var roles = new List<Role>()
            {
                adminRole,
                userRole
            };
            dbContext.Attach(roles);
            adminRole.Id = "000000000000000000000001";
            userRole.Id = "000000000000000000000002";
            var admin = User.New(dbContext.ServiceProvider.GetService<IUserCreationValidator>(), dbContext.ServiceProvider.GetService<IPasswordHasher<IUser>>(), "admin", "13333333332", "admin@fms.kuanfang.com", "admin");
            dbContext.Attach(admin);
            admin.Id = "000000000000000000000002";
            await admin.AssignRoles("admin");
            var user = User.New(dbContext.ServiceProvider.GetService<IUserCreationValidator>(), dbContext.ServiceProvider.GetService<IPasswordHasher<IUser>>(), "user", "15555555555", "user@fms.kuanfang.cn", "user");
            dbContext.Attach(user);
            user.Id = "000000000000000000000003";
            await user.AssignRoles("user");
            await dbContext.ServiceProvider.GetService<IRbacEnforcer>().SetPermissionsAsync(adminRole, AppPermission.List.Select(x => x.Value).ToArray());
        }
    }
}
