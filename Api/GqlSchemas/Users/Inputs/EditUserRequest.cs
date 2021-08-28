using System;

using Geex.Common.Identity.Api.Aggregates.Users;

using MediatR;

using MongoDB.Bson;
using MongoDB.Entities;

namespace Geex.Common.Identity.Api.GqlSchemas.Users.Inputs
{
    public class EditUserRequest : IRequest<Unit>
    {
        public string Id { get; set; }
        public bool IsEnable { get; set; }
        public string? Email { get; set; }
        public string[] RoleNames { get; set; }
        public string[] OrgCodes { get; set; }
        public string AvatarFileId { get; set; }
        public UserClaim[] Claims { get; set; }
        public string? PhoneNumber { get; set; }
        public string UserName { get; set; }
    }
}