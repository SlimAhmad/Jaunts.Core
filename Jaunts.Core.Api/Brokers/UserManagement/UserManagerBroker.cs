using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Microsoft.AspNetCore.Identity;

namespace Jaunts.Core.Api.Brokers.UserManagement
{
    public partial class UserManagementBroker : IUserManagementBroker
    {
        private readonly UserManager<ApplicationUser> userManagement;

        public UserManagementBroker(UserManager<ApplicationUser> userManager)
        {
            this.userManagement = userManager;
        }

      
    }
}
