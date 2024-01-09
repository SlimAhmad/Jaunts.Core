using FluentAssertions;
using Force.DeepCloner;
using Jaunts.Core.Api.Models.Auth;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Orchestrations.Users
{
    public partial class UserOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldEnableOrDisableTwoFAuthenticationAsync()
        {
            // given
            ApplicationUser randomUser = CreateRandomUser();
            ApplicationUser inputUser = randomUser;
            ApplicationUser addedUser = inputUser;
            ApplicationUser expectedUser = addedUser.DeepClone();
            Guid randomGuid = GetRandomGuid();
            Guid inputId = randomGuid;


            this.userProcessingServiceMock.Setup(service =>
                service.ModifyTwoFactorAsync(inputUser.Id))
                    .ReturnsAsync(expectedUser);

            // when
            ApplicationUser actualUser = await this.userOrchestrationService
                .EnableOrDisableTwoFactorAsync(inputUser.Id);

            // then
            actualUser.Should().NotBeNull();

            this.userProcessingServiceMock.Verify(service =>
                service.ModifyTwoFactorAsync(inputUser.Id),
                    Times.Once);

            this.userProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

    }
}
