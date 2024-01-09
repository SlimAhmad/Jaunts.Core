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
        public async Task ShouldRemoveByIdUserAsync()
        {
            // given
            Guid randomUserId = GetRandomGuid();
            ApplicationUser randomUser = CreateRandomUser();
            ApplicationUser storageUser = randomUser;
            ApplicationUser expectedUser = storageUser;

            bool randomBoolean = GetRandomBoolean();
            bool inputBoolean = randomBoolean;
            bool expectedBoolean = inputBoolean;

            this.userProcessingServiceMock.Setup(service =>
                service.RemoveUserByIdAsync(randomUserId))
                    .ReturnsAsync(expectedBoolean);

            // when
            bool actualUser =
                  await this.userOrchestrationService.RemoveUserByIdAsync(randomUserId);

            // then
            actualUser.Should().Be(expectedBoolean);

            this.userProcessingServiceMock.Verify(service =>
                service.RemoveUserByIdAsync(randomUserId),
                    Times.Once);

            this.userProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
