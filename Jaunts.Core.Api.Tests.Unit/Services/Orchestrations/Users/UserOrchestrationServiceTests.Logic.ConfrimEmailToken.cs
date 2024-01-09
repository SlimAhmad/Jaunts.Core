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
        public async Task ShouldConfirmEmailAsync()
        {
            // given
            ApplicationUser randomUser = CreateRandomUser();
            ApplicationUser inputUser = randomUser;
            ApplicationUser addedUser = inputUser;
            ApplicationUser expectedUser = addedUser.DeepClone();
            string randomToken = GetRandomString();
            string inputToken = randomToken;

            this.userProcessingServiceMock.Setup(service =>
                service.ConfirmEmailAsync(inputToken,inputUser.Email))
                    .ReturnsAsync(expectedUser);

            // when
            ApplicationUser actualUser = await this.userOrchestrationService
                .ConfirmEmailAsync(inputToken,inputUser.Email);

            // then
            actualUser.Should().BeEquivalentTo(expectedUser);


            this.userProcessingServiceMock.Verify(service =>
                service.ConfirmEmailAsync(inputToken,inputUser.Email),
                    Times.Once);

            this.userProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }


    }
}
