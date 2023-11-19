using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Microsoft.AspNetCore.Identity;

namespace Jaunts.Core.Api.Services.Foundations.Users
{
    public interface IUserService
    {
        ValueTask<ApplicationUser> RegisterUserRequestAsync(ApplicationUser user, string password);
        IQueryable<ApplicationUser> RetrieveAllUsers();
        ValueTask<ApplicationUser> RetrieveUserByIdRequestAsync(Guid userId);
        ValueTask<ApplicationUser> ModifyUserRequestAsync(ApplicationUser user);
        ValueTask<ApplicationUser> RemoveUserByIdRequestAsync(Guid userId);
        ValueTask<string> GenerateEmailConfirmationTokenRequestAsync(ApplicationUser user);
        ValueTask<string> GeneratePasswordResetTokenRequestAsync(ApplicationUser user);
        ValueTask<string> GenerateTwoFactorTokenRequestAsync(ApplicationUser user);
        ValueTask<bool> CheckPasswordRequestAsync(ApplicationUser user, string password);
        ValueTask<ApplicationUser> ResetPasswordRequestAsync(ApplicationUser user, string token, string password);
        ValueTask<ApplicationUser> SetTwoFactorEnabledRequestAsync(ApplicationUser user, bool enabled);
        ValueTask<List<string>> RetrieveUserRolesRequestAsync(ApplicationUser user);
        ValueTask<ApplicationUser> AddToRoleRequestAsync(ApplicationUser user, string role);
        ValueTask<ApplicationUser> ConfirmEmailRequestAsync(ApplicationUser user, string token);
    }
}
