using MediatR;
using MongoDB.Bson;

namespace Geex.Common.Identity.Api.GqlSchemas.Users.Inputs
{
    public class UploadProfileRequest : IRequest<Unit>
    {
        public ObjectId UserId { get; set; }
        public string? NewAvatar { get; set; }
    }
}