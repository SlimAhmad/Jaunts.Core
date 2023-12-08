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

namespace Jaunts.Core.Api.Tests.Unit.Services.Processings.Role
{
    public partial class RoleProcessingServiceTests
    {
        [Fact]
        public async Task ShouldAddRoleIfNotExistAsync()
        {
            // given
            IQueryable<ApplicationRole> randomRoles = CreateRandomRoles();

            IQueryable<ApplicationRole> retrievedRoles =
            randomRoles;

            ApplicationRole randomRole = CreateRandomRole();
            ApplicationRole inputRole = randomRole;
            ApplicationRole addedRole = inputRole;
            ApplicationRole expectedRole = addedRole.DeepClone();

            this.roleServiceMock.Setup(service =>
                service.RetrieveAllRoles())
                    .Returns(retrievedRoles);

            this.roleServiceMock.Setup(service =>
                service.AddRoleRequestAsync(inputRole))
                    .ReturnsAsync(addedRole);

            // when
            ApplicationRole actualRole = await this.roleProcessingService
                .UpsertRoleAsync(inputRole);

            // then
            actualRole.Should().BeEquivalentTo(expectedRole);

            this.roleServiceMock.Verify(service =>
                service.RetrieveAllRoles(),
                    Times.Once);

            this.roleServiceMock.Verify(service =>
                service.AddRoleRequestAsync(inputRole),
                    Times.Once);

            this.roleServiceMock.Verify(service =>
                service.ModifyRoleRequestAsync(It.IsAny<ApplicationRole>()),
                    Times.Never);

            this.roleServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldModifyRoleIfRoleExistAsync()
        {
            // given
            ApplicationRole randomRole = CreateRandomRole();
            ApplicationRole inputRole = randomRole;
            ApplicationRole modifiedRole = inputRole;
            ApplicationRole expectedRole = modifiedRole.DeepClone();

            IQueryable<ApplicationRole> randomRoles =
                CreateRandomRoles(inputRole);

            IQueryable<ApplicationRole> retrievedRoles =
                randomRoles;

            this.roleServiceMock.Setup(service =>
                service.RetrieveAllRoles())
                    .Returns(retrievedRoles);

            this.roleServiceMock.Setup(service =>
                service.ModifyRoleRequestAsync(inputRole))
                    .ReturnsAsync(modifiedRole);

            // when
            ApplicationRole actualRole = await this.roleProcessingService
                .UpsertRoleAsync(inputRole);

            // then
            actualRole.Should().BeEquivalentTo(expectedRole);

            this.roleServiceMock.Verify(service =>
                service.RetrieveAllRoles(),
                    Times.Once);

            this.roleServiceMock.Verify(service =>
                service.ModifyRoleRequestAsync(inputRole),
                    Times.Once);

            this.roleServiceMock.Verify(service =>
                service.AddRoleRequestAsync(It.IsAny<ApplicationRole>()),
                    Times.Never);

            this.roleServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

    }
}
