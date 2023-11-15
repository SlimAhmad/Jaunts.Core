// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Jaunts.Core.Api.Models.Role.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Role;
using Moq;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Roles
{
    public partial class RoleServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnDeleteWhenRoleIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomRoleId = default;
            Guid inputRoleId = randomRoleId;

            var invalidRoleException = new InvalidRoleException();

            invalidRoleException.AddData(
                key: nameof(ApplicationRole.Id),
                values: "Id is required");


            var expectedRoleValidationException =
                new RoleValidationException(invalidRoleException);

            // when
            ValueTask<ApplicationRole> actualRoleTask =
                this.roleService.RemoveRoleByIdRequestAsync(inputRoleId);

            // then
            await Assert.ThrowsAsync<RoleValidationException>(() => actualRoleTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRoleValidationException))),
                        Times.Once);

            this.roleManagementBrokerMock.Verify(broker =>
                broker.SelectRoleByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.roleManagementBrokerMock.Verify(broker =>
                broker.DeleteRoleAsync(It.IsAny<ApplicationRole>()),
                    Times.Never);

            this.roleManagementBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnDeleteWhenStorageRoleIsInvalidAndLogItAsync()
        {
            // given
            Guid randomRoleId = Guid.NewGuid();
            Guid inputRoleId = randomRoleId;
            ApplicationRole invalidStorageRole = null;
            var notFoundRoleException = new NotFoundRoleException(inputRoleId);

            var expectedRoleValidationException =
                new RoleValidationException(notFoundRoleException);

            this.roleManagementBrokerMock.Setup(broker =>
                broker.SelectRoleByIdAsync(inputRoleId))
                    .ReturnsAsync(invalidStorageRole);

            // when
            ValueTask<ApplicationRole> deleteRoleTask =
                this.roleService.RemoveRoleByIdRequestAsync(inputRoleId);

            // then
            await Assert.ThrowsAsync<RoleValidationException>(() =>
                deleteRoleTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRoleValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.roleManagementBrokerMock.Verify(broker =>
                broker.SelectRoleByIdAsync(inputRoleId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.roleManagementBrokerMock.VerifyNoOtherCalls();
        }
    }
}
