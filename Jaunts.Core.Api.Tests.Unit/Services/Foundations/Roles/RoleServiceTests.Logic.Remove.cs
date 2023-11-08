// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Role;
using Moq;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Roles
{
    public partial class RoleServiceTests
    {
        [Fact]
        public async Task ShouldDeleteRoleAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            ApplicationRole randomRole = CreateRandomRole(dates: dateTime);
            Guid inputRoleId = randomRole.Id;
            ApplicationRole inputRole = randomRole;
            ApplicationRole storageRole = randomRole;
            ApplicationRole expectedRole = randomRole;

            this.roleManagementBrokerMock.Setup(broker =>
                broker.SelectRoleByIdAsync(inputRoleId))
                    .ReturnsAsync(inputRole);

            this.roleManagementBrokerMock.Setup(broker =>
                broker.DeleteRoleAsync(inputRole))
                    .ReturnsAsync(storageRole);

            // when
            ApplicationRole actualRole =
                await this.roleService.RemoveRoleByIdRequestAsync(inputRoleId);

            // then
            actualRole.Should().BeEquivalentTo(expectedRole);

            this.roleManagementBrokerMock.Verify(broker =>
                broker.SelectRoleByIdAsync(inputRoleId),
                    Times.Once);

            this.roleManagementBrokerMock.Verify(broker =>
                broker.DeleteRoleAsync(inputRole),
                    Times.Once);

            this.roleManagementBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
