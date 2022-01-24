using System;
using System.Collections.Generic;

using Geex.Common.Identity.Api.Aggregates.Users;

using MediatR;

using MongoDB.Bson;
using MongoDB.Entities;

namespace Geex.Common.Identity.Api.GqlSchemas.Users.Inputs
{
    public class EditUserRequest : IRequest<Unit>
    {
        public string Id { get; set; }
        public bool? IsEnable { get; set; }
        public string? Email { get; set; }
        public List<string> RoleNames { get; set; }
        public List<string> OrgCodes { get; set; }
        public string AvatarFileId { get; set; }
        public List<UserClaim> Claims { get; set; }
        public string? PhoneNumber { get; set; }
        public string Username { get; set; }
    }
}