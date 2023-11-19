using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Models.Email;

namespace Jaunts.Core.Api.Services.Processings.Jwt
{
    public partial interface IJwtProcessingService
    {
        ValueTask<string> GenerateJwtTokenAsync(ApplicationUser user, int permissions);
    }
}
