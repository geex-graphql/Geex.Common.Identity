using System.Collections.Generic;
using MediatR;

namespace Geex.Common.Identity.Api.Aggregates.Users.Events
{
    public record UserRoleChangedEvent(string UserId, List<string> Roles) : INotification
    {
    }
}
