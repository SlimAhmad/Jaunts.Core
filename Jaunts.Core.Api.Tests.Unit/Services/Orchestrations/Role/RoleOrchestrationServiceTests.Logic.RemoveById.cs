using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Role;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Orchestrations.Role
{
    public partial class RoleOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldRemoveByIdRoleAsync()
        {
            // given
            Guid randomRoleId = GetRandomGuid();
            bool randomBoolean = true;
            bool expectedBoolean = randomBoolean;
            
          
            this.roleProcessingServiceMock.Setup(service =>
                service.RemoveRoleByIdAsync(randomRoleId))
                    .ReturnsAsync(expectedBoolean);

            // when
            bool actualRole =
                  await this.roleOrchestrationService.RemoveRoleByIdAsync(randomRoleId);

            // then
            actualRole.Should().BeTrue();

            this.roleProcessingServiceMock.Verify(service =>
                service.RemoveRoleByIdAsync(randomRoleId),
                    Times.Once);

            this.roleProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
