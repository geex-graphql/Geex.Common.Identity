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
            var user = new User(dbContext.ServiceProvider.GetService<IUserCreationValidator>(), dbContext.ServiceProvider.GetService<IPasswordHasher<IUser>>(), "admin@fms.kuanfang.com", "admin", "admin")
            {
                Id = "000000000000000000000001",
                RoleNames = new[] { "admin" }
            };
            dbContext.Attach(user);
            await user.SaveAsync();
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
            await roles.SaveAsync();
            var orgs = new List<Org>()
            {
                new Org("1","1"),
                new Org("2","2"),
                new Org("3","3"),
                new Org("1.1","1.1"),
                new Org("1.1.1","1.1.1"),
                new Org("2.1","2.1"),
            };
            dbContext.Attach(orgs);
            await orgs.SaveAsync();
        }
    }
}
