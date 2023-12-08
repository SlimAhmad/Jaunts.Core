using Jaunts.Core.Api.Models.Services.Foundations.Users;

namespace Jaunts.Core.Api.Brokers.Events
{
    public partial class EventBroker
    {
        private static Func<ApplicationUser, ValueTask<ApplicationUser>> UserEventHandler;

        public void ListenToUserEvent(Func<ApplicationUser, ValueTask<ApplicationUser>> userEventHandler) =>
            UserEventHandler = userEventHandler;

        public async ValueTask PublishUserEventAsync(ApplicationUser user) =>
            await UserEventHandler(user);
    }
}
