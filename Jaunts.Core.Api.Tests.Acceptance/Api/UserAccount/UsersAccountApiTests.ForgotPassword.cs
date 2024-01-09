using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Users;

namespace Jaunts.Core.Api.Tests.Acceptance.Api.UsersAccount
{
    public partial class UsersAccountApiTests
    {
        [Fact]
        public async Task ShouldForgetPasswordAsync()
        {
            // given
            List<ApplicationUser> retrievedUsers =
                   await jauntsApiBroker.GetAllUsersAsync();
            ApplicationUser retrievedUser = retrievedUsers.FirstOrDefault();
            ApplicationUser expectedUser = retrievedUser;

            // when 
            bool actualUser =
                await this.jauntsApiBroker.ForgotPasswordRequestAsync(retrievedUser.Email);      

            // then
            actualUser.Should().BeTrue(); 

        }

    }
}
