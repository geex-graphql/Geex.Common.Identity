using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Geex.Common.Identity.Api.Aggregates.Users;

using MediatR;

using Volo.Abp;

namespace Geex.Common.Identity.Api.GqlSchemas.Users.Inputs
{
    public record CreateUserRequest : IRequest<Unit>
    {
        [Obsolete]
        public CreateUserRequest()
        {

        }


        public static CreateUserRequest CreateInstance(string username, string? phoneNumber, string? email,
            string password, bool isEnable = true)
        {
            return new CreateUserRequest
            {
                Id = null,
                Username = username,
                IsEnable = isEnable,
                Email = email,
                RoleNames = new List<string>(),
                OrgCodes = new List<string>(),
                AvatarFileId = null,
                Claims = new UserClaim[]
                {
                },
                PhoneNumber = phoneNumber,
                Password = password
            };
        }

        public string Username { get; set; }

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
