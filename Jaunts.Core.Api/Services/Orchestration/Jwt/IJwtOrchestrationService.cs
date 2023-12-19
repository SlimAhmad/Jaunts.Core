using Jaunts.Core.Api.Models.Auth;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Models.Auth.LoginRegister;

namespace Jaunts.Core.Api.Services.Orchestration.Jwt
{
    public interface IJwtOrchestrationService
    {
        ValueTask<UserAccountDetailsResponse> JwtAccountDetailsAsync(
                ApplicationUser user);
    }
}
