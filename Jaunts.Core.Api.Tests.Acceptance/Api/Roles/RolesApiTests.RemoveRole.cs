using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Role;

namespace Jaunts.Core.Api.Tests.Acceptance.Api.Roles
{
    public partial class RolesApiTests
    {
        [Fact]
        public async Task ShouldRemoveRoleAsync()
        {
            // given
            List<ApplicationRole> retrievedRoles =
                   await jauntsApiBroker.GetAllRolesAsync();
            ApplicationRole retrievedRole = retrievedRoles.FirstOrDefault();
            ApplicationRole expectedRole = retrievedRole;

            // when 
            bool actualRole =
                await this.jauntsApiBroker.DeleteRoleByIdAsync(retrievedRole.Id);      

            // then
            actualRole.Should().BeTrue(); 

        }

    }
}
