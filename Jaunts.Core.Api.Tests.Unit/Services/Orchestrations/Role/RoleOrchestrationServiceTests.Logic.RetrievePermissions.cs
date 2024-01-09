using FluentAssertions;
using Force.DeepCloner;
using Jaunts.Core.Api.Models.Services.Foundations.Role;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Orchestrations.Role
{
    public partial class RoleOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldRetrievePermissionsAsync()
        {
            // given
            List<string> randomRoles = CreateRandomStringList();
            int randomNumber = GetRandomNumber();
            int inputNumber = randomNumber;
            int expectedNumber = inputNumber;
            ApplicationUser randomUser = CreateRandomUser();
            ApplicationUser inputUser = randomUser;
            
            this.userProcessingServiceMock.Setup(service =>
                service.RetrieveUserRolesAsync(inputUser))
                    .ReturnsAsync(randomRoles);

            this.roleProcessingServiceMock.Setup(service =>
                service.RetrievePermissions(randomRoles))
                    .ReturnsAsync(inputNumber);


            // when
            int actualRole =
                  await this.roleOrchestrationService.RetrievePermissions(inputUser);

            // then
            actualRole.Should().BeGreaterThanOrEqualTo(expectedNumber);

            this.userProcessingServiceMock.Verify(service =>
                service.RetrieveUserRolesAsync(inputUser),
                    Times.Once);

            this.roleProcessingServiceMock.Verify(service =>
                service.RetrievePermissions(randomRoles),
                    Times.Once);

            this.roleProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
