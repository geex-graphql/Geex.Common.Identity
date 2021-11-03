using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Geex.Common.Identity.Api.Aggregates.Users;

using MediatR;

namespace Geex.Common.Identity.Api.GqlSchemas.Users.Inputs
{
    public class CreateUserRequest : IRequest<Unit>
    {
        public string Id { get; set; }
        public bool IsEnable { get; set; }
        public string? Email { get; set; }
        public List<string> RoleNames { get; set; }
        public List<string> OrgCodes { get; set; }
        public string AvatarFileId { get; set; }
        public UserClaim[] Claims { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Password { get; set; }
    }
}
