using Jaunts.Core.Api.Models.Services.Foundations.Role;

namespace Jaunts.Core.Api.Tests.Acceptance.Brokers
{
    public partial class JauntsApiBroker
    {
        private const string RolesApiRelativeUrl = "api/role";

        #region CRUD
        public async ValueTask<ApplicationRole> PostRoleAsync(ApplicationRole role) =>
            await this.apiFactoryClient.PostContentAsync(RolesApiRelativeUrl, role);

        public async ValueTask<List<ApplicationRole>> GetAllRolesAsync() =>
          await this.apiFactoryClient.GetContentAsync<List<ApplicationRole>>($"{RolesApiRelativeUrl}/");

        public async ValueTask<ApplicationRole> GetRoleByIdAsync(Guid roleId) =>
            await this.apiFactoryClient.GetContentAsync<ApplicationRole>($"{RolesApiRelativeUrl}/{roleId}");

        public async ValueTask<bool> DeleteRoleByIdAsync(Guid roleId) =>
            await this.apiFactoryClient.DeleteContentAsync<bool>($"{RolesApiRelativeUrl}/{roleId}");

        #endregion

    }
}
