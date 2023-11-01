using Jaunts.Core.Api.Models.Services.Foundations.Users;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Jaunts.Core.Api.Tests.Acceptance.Brokers.UserManagement
{
    public interface IUserManagementBroker
    {
        ValueTask<ApplicationUser> InsertUserAsync(ApplicationUser user, string password);
        IQueryable<ApplicationUser> SelectAllUsers();
        ValueTask<ApplicationUser> SelectUserByIdAsync(Guid userId);
        ValueTask<ApplicationUser> UpdateUserAsync(ApplicationUser user);
        ValueTask<ApplicationUser> DeleteUserAsync(ApplicationUser user);
    }
}
