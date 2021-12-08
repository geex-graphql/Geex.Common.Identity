using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Geex.Common.Abstraction;
using Geex.Common.Identity.Api.Aggregates.Roles;
using Geex.Common.Identity.Api.Aggregates.Users;
using Geex.Common.Identity.Core.Aggregates.Orgs;
using Geex.Common.Identity.Core.Aggregates.Users;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

using MongoDB.Entities;

namespace Geex.Core.Authentication.Migrations
{
    public class _20210705110119_init_admin : IMigration
    {
        public async Task UpgradeAsync(DbContext dbContext)
        {
            var user = User.CreateInstance(dbContext.ServiceProvider.GetService<IUserCreationValidator>(), dbContext.ServiceProvider.GetService<IPasswordHasher<IUser>>(), "admin", "15555555555", "admin@fms.kuanfang.com", "admin");
            user.Id = "000000000000000000000001";
            user.RoleNames = new List<string> { "admin" };
            dbContext.Attach(user);
            var roles = new List<Role>()
            {
                new Role("admin")
                {
                    Id = "000000000000000000000001",
                },
                new Role("user")
                {
                    Id = "000000000000000000000002",
                }
            };
            dbContext.Attach(roles);
        }
    }
}
