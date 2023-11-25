// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using FluentAssertions;
using Jaunts.Core.Api.Models.Role.Exceptions;
using Microsoft.Data.SqlClient;
using Moq;
using Xunit;

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
                new FailedRoleStorageException(
                    message: "Failed Role storage error occurred, contact support.",
                    innerException: sqlException);

            var expectedRoleDependencyException =
                new RoleDependencyException(
                    message: "Role dependency error occurred, contact support.",
                    innerException: failedRoleStorageException);

            this.roleManagementBrokerMock.Setup(broker =>
                broker.SelectAllRoles())
                    .Throws(sqlException);

            // when
            Action retrieveAllRolesAction = () =>
                this.roleService.RetrieveAllRoles();

            RoleDependencyException actualRoleDependencyException =
               Assert.Throws<RoleDependencyException>(
                 retrieveAllRolesAction);

            // then
            actualRoleDependencyException.Should().BeEquivalentTo(
                expectedRoleDependencyException);

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
                  new FailedRoleServiceException(
                      message: "Failed Role service occurred, please contact support",
                      innerException: serviceException);

            var expectedRoleServiceException =
                new RoleServiceException(
                    message: "Role service error occurred, contact support.",
                    innerException: failedRoleServiceException);

            this.roleManagementBrokerMock.Setup(broker =>
                broker.SelectAllRoles())
                    .Throws(serviceException);


            // When
            Action retrieveAllRolesAction = () =>
                this.roleService.RetrieveAllRoles();

            RoleServiceException actualRoleServiceException =
                 Assert.Throws<RoleServiceException>(
                    retrieveAllRolesAction);

            // then
            actualRoleServiceException.Should().BeEquivalentTo(
                expectedRoleServiceException);

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
