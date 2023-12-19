using Jaunts.Core.Api.Brokers.DateTimes;
using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Models.Services.Foundations.Users;

namespace Jaunts.Core.Api.Services.Foundations.Jwt
{
    public partial class JwtService : IJwtService
    {

        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public JwtService(
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<string> GenerateJwtTokenRequestAsync(
            ApplicationUser user, int permission) =>
        TryCatch( async () =>
        {
            ValidateUser(user);
            string token = user.GenerateJwtToken(permission);
            return token;

        });

    
    }
}
