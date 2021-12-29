using System;
using System.Collections.Generic;
using System.Linq;
using Geex.Common.Abstraction;
using Geex.Common.Abstraction.Storage;
using Geex.Common.Identity.Api.Aggregates.Users;
using Geex.Common.Identity.Core.Aggregates.Users;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NetCasbin.Abstractions;

namespace Geex.Common.Identity.Api.Aggregates.Roles
{
    /// <summary>
    /// role为了方便和string的相互转化, 采用class的形式
    /// </summary>
    public class Role : Entity
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
                var userIds = DbContext.ServiceProvider.GetService<IEnforcer>().GetUsersForRole(this.Name);
                return DbContext.Queryable<User>().Where(x => userIds.Contains(x.Id));
            }
        }
        public List<string> Permissions => DbContext.ServiceProvider.GetService<IMediator>().Send(new GetSubjectPermissionsRequest(this.Name)).Result.ToList();

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
    }
}
