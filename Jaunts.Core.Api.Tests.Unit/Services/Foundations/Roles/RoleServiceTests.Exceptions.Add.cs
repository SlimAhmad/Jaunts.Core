// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Jaunts.Core.Api.Models.Role.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Role;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Roles
{
    public partial class RoleServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnCreateWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            ApplicationRole randomRole = CreateRandomRole(dates: dateTime);
            ApplicationRole inputRole = randomRole;
            var sqlException = GetSqlException();
            string password = GetRandomPassword();

            var failedRoleStorageException =
              new FailedRoleStorageException(
                  message: "Failed Role storage error occurred, contact support.",
                  innerException: sqlException);

            var expectedRoleDependencyException =
                new RoleDependencyException(
                    message: "Role dependency error occurred, contact support.",
                    innerException: failedRoleStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.roleManagementBrokerMock.Setup(broker =>
                broker.InsertRoleAsync(It.IsAny<ApplicationRole>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<ApplicationRole> registerRoleTask =
                this.roleService.AddRoleRequestAsync(inputRole);

            RoleDependencyException actualRoleDependencyException =
                 await Assert.ThrowsAsync<RoleDependencyException>(
                     registerRoleTask.AsTask);

            // then
            actualRoleDependencyException.Should().BeEquivalentTo(
                expectedRoleDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedRoleDependencyException))),
                        Times.Once);

            this.roleManagementBrokerMock.Verify(broker =>
                broker.InsertRoleAsync(It.IsAny<ApplicationRole>()),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.roleManagementBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfRoleAlreadyExistsAndLogItAsync()
        {
            // given
            ApplicationRole randomRole = CreateRandomRole();
            ApplicationRole alreadyExistsRole = randomRole;
            string randomMessage = GetRandomMessage();

            var duplicateKeyException =
                new DuplicateKeyException(randomMessage);

            var failedRoleStorageException =
                new AlreadyExistsRoleException(
                    message: "Role with the same id already exists.",
                    innerException: duplicateKeyException);

            var expectedRoleDependencyValidationException =
                new RoleDependencyValidationException(
                    message: "Role dependency validation occurred, please try again.",
                    innerException: failedRoleStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(duplicateKeyException);

            // when
            ValueTask<ApplicationRole> addRoleTask =
                this.roleService.AddRoleRequestAsync(alreadyExistsRole);

            RoleDependencyValidationException actualRoleDependencyValidationException =
                await Assert.ThrowsAsync<RoleDependencyValidationException>(
                    addRoleTask.AsTask);

            // then
            actualRoleDependencyValidationException.Should().BeEquivalentTo(
                expectedRoleDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.roleManagementBrokerMock.Verify(broker =>
                broker.InsertRoleAsync(It.IsAny<ApplicationRole>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRoleDependencyValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.roleManagementBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnCreateWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            ApplicationRole randomRole = CreateRandomRole(dates: dateTime);
            ApplicationRole inputRole = randomRole;
            var databaseUpdateException = new DbUpdateException();
            string password = GetRandomPassword();

            var failedRoleStorageException =
                 new FailedRoleStorageException(
                     message: "Failed Role storage error occurred, contact support.",
                     innerException: databaseUpdateException);

            var expectedRoleDependencyException =
                new RoleDependencyException(
                    message: "Role dependency error occurred, contact support.",
                    innerException: failedRoleStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.roleManagementBrokerMock.Setup(broker =>
                broker.InsertRoleAsync(It.IsAny<ApplicationRole>()))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<ApplicationRole> registerRoleTask =
                this.roleService.AddRoleRequestAsync(inputRole);

            RoleDependencyException actualRoleDependencyException =
            await Assert.ThrowsAsync<RoleDependencyException>(
                registerRoleTask.AsTask);

            // then
            actualRoleDependencyException.Should().BeEquivalentTo(
                expectedRoleDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRoleDependencyException))),
                        Times.Once);

            this.roleManagementBrokerMock.Verify(broker =>
                broker.InsertRoleAsync(It.IsAny<ApplicationRole>()),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.roleManagementBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnCreateWhenExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            ApplicationRole randomRole = CreateRandomRole(dates: dateTime);
            ApplicationRole inputRole = randomRole;
            var serviceException = new Exception();
            string password = GetRandomPassword();

            var failedRoleServiceException =
                  new FailedRoleServiceException(
                      message: "Failed Role service occurred, please contact support",
                      innerException: serviceException);

            var expectedRoleServiceException =
                new RoleServiceException(
                    message: "Role service error occurred, contact support.",
                    innerException: failedRoleServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.roleManagementBrokerMock.Setup(broker =>
                broker.InsertRoleAsync(It.IsAny<ApplicationRole>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<ApplicationRole> registerRoleTask =
                 this.roleService.AddRoleRequestAsync(inputRole);

            RoleServiceException actualRoleServiceException =
              await Assert.ThrowsAsync<RoleServiceException>(
                  registerRoleTask.AsTask);

            // then
            actualRoleServiceException.Should().BeEquivalentTo(
                expectedRoleServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRoleServiceException))),
                        Times.Once);

            this.roleManagementBrokerMock.Verify(broker =>
                broker.InsertRoleAsync(It.IsAny<ApplicationRole>()),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.roleManagementBrokerMock.VerifyNoOtherCalls();
        }
    }
}
