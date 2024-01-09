using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Role;
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
        public async Task ShouldRetrieveByIdRoleAsync()
        {
            // given
            Guid randomRoleId = GetRandomGuid();
            ApplicationRole randomRole = CreateRandomRole();
            ApplicationRole storageRole = randomRole;
            ApplicationRole expectedRole = storageRole;

            this.roleProcessingServiceMock.Setup(service =>
                service.RetrieveRoleById(randomRoleId))
                    .ReturnsAsync(storageRole);

            // when
            ApplicationRole actualRole =
                  await this.roleOrchestrationService.RetrieveRoleById(randomRoleId);

            // then
            actualRole.Should().BeEquivalentTo(expectedRole);

            this.roleProcessingServiceMock.Verify(service =>
                service.RetrieveRoleById(randomRoleId),
                    Times.Once);

            this.roleProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
