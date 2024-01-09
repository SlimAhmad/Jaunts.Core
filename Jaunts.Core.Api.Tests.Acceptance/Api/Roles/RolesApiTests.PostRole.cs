using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Role;

namespace Jaunts.Core.Api.Tests.Acceptance.Api.Roles
{
    public partial class RolesApiTests
    {
        [Fact]
        public async Task ShouldPostRoleAsync()
        {
            // given
            ApplicationRole randomRole = CreateRandomRole();
            ApplicationRole inputRole = randomRole;
            ApplicationRole expectedRole = inputRole;

            // when 
            await this.jauntsApiBroker.PostRoleAsync(inputRole);

            ApplicationRole actualRole =
                await this.jauntsApiBroker.GetRoleByIdAsync(inputRole.Id);

            // then
            actualRole.Should().BeEquivalentTo(expectedRole);

            //await this.jauntsApiBroker.DeleteRoleByIdAsync(actualRole.Id);
        }

        [Fact]
        public async Task ShouldModifyRoleAsync()
        {
            // given
            List<ApplicationRole> retrievedRoles =
                await jauntsApiBroker.GetAllRolesAsync();
            ApplicationRole retrievedRole = retrievedRoles.FirstOrDefault();
            retrievedRole.Name = "Admin";
            retrievedRole.NormalizedName = "ADMIN";
            retrievedRole.CreatedDate.AddDays(GetRandomNegativeNumber()); 
            retrievedRole.UpdatedDate = DateTime.UtcNow;
            ApplicationRole expectedRole = retrievedRole;
            
            // when
            ApplicationRole actualRole =
                 await this.jauntsApiBroker.PostRoleAsync(retrievedRole);

            expectedRole.ConcurrencyStamp =
                actualRole.ConcurrencyStamp;

            // then
            actualRole.Should().BeEquivalentTo(expectedRole);
        }

    }
}
