using FluentAssertions;
using Force.DeepCloner;
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
        public async Task ShouldRetrievePasswordResetTokenAsync()
        {
            // given

            string randomToken = GetRandomString();
            string inputToken = randomToken;
            string expectedToken = inputToken.DeepClone();
            ApplicationUser randomUser = CreateRandomUser();
            ApplicationUser inputUser = randomUser;
            ApplicationUser addedUser = inputUser;
            ApplicationUser expectedUser = addedUser.DeepClone();

            this.userProcessingServiceMock.Setup(service =>
                service.PasswordResetTokenAsync(inputUser))
                    .ReturnsAsync(expectedToken);

            // when
            string actualToken = await this.userOrchestrationService
                .PasswordResetTokenAsync(inputUser);

            // then
            actualToken.Should().BeEquivalentTo(expectedToken);

            this.userProcessingServiceMock.Verify(service =>
                service.PasswordResetTokenAsync(inputUser),
                    Times.Once);

            this.userProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
