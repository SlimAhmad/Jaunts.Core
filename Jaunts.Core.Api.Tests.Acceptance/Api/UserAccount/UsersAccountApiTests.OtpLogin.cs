using FluentAssertions;
using Jaunts.Core.Api.Models.Auth;
using Jaunts.Core.Api.Models.Services.Foundations.Users;

namespace Jaunts.Core.Api.Tests.Acceptance.Api.UsersAccount
{
    public partial class UsersAccountApiTests
    {
        [Fact]
        public async Task ShouldLoginWithOtpAsync()
        {
            // given
            List<ApplicationUser> retrievedUsers =
                   await jauntsApiBroker.GetAllUsersAsync();
            ApplicationUser retrievedUser = retrievedUsers.FirstOrDefault();
            ApplicationUser expectedUser = retrievedUser;

            // when 
            UserAccountDetailsResponse actualUser =
                await this.jauntsApiBroker.OtpLoginRequestAsync("",retrievedUser.Email);      

            // then
            actualUser.Should().NotBeNull(); 

        }

    }
}
