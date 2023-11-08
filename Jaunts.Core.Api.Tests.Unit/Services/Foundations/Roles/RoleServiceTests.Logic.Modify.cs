// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Force.DeepCloner;
using Jaunts.Core.Api.Models.Services.Foundations.Role;
using Moq;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Roles
{
    public partial class RoleServiceTests
    {
        [Fact]
        public async Task ShouldModifyRoleAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            int randomDays = randomNumber;
            DateTimeOffset randomDate = GetRandomDateTime();
            DateTimeOffset randomInputDate = GetRandomDateTime();
            ApplicationRole randomRole = CreateRandomRole(dates: randomInputDate);
            ApplicationRole inputRole = randomRole;
            ApplicationRole afterUpdateStorageRole = inputRole;
            ApplicationRole expectedRole = afterUpdateStorageRole;
            ApplicationRole beforeUpdateStorageRole = randomRole.DeepClone();
            inputRole.UpdatedDate = randomDate;
            Guid RoleId = inputRole.Id;

            this.roleManagementBrokerMock.Setup(broker =>
                broker.SelectRoleByIdAsync(RoleId))
                    .ReturnsAsync(beforeUpdateStorageRole);

            this.roleManagementBrokerMock.Setup(broker =>
                broker.UpdateRoleAsync(inputRole))
                    .ReturnsAsync(afterUpdateStorageRole);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            // when
            ApplicationRole actualRole =
                await this.roleService.ModifyRoleRequestAsync(inputRole);

            // then
            actualRole.Should().BeEquivalentTo(expectedRole);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.roleManagementBrokerMock.Verify(broker =>
                broker.SelectRoleByIdAsync(RoleId),
                    Times.Once);

            this.roleManagementBrokerMock.Verify(broker =>
                broker.UpdateRoleAsync(inputRole),
                    Times.Once);

            this.roleManagementBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
