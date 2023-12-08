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
        public async Task ShouldRetrieveByIdRoleAsync()
        {
            // given
            Guid randomRoleId = GetRandomGuid();
            ApplicationRole randomRole = CreateRandomRole();
            ApplicationRole storageRole = randomRole;
            ApplicationRole expectedRole = storageRole;

            this.roleServiceMock.Setup(service =>
                service.RetrieveRoleByIdRequestAsync(randomRoleId))
                    .ReturnsAsync(storageRole);

            // when
            ApplicationRole actualRole =
                  await this.roleProcessingService.RetrieveRoleById(randomRoleId);

            // then
            actualRole.Should().BeEquivalentTo(expectedRole);

            this.roleServiceMock.Verify(service =>
                service.RetrieveRoleByIdRequestAsync(randomRoleId),
                    Times.Once);

            this.roleServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
