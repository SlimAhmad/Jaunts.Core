using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Role;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Processings.Role
{
    public partial class RoleProcessingServiceTests
    {
        [Fact]
        public async Task ShouldRetrievePermissionsAsync()
        {
            // given
            List<string> randomRoles = CreateRandomStringList();
            IQueryable<ApplicationRole> randomRole = CreateRandomRoles(randomRoles);
            IQueryable<ApplicationRole> storageRole = randomRole;
            IQueryable<ApplicationRole> expectedRole = storageRole;

            this.roleServiceMock.Setup(service =>
                service.RetrieveAllRoles())
                    .Returns(storageRole);

            // when
            int actualRole =
                  await this.roleProcessingService.RetrievePermissions(randomRoles);

            // then
            actualRole.Should().BeGreaterThanOrEqualTo(0);

            this.roleServiceMock.Verify(service =>
                service.RetrieveAllRoles(),
                    Times.Once);

            this.roleServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
