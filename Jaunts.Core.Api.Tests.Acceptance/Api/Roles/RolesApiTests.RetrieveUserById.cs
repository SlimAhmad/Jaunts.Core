using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Role;
using Newtonsoft.Json;
using System.Net;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace Jaunts.Core.Api.Tests.Acceptance.Api.Roles
{
    public partial class RolesApiTests
    {
        [Fact]
        public async Task ShouldRetrieveRoleByIdAsync()
        {
            // given
            List<ApplicationRole> retrievedRoles = 
                await jauntsApiBroker.GetAllRolesAsync();
            ApplicationRole retrievedRole = retrievedRoles.FirstOrDefault();
            ApplicationRole expectedRole = retrievedRole;

            // when
            ApplicationRole actualRole =
                 await this.jauntsApiBroker.GetRoleByIdAsync(retrievedRole.Id);

            // then
            actualRole.Should().BeEquivalentTo(expectedRole);
        }

    }
}
