// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Role.Exceptions;
using Microsoft.Data.SqlClient;
using Moq;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Roles
{
    public partial class RoleServiceTests
    {
        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
            SqlException sqlException = GetSqlException();

            var failedRoleStorageException =
                new FailedRoleStorageException(sqlException);

            var expectedRoleDependencyException =
                new RoleDependencyException(failedRoleStorageException);

            this.roleManagementBrokerMock.Setup(broker =>
                broker.SelectAllRoles())
                    .Throws(sqlException);

            // when
            Action retrieveAllRolesAction = () =>
                this.roleService.RetrieveAllRoles();

            // then
            Assert.Throws<RoleDependencyException>(
                retrieveAllRolesAction);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedRoleDependencyException))),
                        Times.Once);

            this.roleManagementBrokerMock.Verify(broker =>
                broker.SelectAllRoles(),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.roleManagementBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllWhenExceptionOccursAndLogIt()
        {
            // given
            var serviceException = new Exception();

            var failedRoleServiceException =
                new FailedRoleServiceException(serviceException);

            var expectedRoleServiceException =
                new RoleServiceException(failedRoleServiceException);

            this.roleManagementBrokerMock.Setup(broker =>
                broker.SelectAllRoles())
                    .Throws(serviceException);


            // When
            Action retrieveAllRolesAction = () =>
                this.roleService.RetrieveAllRoles();

            // then
            Assert.Throws<RoleServiceException>(
                retrieveAllRolesAction);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRoleServiceException))),
                        Times.Once);

            this.roleManagementBrokerMock.Verify(broker =>
                broker.SelectAllRoles(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.roleManagementBrokerMock.VerifyNoOtherCalls();
        }
    }
}
