using FluentAssertions;
using Jaunts.Core.Api.Models.Auth;
using Jaunts.Core.Api.Models.Services.Foundations.Users;

namespace Jaunts.Core.Api.Tests.Acceptance.Api.UsersAccount
{
    public partial class UsersAccountApiTests
    {
        [Fact]
        public async Task ShouldRetrieveUserByIdAsync()
        {
            // given
            List<ApplicationUser> retrievedUsersAccount = 
                await jauntsApiBroker.GetAllUsersAsync();
            ApplicationUser retrievedUser = retrievedUsersAccount.FirstOrDefault();
            ApplicationUser expectedUser = retrievedUser;

            // when
            UserAccountDetailsResponse actualUser =
                 await this.jauntsApiBroker.EmailConfirmationAsync("",retrievedUser.Email);

            // then
            actualUser.Should().BeEquivalentTo(expectedUser);
        }

    }
}
