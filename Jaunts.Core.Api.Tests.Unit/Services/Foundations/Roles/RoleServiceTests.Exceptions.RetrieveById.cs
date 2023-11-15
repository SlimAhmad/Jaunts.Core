// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Jaunts.Core.Api.Models.Role.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Role;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Roles
{
    public partial class RoleServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someRoleId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedRoleStorageException =
                new FailedRoleStorageException(sqlException);

            var expectedRoleDependencyException =
                new RoleDependencyException(failedRoleStorageException);

            this.roleManagementBrokerMock.Setup(broker =>
                broker.SelectRoleByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<ApplicationRole> retrieveRoleTask =
                this.roleService.RetrieveRoleByIdRequestAsync(someRoleId);

            // then
            await Assert.ThrowsAsync<RoleDependencyException>(() =>
                retrieveRoleTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedRoleDependencyException))),
                        Times.Once);

            this.roleManagementBrokerMock.Verify(broker =>
                broker.SelectRoleByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.roleManagementBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            Guid someRoleId = Guid.NewGuid();
            var databaseUpdateException = new DbUpdateException();

            var failedRoleStorageException =
                new FailedRoleStorageException(databaseUpdateException);

            var expectedRoleDependencyException =
                new RoleDependencyException(failedRoleStorageException);

            this.roleManagementBrokerMock.Setup(broker =>
                broker.SelectRoleByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<ApplicationRole> retrieveRoleTask =
                this.roleService.RetrieveRoleByIdRequestAsync(someRoleId);

            // then
            await Assert.ThrowsAsync<RoleDependencyException>(() =>
                retrieveRoleTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRoleDependencyException))),
                        Times.Once);

            this.roleManagementBrokerMock.Verify(broker =>
                broker.SelectRoleByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.roleManagementBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnRetrieveByIdWhenDbConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            Guid someRoleId = Guid.NewGuid();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();
            var lockedRoleException = new LockedRoleException(databaseUpdateConcurrencyException);

            var expectedRoleDependencyValidationException =
                new RoleDependencyValidationException(lockedRoleException);

            this.roleManagementBrokerMock.Setup(broker =>
                broker.SelectRoleByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<ApplicationRole> retrieveRoleTask =
                this.roleService.RetrieveRoleByIdRequestAsync(someRoleId);

            // then
            await Assert.ThrowsAsync<RoleDependencyValidationException>(() =>
                retrieveRoleTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRoleDependencyValidationException))),
                        Times.Once);

            this.roleManagementBrokerMock.Verify(broker =>
                broker.SelectRoleByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.roleManagementBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid someRoleId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedRoleServiceException =
                new FailedRoleServiceException(serviceException);

            var expectedroleServiceException =
                new RoleServiceException(failedRoleServiceException);

            this.roleManagementBrokerMock.Setup(broker =>
                broker.SelectRoleByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<ApplicationRole> retrieveRoleTask =
                this.roleService.RetrieveRoleByIdRequestAsync(someRoleId);

            // then
            await Assert.ThrowsAsync<RoleServiceException>(() =>
                retrieveRoleTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedroleServiceException))),
                        Times.Once);

            this.roleManagementBrokerMock.Verify(broker =>
                broker.SelectRoleByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.roleManagementBrokerMock.VerifyNoOtherCalls();
        }
    }
}
