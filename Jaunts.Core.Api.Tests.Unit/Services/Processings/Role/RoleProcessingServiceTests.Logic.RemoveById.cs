using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Role;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Processings.Role
{
    public partial class RoleProcessingServiceTests
    {
        [Fact]
        public async Task ShouldRemoveByIdRoleAsync()
        {
            // given
            Guid randomRoleId = GetRandomGuid();
            ApplicationRole randomRole = CreateRandomRole();
            ApplicationRole storageRole = randomRole;
            ApplicationRole expectedRole = storageRole;

            this.roleServiceMock.Setup(service =>
                service.RemoveRoleByIdRequestAsync(randomRoleId))
                    .ReturnsAsync(storageRole);

            // when
            bool actualRole =
                  await this.roleProcessingService.RemoveRoleByIdAsync(randomRoleId);

            // then
            actualRole.Should().BeTrue();

            this.roleServiceMock.Verify(service =>
                service.RemoveRoleByIdRequestAsync(randomRoleId),
                    Times.Once);

            this.roleServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
