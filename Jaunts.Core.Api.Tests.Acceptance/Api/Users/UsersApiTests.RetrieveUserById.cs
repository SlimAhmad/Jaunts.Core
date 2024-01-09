using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Users;

namespace Jaunts.Core.Api.Tests.Acceptance.Api.Users
{
    public partial class UsersApiTests
    {
        [Fact]
        public async Task ShouldRetrieveUserByIdAsync()
        {
            // given
            List<ApplicationUser> retrievedUsers = 
                await jauntsApiBroker.GetAllUsersAsync();
            ApplicationUser retrievedUser = retrievedUsers.FirstOrDefault();
            ApplicationUser expectedUser = retrievedUser;

            // when
            ApplicationUser actualUser =
                 await this.jauntsApiBroker.GetUserByIdAsync(retrievedUser.Id);

            // then
            actualUser.Should().BeEquivalentTo(expectedUser);
        }

    }
}
