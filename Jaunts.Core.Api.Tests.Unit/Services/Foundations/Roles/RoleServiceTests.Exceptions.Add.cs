// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Role.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Role;
using Microsoft.EntityFrameworkCore;
using Moq;

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
                new FailedRoleStorageException(sqlException);

            var expectedRoleDependencyException =
                new RoleDependencyException(failedRoleStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.roleManagementBrokerMock.Setup(broker =>
                broker.InsertRoleAsync(It.IsAny<ApplicationRole>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<ApplicationRole> registerRoleTask =
                this.roleService.RegisterRoleRequestAsync(inputRole);

            // then
            await Assert.ThrowsAsync<RoleDependencyException>(() =>
                registerRoleTask.AsTask());

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
        public async Task ShouldThrowDependencyExceptionOnCreateWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            ApplicationRole randomRole = CreateRandomRole(dates: dateTime);
            ApplicationRole inputRole = randomRole;
            var databaseUpdateException = new DbUpdateException();
            string password = GetRandomPassword();

            var failedRoleStorageException =
                new FailedRoleStorageException(databaseUpdateException);

            var expectedRoleDependencyException =
                new RoleDependencyException(failedRoleStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.roleManagementBrokerMock.Setup(broker =>
                broker.InsertRoleAsync(It.IsAny<ApplicationRole>()))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<ApplicationRole> registerRoleTask =
                this.roleService.RegisterRoleRequestAsync(inputRole);

            // then
            await Assert.ThrowsAsync<RoleDependencyException>(() =>
                registerRoleTask.AsTask());

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
                new FailedRoleServiceException(serviceException);

            var expectedAssignmentServiceException =
                new RoleServiceException(failedRoleServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.roleManagementBrokerMock.Setup(broker =>
                broker.InsertRoleAsync(It.IsAny<ApplicationRole>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<ApplicationRole> registerRoleTask =
                 this.roleService.RegisterRoleRequestAsync(inputRole);

            // then
            await Assert.ThrowsAsync<RoleServiceException>(() =>
                registerRoleTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAssignmentServiceException))),
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
