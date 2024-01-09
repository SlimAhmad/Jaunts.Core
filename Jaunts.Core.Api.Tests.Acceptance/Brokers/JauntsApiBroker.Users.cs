using Jaunts.Core.Api.Models.Services.Foundations.Users;

namespace Jaunts.Core.Api.Tests.Acceptance.Brokers
{
    public partial class JauntsApiBroker
    {
        private const string UsersApiRelativeUrl = "api/users";

        #region CRUD

        public async ValueTask<ApplicationUser> PostUserAsync(ApplicationUser user) =>
           await this.apiFactoryClient.PostContentAsync(UsersApiRelativeUrl, user);

        public async ValueTask<List<ApplicationUser>> GetAllUsersAsync() =>
          await this.apiFactoryClient.GetContentAsync<List<ApplicationUser>>($"{UsersApiRelativeUrl}/");

        public async ValueTask<ApplicationUser> GetUserByIdAsync(Guid userId) =>
            await this.apiFactoryClient.GetContentAsync<ApplicationUser>($"{UsersApiRelativeUrl}/{userId}");

        public async ValueTask<bool> DeleteUserByIdAsync(Guid userId) =>
            await this.apiFactoryClient.DeleteContentAsync<bool>($"{UsersApiRelativeUrl}/{userId}");

        #endregion

    }
}
