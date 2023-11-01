using Jaunts.Core.Api.Models.Services.Foundations.Users;

namespace Jaunts.Core.Api.Services.Foundations.Users
{
    public interface IUserService
    {
        ValueTask<ApplicationUser> RegisterUserRequestAsync(ApplicationUser user, string password);
        IQueryable<ApplicationUser> RetrieveAllUsers();
        ValueTask<ApplicationUser> RetrieveUserByIdRequestAsync(Guid userId);
        ValueTask<ApplicationUser> ModifyUserRequestAsync(ApplicationUser user);
        ValueTask<ApplicationUser> RemoveUserByIdRequestAsync(Guid userId);
    }
}
