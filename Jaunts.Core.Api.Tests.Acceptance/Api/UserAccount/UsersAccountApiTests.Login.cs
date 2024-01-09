using FluentAssertions;
using Jaunts.Core.Api.Models.Auth;
using Jaunts.Core.Api.Models.Services.Foundations.Users;

namespace Jaunts.Core.Api.Tests.Acceptance.Api.UsersAccount
{
    public partial class UsersAccountApiTests
    {
        [Fact]
        public async Task ShouldLoginAsync()
        {
            // given
            List<ApplicationUser> retrievedUsers =
                   await jauntsApiBroker.GetAllUsersAsync();
            ApplicationUser retrievedUser = retrievedUsers.FirstOrDefault();
            ApplicationUser expectedUser = retrievedUser;

            var loginCredential = new LoginRequest
            {
                UsernameOrEmail = expectedUser.Email,
                Password = "Msssl204"
            };

            // when 
            UserAccountDetailsResponse actualUser =
                await this.jauntsApiBroker.LogInRequestAsync(loginCredential);      

            // then
            actualUser.Should().NotBeNull(); 

        }

    }
}
