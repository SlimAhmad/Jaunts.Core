// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Jaunts.Core.Api.Models.Role.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Role;
using Jaunts.Core.Api.Models.Role.Exceptions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Roles
{
    public partial class RoleServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenRoleIsNullAndLogItAsync()
        {
            // given
            ApplicationRole invalidRole = null;

            var nullRoleException = new NullRoleException();
            string password = GetRandomPassword();

            var expectedRoleValidationException =
                new RoleValidationException(
                    message: "Role validation errors occurred, please try again.",
                    innerException: nullRoleException);

            // when
            ValueTask<ApplicationRole> CreateRoleTask =
                this.roleService.AddRoleRequestAsync(invalidRole);

            RoleValidationException actualRoleValidationException =
              await Assert.ThrowsAsync<RoleValidationException>(
                  CreateRoleTask.AsTask);

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

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.roleManagementBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnCreateWhenRoleRoleNameIsInvalidAndLogItAsync(
            string invalidRoleRoleName)
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            ApplicationRole randomRole = CreateRandomRole(dateTime);
            ApplicationRole invalidRole = randomRole;
            invalidRole.Name = invalidRoleRoleName;

            var invalidRoleException =
              new InvalidRoleException(
                  message: "Invalid Role. Please correct the errors and try again.");

            invalidRoleException.AddData(
               key: nameof(ApplicationRole.Name),
               values: "Text is required");

            var expectedRoleValidationException =
                new RoleValidationException(
                    message: "Role validation errors occurred, please try again.",
                    innerException: invalidRoleException);

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Returns(dateTime);

            // when
            ValueTask<ApplicationRole> CreateRoleTask =
                this.roleService.AddRoleRequestAsync(invalidRole);

            RoleValidationException actualRoleValidationException =
              await Assert.ThrowsAsync<RoleValidationException>(
                  CreateRoleTask.AsTask);

            // then
            actualRoleValidationException.Should().BeEquivalentTo(
                expectedRoleValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRoleValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
               broker.GetCurrentDateTime(),
                   Times.Once());

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.roleManagementBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenCreatedDateIsInvalidAndLogItAsync()
        {
            // given
            ApplicationRole randomRole = CreateRandomRole();
            ApplicationRole inputRole = randomRole;
            inputRole.CreatedDate = default;
            string password = GetRandomPassword();

            var invalidRoleException =
                new InvalidRoleException(
                    message: "Invalid Role. Please correct the errors and try again.");

            invalidRoleException.AddData(
               key: nameof(ApplicationRole.CreatedDate),
               values: "Date is required");

            invalidRoleException.AddData(
               key: nameof(ApplicationRole.UpdatedDate),
               values: ["Date is not the same as CreatedDate", "Date is not recent"]);


            var expectedRoleValidationException =
                new RoleValidationException(
                    message: "Role validation errors occurred, please try again.",
                    innerException: invalidRoleException);

            // when
            ValueTask<ApplicationRole> CreateRoleTask =
                this.roleService.AddRoleRequestAsync(inputRole);

            RoleValidationException actualRoleValidationException =
              await Assert.ThrowsAsync<RoleValidationException>(
                  CreateRoleTask.AsTask);

            // then
            actualRoleValidationException.Should().BeEquivalentTo(
                expectedRoleValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
               broker.GetCurrentDateTime(),
                   Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRoleValidationException))),
                        Times.Once);

            this.roleManagementBrokerMock.Verify(broker =>
                broker.SelectRoleByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.roleManagementBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenUpdatedDateIsNotSameToCreatedDateAndLogItAsync()
        {
            // given
            ApplicationRole randomRole = CreateRandomRole();
            ApplicationRole inputRole = randomRole;
            inputRole.UpdatedDate = GetRandomDateTime();
            string password = GetRandomPassword();

            var invalidRoleException =
                new InvalidRoleException(
                    message: "Invalid Role. Please correct the errors and try again.");

            invalidRoleException.AddData(
            key: nameof(ApplicationRole.UpdatedDate),
                values: [$"Date is not the same as {nameof(ApplicationRole.CreatedDate)}", "Date is not recent"]);

            var expectedRoleValidationException =
               new RoleValidationException(
                   message: "Role validation errors occurred, please try again.",
                   innerException: invalidRoleException);

            // when
            ValueTask<ApplicationRole> CreateRoleTask =
                this.roleService.AddRoleRequestAsync(inputRole);

            RoleValidationException actualRoleValidationException =
              await Assert.ThrowsAsync<RoleValidationException>(
                  CreateRoleTask.AsTask);

            // then
            actualRoleValidationException.Should().BeEquivalentTo(
                expectedRoleValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
               broker.GetCurrentDateTime(),
                   Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRoleValidationException))),
                        Times.Once);

            this.roleManagementBrokerMock.Verify(broker =>
                broker.SelectRoleByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.roleManagementBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidMinuteCases))]
        public async void ShouldThrowValidationExceptionOnCreateWhenCreatedDateIsNotRecentAndLogItAsync(
            int minutes)
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            ApplicationRole randomRole = CreateRandomRole(dates: dateTime);
            ApplicationRole inputRole = randomRole;
            inputRole.CreatedDate = dateTime.AddMinutes(minutes);
            inputRole.UpdatedDate = inputRole.CreatedDate;
            string password = GetRandomPassword();

            var invalidRoleException =
               new InvalidRoleException(
                   message: "Invalid Role. Please correct the errors and try again.");

            invalidRoleException.AddData(
            key: nameof(ApplicationRole.UpdatedDate),
                values: "Date is not recent");

            var expectedRoleValidationException =
               new RoleValidationException(
                   message: "Role validation errors occurred, please try again.",
                   innerException: invalidRoleException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<ApplicationRole> CreateRoleTask =
                this.roleService.AddRoleRequestAsync(inputRole);

            RoleValidationException actualRoleValidationException =
              await Assert.ThrowsAsync<RoleValidationException>(
                  CreateRoleTask.AsTask);

            // then
            actualRoleValidationException.Should().BeEquivalentTo(
                expectedRoleValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRoleValidationException))),
                        Times.Once);

            this.roleManagementBrokerMock.Verify(broker =>
                broker.SelectRoleByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.roleManagementBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenRoleAlreadyExistsAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            DateTimeOffset dateTime = randomDateTime;
            ApplicationRole randomRole = CreateRandomRole(dateTime);
            ApplicationRole alreadyExistsRole = randomRole;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var duplicateKeyException = new DuplicateKeyException(exceptionMessage);
            string password = GetRandomPassword();

            var alreadyExistsRoleException =
               new AlreadyExistsRoleException(
                   message: "Role with the same id already exists.",
                   duplicateKeyException);

            var expectedRoleDependencyValidationException =
               new RoleDependencyValidationException(
                   message: "Role dependency validation occurred, please try again.",
                   innerException: alreadyExistsRoleException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.roleManagementBrokerMock.Setup(broker =>
                broker.InsertRoleAsync(alreadyExistsRole))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<ApplicationRole> CreateRoleTask =
                this.roleService.AddRoleRequestAsync(alreadyExistsRole);

            // then
            await Assert.ThrowsAsync<RoleDependencyValidationException>(() =>
                CreateRoleTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.roleManagementBrokerMock.Verify(broker =>
                broker.InsertRoleAsync(alreadyExistsRole),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedRoleDependencyValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.roleManagementBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
