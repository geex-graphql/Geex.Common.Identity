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
            var superAdmin = User.New(dbContext.ServiceProvider.GetService<IUserCreationValidator>(), dbContext.ServiceProvider.GetService<IPasswordHasher<IUser>>(), "superAdmin", "15055555555", "superAdmin@fms.kuanfang.com", "admin");
            superAdmin.Id = "000000000000000000000001";
            dbContext.Attach(superAdmin);
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
