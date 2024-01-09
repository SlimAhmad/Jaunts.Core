using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Users;

namespace Jaunts.Core.Api.Tests.Acceptance.Api.Users
{
    public partial class UsersApiTests
    {
        [Fact]
        public async Task ShouldRemoveUserAsync()
        {
            // given
            List<ApplicationUser> retrievedUsers =
                   await jauntsApiBroker.GetAllUsersAsync();
            ApplicationUser retrievedUser = retrievedUsers.FirstOrDefault();
            ApplicationUser expectedUser = retrievedUser;

            // when 
            bool actualUser =
                await this.jauntsApiBroker.DeleteUserByIdAsync(retrievedUser.Id);      

            // then
            actualUser.Should().BeTrue(); 

        }

    }
}
