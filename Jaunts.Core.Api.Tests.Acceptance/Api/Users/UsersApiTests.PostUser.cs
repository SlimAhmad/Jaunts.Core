using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Users;

namespace Jaunts.Core.Api.Tests.Acceptance.Api.Users
{
    public partial class UsersApiTests
    {
        [Fact]
        public async Task ShouldPostUserAsync()
        {
            // given
            DateTimeOffset dateTime = GetCurrentDateTime();
            ApplicationUser randomUser = CreateRandomUser(dateTime);
            ApplicationUser inputUser = randomUser;
            ApplicationUser expectedUser = inputUser;

            // when 
            await this.jauntsApiBroker.PostUserAsync(inputUser);

            ApplicationUser actualUser =
                await this.jauntsApiBroker.GetUserByIdAsync(inputUser.Id);

            // then
            actualUser.Should().NotBeNull();

            await this.jauntsApiBroker.DeleteUserByIdAsync(actualUser.Id);
        }

        [Fact]
        public async Task ShouldModifyUserAsync()
        {
            // given
            List<ApplicationUser> retrievedUsers =
                await jauntsApiBroker.GetAllUsersAsync();
            ApplicationUser retrievedUser = retrievedUsers.FirstOrDefault();
            retrievedUser.Name = "Ahmad";
            retrievedUser.CreatedDate.AddDays(GetRandomNegativeNumber()); 
            retrievedUser.UpdatedDate = DateTime.UtcNow;
            ApplicationUser expectedUser = retrievedUser;
            
            // when
            ApplicationUser actualUser =
                 await this.jauntsApiBroker.PostUserAsync(retrievedUser);

            expectedUser.ConcurrencyStamp =
                actualUser.ConcurrencyStamp;

            // then
            actualUser.Should().BeEquivalentTo(expectedUser);
        }

    }
}
