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
        public async Task ShouldCheckPasswordValidityAsync()
        {
            // given
            ApplicationUser randomUser = CreateRandomUser();
            ApplicationUser inputUser = randomUser;
            ApplicationUser addedUser = inputUser;
            ApplicationUser expectedUser = addedUser.DeepClone();
            string randomPassword = GetRandomString();
            string inputPassword = randomPassword;
            bool randomBoolean = GetRandomBoolean();
            bool inputBoolean = randomBoolean;
            bool expectedBoolean = inputBoolean;

            this.userProcessingServiceMock.Setup(service =>
                service.ValidatePasswordAsync(inputPassword,inputUser.Id))
                    .ReturnsAsync(expectedBoolean);

            // when
            bool actualUser = await this.userOrchestrationService
                .CheckPasswordValidityAsync(inputPassword,inputUser.Id);

            // then
            actualUser.Should().Be(expectedBoolean);

            this.userProcessingServiceMock.Verify(service =>
                service.ValidatePasswordAsync(inputPassword,inputUser.Id),
                    Times.Once);

            this.userProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }


    }
}
