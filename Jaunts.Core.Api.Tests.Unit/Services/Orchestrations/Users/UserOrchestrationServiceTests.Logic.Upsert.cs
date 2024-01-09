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
        public async Task ShouldAddOrUpdateUserIfNotExistAsync()
        {
            // given
            string randomPassword = GetRandomPassword();
            string inputPassword = randomPassword;
            ApplicationUser randomUser = CreateRandomUser();
            ApplicationUser inputUser = randomUser;
            ApplicationUser addedUser = inputUser;
            ApplicationUser expectedUser = addedUser.DeepClone();

            this.userProcessingServiceMock.Setup(service =>
                service.UpsertUserAsync(inputUser,inputPassword))
                    .ReturnsAsync(expectedUser);

            // when
            ApplicationUser actualUser = await this.userOrchestrationService
                .UpsertUserAsync(inputUser,inputPassword);

            // then
            actualUser.Should().BeEquivalentTo(expectedUser);

            this.userProcessingServiceMock.Verify(service =>
                service.UpsertUserAsync(inputUser,inputPassword),
                    Times.Once);

            this.userProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
