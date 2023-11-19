using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Microsoft.AspNetCore.Identity;

namespace Jaunts.Core.Api.Services.Foundations.Jwt
{
    public partial interface IJwtService
    {
        ValueTask<string> GenerateJwtTokenRequestAsync(ApplicationUser user,int Permissions);
    }
}
