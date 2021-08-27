using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Geex.Common.Abstraction;
using Geex.Common.Identity.Api.Aggregates.Roles;
using Geex.Common.Identity.Api.Aggregates.Users;
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
                RoleIds = new[] { "000000000000000000000001" }
            };
            dbContext.Attach(user);
            var role = new Role("admin")
            {
                Id = "000000000000000000000001",
            };
            dbContext.Attach(role);
            await role.SaveAsync();
            await user.SaveAsync();
        }
    }
}
