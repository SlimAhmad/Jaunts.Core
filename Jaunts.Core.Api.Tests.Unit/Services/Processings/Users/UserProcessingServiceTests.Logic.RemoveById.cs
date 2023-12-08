using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Processings.Users
{
    public partial class UserProcessingServiceTests
    {
        [Fact]
        public async Task ShouldRemoveByIdUserAsync()
        {
            // given
            Guid randomUserId = GetRandomGuid();
            ApplicationUser randomUser = CreateRandomUser();
            ApplicationUser storageUser = randomUser;
            ApplicationUser expectedUser = storageUser;

            this.userServiceMock.Setup(service =>
                service.RemoveUserByIdRequestAsync(randomUserId))
                    .ReturnsAsync(storageUser);

            // when
            bool actualUser =
                  await this.userProcessingService.RemoveUserByIdAsync(randomUserId);

            // then
            actualUser.Should().BeTrue();

            this.userServiceMock.Verify(service =>
                service.RemoveUserByIdRequestAsync(randomUserId),
                    Times.Once);

            this.userServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
