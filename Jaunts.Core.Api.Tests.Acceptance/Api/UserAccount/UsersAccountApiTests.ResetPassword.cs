using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Models.Auth.LoginRegister;

namespace Jaunts.Core.Api.Tests.Acceptance.Api.UsersAccount
{
    public partial class UsersAccountApiTests
    {
        [Fact]
        public async Task ShouldResetPasswordAsync()
        {
            // given
            List<ApplicationUser> retrievedUsers =
                   await jauntsApiBroker.GetAllUsersAsync();
            ApplicationUser retrievedUser = retrievedUsers.FirstOrDefault();
            ApplicationUser expectedUser = retrievedUser;

            var resetPassword = new ResetPasswordRequest
            {
                Email = "rirazucal@mumuhepukitizo.com",
                ConfirmPassword = "Test@123",
                Password = "Test@123",
                Token = "%20CfDJ8LNxvXjI49pLhOLcbJfX9VDoeWrjMIeFtWkOQePatpYyQ0iNP1XkONsNaCh1e80FfTqF0GXSgpBrroSAMOoVgBoBj%2fw%2fA8%2bhx%2bRukx2d4l8L3Q3nj5Ytmi8BUrztIGNisrjpY4oGto8ZfBSqlQHZ41FHclEox0Ycf3eTmsqOIU4cjXMXUzGK%2bkrMjma6RaurHfj3W5SxdZMYYeX9Tz3BeC3iY16NRADDz%2bQL%2ffz4ODQV"
            };
            // when 
            bool actualUser =
                await this.jauntsApiBroker.ResetPasswordRequestAsync(resetPassword);      

            // then
            actualUser.Should().BeTrue(); 

        }

    }
}
