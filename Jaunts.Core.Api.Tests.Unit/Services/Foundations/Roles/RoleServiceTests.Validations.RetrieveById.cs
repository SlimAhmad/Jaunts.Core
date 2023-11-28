// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Role.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Role;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Roles
{
    public partial class RoleServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdWhenRoleIdIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidRoleId = Guid.Empty;

            var invalidRoleException =
              new InvalidRoleException(
                  message: "Invalid Role. Please correct the errors and try again.");

            invalidRoleException.AddData(
                key: nameof(ApplicationRole.Id),
                values: "Id is required");

            var expectedRoleValidationException =
                new RoleValidationException(
                    message: "Role validation errors occurred, please try again.",
                    innerException: invalidRoleException);

            // when
            ValueTask<ApplicationRole> retrieveRoleTask =
                this.roleService.RetrieveRoleByIdRequestAsync(invalidRoleId);

            RoleValidationException actualRoleValidationException =
                 await Assert.ThrowsAsync<RoleValidationException>(
                     retrieveRoleTask.AsTask);

            // then
            actualRoleValidationException.Should().BeEquivalentTo(
                expectedRoleValidationException);

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
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdWhenStorageRoleIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidRoleId = Guid.NewGuid();
            ApplicationRole invalidStorageRole = null;
            var notFoundRoleException = new NotFoundRoleException(invalidRoleId);

            var expectedRoleValidationException =
                new RoleValidationException(
                    message: "Role validation errors occurred, please try again.",
                    innerException: notFoundRoleException);

            this.roleManagementBrokerMock.Setup(broker =>
                broker.SelectRoleByIdAsync(invalidRoleId))
                    .ReturnsAsync(invalidStorageRole);

            // when
            ValueTask<ApplicationRole> retrieveRoleTask =
                this.roleService.RetrieveRoleByIdRequestAsync(invalidRoleId);

            RoleValidationException actualRoleValidationException =
                 await Assert.ThrowsAsync<RoleValidationException>(
                     retrieveRoleTask.AsTask);

            // then
            actualRoleValidationException.Should().BeEquivalentTo(
                expectedRoleValidationException);

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
