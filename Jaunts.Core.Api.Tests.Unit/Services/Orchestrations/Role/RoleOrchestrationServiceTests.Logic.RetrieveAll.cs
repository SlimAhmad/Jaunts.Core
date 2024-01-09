using FluentAssertions;
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
        public void ShouldRetrieveAllRoles()
        {
            // given
            IQueryable<ApplicationRole> randomRoles = CreateRandomRoles();
            IQueryable<ApplicationRole> storageRoles = randomRoles;
            IQueryable<ApplicationRole> expectedRoles = storageRoles;

            this.roleProcessingServiceMock.Setup(service =>
                service.RetrieveAllRoles())
                    .Returns(storageRoles);

            // when
            IQueryable<ApplicationRole> actualRoles =
                this.roleOrchestrationService.RetrieveAllRoles();

            // then
            actualRoles.Should().BeEquivalentTo(expectedRoles);

            this.roleProcessingServiceMock.Verify(service =>
                service.RetrieveAllRoles(),
                    Times.Once);

            this.roleProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
