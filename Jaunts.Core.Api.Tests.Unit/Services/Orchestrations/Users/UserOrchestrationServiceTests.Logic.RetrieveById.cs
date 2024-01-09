using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Orchestrations.Users
{
    public partial class UserOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveByIdUserAsync()
        {
            // given
            ApplicationUser randomUser = CreateRandomUser();
            ApplicationUser inputUser = randomUser;
            ApplicationUser storageUser = inputUser;
            ApplicationUser expectedUser = storageUser;

            this.userProcessingServiceMock.Setup(service =>
                service.RetrieveUserById(inputUser.Id))
                    .ReturnsAsync(storageUser);

            // when
            ApplicationUser actualUser =
                  await this.userOrchestrationService.RetrieveUserByIdAsync(inputUser.Id);

            // then
            actualUser.Should().BeEquivalentTo(expectedUser);

            this.userProcessingServiceMock.Verify(service =>
                service.RetrieveUserById(inputUser.Id),
                    Times.Once);

            this.userProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
