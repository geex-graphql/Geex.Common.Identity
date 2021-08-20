using System.Collections.Generic;
using MediatR;
using MongoDB.Bson;

namespace Geex.Common.Identity.Api.GqlSchemas.Users.Inputs
{
    public record AddToOrgRequest : IRequest<Unit>
    {
        public ObjectId UserId { get; set; }
        public List<string> Orgs { get; set; }
    }
}