using Jaunts.Core.Api.Models.Services.Foundations.Users;

namespace Jaunts.Core.Api.Brokers.Events
{
    public partial interface IEventBroker
    {
        void ListenToUserEvent(Func<ApplicationUser, ValueTask<ApplicationUser>> userEventHandler);
        ValueTask PublishUserEventAsync(ApplicationUser user);
    }
}
