using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geex.Common.Abstraction;
using Geex.Common.Identity.Api.Aggregates.Users;
using HotChocolate;
using MediatR;

using Volo.Abp;

namespace Geex.Common.Identity.Api.GqlSchemas.Users.Inputs
{
    public record CreateUserRequest : IRequest<IUser>
    {
        public CreateUserRequest()
        {

        }

        public string Username { get; set; }

        public Optional<bool> IsEnable { get; set; } = true;
        public string? Email { get; set; }
        public Optional<List<string>> RoleNames { get; set; } = new List<string>();
        public Optional<List<string>> OrgCodes { get; set; } = new List<string>();
        public string? AvatarFileId { get; set; }
        public List<UserClaim>? Claims { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Password { get; set; }
        public Optional<string> Nickname { get; set; }
        public Optional<string> OpenId { get; set; }
        public Optional<LoginProviderEnum> Provider { get; set; }
    }
}
