﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Role.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Role;
using Microsoft.Data.SqlClient;
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
        public async Task ShouldThrowDependencyExceptionOnModifyIfSqlExceptionOccursAndLogItAsync()
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            ApplicationRole randomRole = CreateRandomRole(dates: randomDateTime);
            ApplicationRole someRole = randomRole;
            someRole.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            SqlException sqlException = GetSqlException();

            var failedRoleStorageException =
                new FailedRoleStorageException(
                    message: "Failed Role storage error occurred, contact support.",
                    innerException: sqlException);

            var expectedRoleDependencyException =
                new RoleDependencyException(
                    message: "Role dependency error occurred, contact support.",
                    innerException: failedRoleStorageException);

            this.roleManagementBrokerMock.Setup(broker =>
                broker.SelectRoleByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTime);

            // when
            ValueTask<ApplicationRole> modifyRoleTask =
                this.roleService.ModifyRoleRequestAsync(someRole);

            RoleDependencyException actualRoleDependencyException =
             await Assert.ThrowsAsync<RoleDependencyException>(
                 modifyRoleTask.AsTask);

            // then
            actualRoleDependencyException.Should().BeEquivalentTo(
                expectedRoleDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.roleManagementBrokerMock.Verify(broker =>
                broker.SelectRoleByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedRoleDependencyException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.roleManagementBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDbUpdateExceptionOccursAndLogItAsync()
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            ApplicationRole randomRole = CreateRandomRole(randomDateTime);
            ApplicationRole someRole = randomRole;
            someRole.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            var databaseUpdateException = new DbUpdateException();

            var failedRoleStorageException =
                   new FailedRoleStorageException(
                       message: "Failed Role storage error occurred, contact support.",
                       innerException: databaseUpdateException);

            var expectedRoleDependencyException =
                new RoleDependencyException(
                    message: "Role dependency error occurred, contact support.",
                    innerException: failedRoleStorageException);

            this.roleManagementBrokerMock.Setup(broker =>
                broker.SelectRoleByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTime);

            // when
            ValueTask<ApplicationRole> modifyRoleTask =
                this.roleService.ModifyRoleRequestAsync(someRole);

            RoleDependencyException actualRoleDependencyException =
                await Assert.ThrowsAsync<RoleDependencyException>(
                    modifyRoleTask.AsTask);

            // then
            actualRoleDependencyException.Should().BeEquivalentTo(
                expectedRoleDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.roleManagementBrokerMock.Verify(broker =>
                broker.SelectRoleByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRoleDependencyException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.roleManagementBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnModifyIfDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            ApplicationRole randomRole = CreateRandomRole(randomDateTime);
            ApplicationRole someRole = randomRole;
            someRole.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedRoleException =
                   new LockedRoleException(
                       message: "Locked Role record exception, please try again later",
                       innerException: databaseUpdateConcurrencyException);

            var expectedRoleDependencyValidationException =
                new RoleDependencyValidationException(
                    message: "Role dependency validation occurred, please try again.",
                    innerException: lockedRoleException);

            this.roleManagementBrokerMock.Setup(broker =>
                broker.SelectRoleByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTime);

            // when
            ValueTask<ApplicationRole> modifyRoleTask =
                this.roleService.ModifyRoleRequestAsync(someRole);

            RoleDependencyValidationException actualRoleDependencyValidationException =
                    await Assert.ThrowsAsync<RoleDependencyValidationException>(
                        modifyRoleTask.AsTask);

            // then
            actualRoleDependencyValidationException.Should().BeEquivalentTo(
                expectedRoleDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.roleManagementBrokerMock.Verify(broker =>
                broker.SelectRoleByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRoleDependencyValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.roleManagementBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnModifyIfServiceExceptionOccursAndLogItAsync()
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            ApplicationRole randomRole = CreateRandomRole(randomDateTime);
            ApplicationRole someRole = randomRole;
            someRole.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            var serviceException = new Exception();

            var failedRoleServiceException =
                  new FailedRoleServiceException(
                      message: "Failed Role service occurred, please contact support",
                      innerException: serviceException);

            var expectedRoleServiceException =
                new RoleServiceException(
                    message: "Role service error occurred, contact support.",
                    innerException: failedRoleServiceException);

            this.roleManagementBrokerMock.Setup(broker =>
                broker.SelectRoleByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTime);

            // when
            ValueTask<ApplicationRole> modifyRoleTask =
                this.roleService.ModifyRoleRequestAsync(someRole);

            RoleServiceException actualRoleServiceException =
                await Assert.ThrowsAsync<RoleServiceException>(
                    modifyRoleTask.AsTask);

            // then
            actualRoleServiceException.Should().BeEquivalentTo(
                expectedRoleServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.roleManagementBrokerMock.Verify(broker =>
                broker.SelectRoleByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRoleServiceException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.roleManagementBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
