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

namespace Jaunts.Core.Api.Tests.Unit.Services.Processings.Role
{
    public partial class RoleProcessingServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllRoles()
        {
            // given
            IQueryable<ApplicationRole> randomRoles = CreateRandomRoles();
            IQueryable<ApplicationRole> storageRoles = randomRoles;
            IQueryable<ApplicationRole> expectedRoles = storageRoles;

            this.roleServiceMock.Setup(service =>
                service.RetrieveAllRoles())
                    .Returns(storageRoles);

            // when
            IQueryable<ApplicationRole> actualRoles =
                this.roleProcessingService.RetrieveAllRoles();

            // then
            actualRoles.Should().BeEquivalentTo(expectedRoles);

            this.roleServiceMock.Verify(service =>
                service.RetrieveAllRoles(),
                    Times.Once);

            this.roleServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
