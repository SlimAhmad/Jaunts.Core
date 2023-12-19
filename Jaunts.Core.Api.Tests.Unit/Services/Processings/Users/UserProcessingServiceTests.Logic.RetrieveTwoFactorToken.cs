using FluentAssertions;
using Force.DeepCloner;
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
        public async Task ShouldGenerateTwoFactorTokenAsync()
        {
            // given

            string randomToken = GetRandomString();
            string inputToken = randomToken;
            string expectedToken = inputToken.DeepClone();
            ApplicationUser randomUser = CreateRandomUser();
            ApplicationUser inputUser = randomUser;
            ApplicationUser addedUser = inputUser;
            ApplicationUser expectedUser = addedUser.DeepClone();

            this.userServiceMock.Setup(service =>
                service.RetrieveUserTwoFactorTokenAsync(inputUser))
                    .ReturnsAsync(expectedToken);

            // when
            string actualToken = await this.userProcessingService
                .RetrieveTwoFactorTokenAsync(inputUser);

            // then
            actualToken.Should().BeEquivalentTo(expectedToken);

            this.userServiceMock.Verify(service =>
                service.RetrieveUserTwoFactorTokenAsync(inputUser),
                    Times.Once);

            this.userServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
