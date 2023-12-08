using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Api.Services.Foundations.Jwt;

namespace Jaunts.Core.Api.Services.Processings.Jwt
{
    public partial class JwtProcessingService : IJwtProcessingService
    {
      
            private readonly IJwtService jwtService;
            private readonly ILoggingBroker loggingBroker;

            public JwtProcessingService(
                IJwtService  jwtService,
                ILoggingBroker loggingBroker

                )
            {
                this.jwtService = jwtService;
                this.loggingBroker = loggingBroker;

            }

           public ValueTask<string> GenerateJwtTokenAsync(
                ApplicationUser user,int permissions) =>
            TryCatch(async () =>
            {
                ValidateUserNotNull(user);
               return await jwtService.GenerateJwtTokenRequestAsync(user,permissions);
        
            });

    





    }
}
