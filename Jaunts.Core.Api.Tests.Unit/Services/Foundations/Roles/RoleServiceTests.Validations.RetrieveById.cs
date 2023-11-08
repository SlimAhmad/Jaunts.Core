// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Role.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Role;
using Moq;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Roles
{
    public partial class RoleServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidatonExceptionOnRetrieveByIdWhenRoleIdIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidRoleId = Guid.Empty;

            var invalidRoleException = new InvalidRoleException();

            invalidRoleException.AddData(
                key: nameof(ApplicationRole.Id),
                values: "Id is required");

            var expectedRoleValidationException =
                new RoleValidationException(invalidRoleException);

            // when
            ValueTask<ApplicationRole> actualRoleTask =
                this.roleService.RetrieveRoleByIdRequestAsync(invalidRoleId);

            // then
            await Assert.ThrowsAsync<RoleValidationException>(() => actualRoleTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRoleValidationException))),
                        Times.Once);

            this.roleManagementBrokerMock.Verify(broker =>
                broker.SelectRoleByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.roleManagementBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidatonExceptionOnRetrieveByIdWhenStorageRoleIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidRoleId = Guid.NewGuid();
            ApplicationRole invalidStorageRole = null;
            var notFoundRoleException = new NotFoundRoleException(invalidRoleId);

            var expectedRoleValidationException =
                new RoleValidationException(notFoundRoleException);

            this.roleManagementBrokerMock.Setup(broker =>
                broker.SelectRoleByIdAsync(invalidRoleId))
                    .ReturnsAsync(invalidStorageRole);

            // when
            ValueTask<ApplicationRole> retrieveRoleTask =
                this.roleService.RetrieveRoleByIdRequestAsync(invalidRoleId);

            // then
            await Assert.ThrowsAsync<RoleValidationException>(() =>
                retrieveRoleTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRoleValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.roleManagementBrokerMock.Verify(broker =>
                broker.SelectRoleByIdAsync(invalidRoleId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.roleManagementBrokerMock.VerifyNoOtherCalls();
        }
    }
}
