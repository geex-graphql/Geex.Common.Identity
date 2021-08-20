using MongoDB.Entities;

namespace Geex.Common.Identity.Api.Aggregates.Users
{
    public interface IUser : IEntity
    {
        string PhoneNumber { get; set; }
        string UserName { get; set; }
        public bool IsEnable { get; set; }
        string Email { get; set; }
        string[] Roles { get; }
        string Avatar { get; }
        UserClaim[] Claims { get; set; }
    }
}
