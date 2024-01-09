using FluentAssertions;
using Force.DeepCloner;
using Jaunts.Core.Api.Models.Auth;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Moq;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Orchestrations.Users
{
    public partial class UserOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveUserByEmailOrUsernameAsync()
        {
            // given
            ApplicationUser randomUser = CreateRandomUser();
            ApplicationUser inputUser = randomUser;
            ApplicationUser addedUser = inputUser;
            ApplicationUser expectedUser = addedUser.DeepClone();


            this.userProcessingServiceMock.Setup(service =>
                service.RetrieveUserByEmailOrUserNameAsync(inputUser.Email))
                    .ReturnsAsync(expectedUser);

            // when
            ApplicationUser actualUser = await this.userOrchestrationService
                .RetrieveUserByEmailOrUserNameAsync(inputUser.Email);

            // then
            actualUser.Should().BeEquivalentTo(expectedUser);

            this.userProcessingServiceMock.Verify(service =>
                service.RetrieveUserByEmailOrUserNameAsync(inputUser.Email),
                    Times.Once);

            this.userProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

    }
}
