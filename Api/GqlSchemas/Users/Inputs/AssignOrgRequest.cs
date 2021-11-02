using System.Collections.Generic;
using MediatR;
using MongoDB.Bson;

namespace Geex.Common.Identity.Api.GqlSchemas.Users.Inputs
{
    public record AssignOrgRequest : IRequest<Unit>
    {
        public List<string> UserIds { get; set; }
        public List<string> Orgs { get; set; }
    }
}