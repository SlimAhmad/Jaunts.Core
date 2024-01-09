using FluentAssertions;
using Force.DeepCloner;
using Jaunts.Core.Api.Models.Auth;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Orchestrations.Users
{
    public partial class UserOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveUserRolesAsync()
        {
            // given
            ApplicationUser randomUser = CreateRandomUser();
            ApplicationUser inputUser = randomUser;
            ApplicationUser addedUser = inputUser;
            ApplicationUser expectedUser = addedUser.DeepClone();
            List<string> randomRole = CreateRandomStringList();
            List<string> inputRole = randomRole;
            List<string> expectedRole = inputRole;

            this.userProcessingServiceMock.Setup(service =>
                service.RetrieveUserRolesAsync(inputUser))
                    .ReturnsAsync(expectedRole);

            // when
            List<string> actualUser = await this.userOrchestrationService
                .RetrieveUserRolesAsync(inputUser);

            // then
            actualUser.Should().BeEquivalentTo(expectedRole);

            this.userProcessingServiceMock.Verify(service =>
                service.RetrieveUserRolesAsync(inputUser),
                    Times.Once);

            this.userProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }


    }
}
