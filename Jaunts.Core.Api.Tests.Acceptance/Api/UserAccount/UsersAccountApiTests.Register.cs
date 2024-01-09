using FluentAssertions;
using Jaunts.Core.Api.Models.Auth;
using Jaunts.Core.Api.Models.Services.Foundations.Users;

namespace Jaunts.Core.Api.Tests.Acceptance.Api.UsersAccount
{
    public partial class UsersAccountApiTests
    {
        [Fact]
        public async Task ShouldPostUserAsync()
        {
            // given
            DateTimeOffset dateTime = GetCurrentDateTime();
            ApplicationUser randomUser = CreateRandomUser(dateTime);
            ApplicationUser inputUser = randomUser;
            ApplicationUser expectedUser = inputUser;

            var userCredentials = new UserCredentialsRequest
            {
                Email = inputUser.Email,
                FirstName = inputUser.FirstName,
                LastName = inputUser.LastName,
                Id = inputUser.Id,
                PhoneNumber = inputUser.PhoneNumber,
                Username = inputUser.UserName,
                Password = "Test@123",
                CreatedDate = inputUser.CreatedDate,
                UpdatedDate = inputUser.UpdatedDate,
            };

            // when 
            UserAccountDetailsResponse user =  await this.jauntsApiBroker.RegisterUserRequestAsync(userCredentials);

            ApplicationUser actualUser =
                await this.jauntsApiBroker.GetUserByIdAsync(user.Id);

            // then
            actualUser.Should().NotBeNull();

            await this.jauntsApiBroker.DeleteUserByIdAsync(actualUser.Id);
        }

    }
}
