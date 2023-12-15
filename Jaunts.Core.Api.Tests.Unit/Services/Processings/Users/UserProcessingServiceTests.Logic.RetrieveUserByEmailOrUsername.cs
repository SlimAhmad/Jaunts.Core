using FluentAssertions;
using Force.DeepCloner;
using Jaunts.Core.Api.Models.Auth;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Moq;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Processings.Users
{
    public partial class UserProcessingServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveUserByEmailOrUsernameAsync()
        {
            // given
            ApplicationUser randomUser = CreateRandomUser();
            ApplicationUser inputUser = randomUser;
            ApplicationUser addedUser = inputUser;
            ApplicationUser expectedUser = addedUser.DeepClone();

            IQueryable<ApplicationUser> randomUsers =
                CreateRandomUsers(inputUser);

            IQueryable<ApplicationUser> retrievedUsers =
                randomUsers;

            this.userServiceMock.Setup(service =>
                service.RetrieveAllUsers())
                    .Returns(retrievedUsers);

            // when
            ApplicationUser actualUser = await this.userProcessingService
                .RetrieveUserByEmailOrUserNameAsync(inputUser.Email);

            // then
            actualUser.Should().BeEquivalentTo(expectedUser);

            this.userServiceMock.Verify(service =>
                service.RetrieveAllUsers(),
                    Times.Once);

            this.userServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

    }
}
