using FluentAssertions;
using Force.DeepCloner;
using Jaunts.Core.Api.Models.Services.Foundations.Role;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Orchestrations.Role
{
    public partial class RoleOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldAddRoleAsync()
        {
            // given
            ApplicationRole randomRole = CreateRandomRole();
            ApplicationRole inputRole = randomRole;
            ApplicationRole addedRole = inputRole;
            ApplicationRole expectedRole = addedRole.DeepClone();

            this.roleProcessingServiceMock.Setup(service =>
                service.UpsertRoleAsync(inputRole))
                    .ReturnsAsync(expectedRole);

            // when
            ApplicationRole actualRole = await this.roleOrchestrationService
                .UpsertRoleAsync(inputRole);

            // then
            actualRole.Should().BeEquivalentTo(expectedRole);

            this.roleProcessingServiceMock.Verify(service =>
                service.UpsertRoleAsync(inputRole),
                    Times.Once);

            this.roleProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

    }
}
