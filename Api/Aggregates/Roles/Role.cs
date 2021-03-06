using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Geex.Common.Abstraction;
using Geex.Common.Abstraction.Authorization;
using Geex.Common.Abstraction.MultiTenant;
using Geex.Common.Identity.Api.Aggregates.Users;
using Geex.Common.Identity.Core.Aggregates.Users;

using HotChocolate;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

using MongoDB.Entities;

using NetCasbin.Abstractions;

using Entity = Geex.Common.Abstraction.Storage.Entity;

namespace Geex.Common.Identity.Api.Aggregates.Roles
{
    /// <summary>
    /// role为了方便和string的相互转化, 采用class的形式
    /// </summary>
    public class Role : Entity, ITenantFilteredEntity
    {
        public string Name { get; set; }

        public Role(string name)
        {
            this.Name = name;
        }

        public IQueryable<IUser> Users
        {
            get
            {
                var userIds = DbContext.ServiceProvider.GetService<IRbacEnforcer>().GetUsersForRole(this.Name);
                return DbContext.Queryable<User>().Where(x => userIds.Contains(x.Id));
            }
        }
        public List<string> Permissions => DbContext.ServiceProvider.GetService<IMediator>().Send(new GetSubjectPermissionsRequest(this.Name)).Result.ToList();

        public string? TenantCode { get; set; }
        public bool IsDefault { get; set; }
        public bool IsStatic { get; set; }
        public bool IsEnabled { get; set; } = true;

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Role);
        }

        public bool Equals(Role other)
        {
            return other != null &&
                   this.Name == other.Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.Name);
        }

        /// <summary>Implicitly calls ToString().</summary>
        /// <param name="path"></param>
        public static implicit operator string(Role path)
        {
            return path.Name;
        }

        /// <summary>
        /// Implicitly creates a new Role from the given string.
        /// </summary>
        /// <param name="s"></param>
        public static implicit operator Role(string s)
        {
            return new Role(s);
        }

        public static bool operator ==(Role left, Role right)
        {
            return EqualityComparer<Role>.Default.Equals(left, right);
        }

        public static bool operator !=(Role left, Role right)
        {
            return !(left == right);
        }

        public static Role Create(string roleName, bool isStatic = false, bool isDefault = false)
        {
            return new Role(roleName)
            {
                IsStatic = isStatic,
                IsDefault = isDefault
            };
        }
        public override async Task<ValidationResult> Validate(IServiceProvider sp, CancellationToken cancellation = default)
        {
            return ValidationResult.Success;
        }
    }
}
