// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Api.Models.User.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Users
{
    public partial class UserServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnConfirmEmailWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            ApplicationUser someUser = CreateRandomUser();
            string someToken = GetRandomString();
            SqlException sqlException = GetSqlException();

            var failedUserStorageException =
                new FailedUserStorageException(
                    message: "Failed User storage error occurred, contact support.",
                    innerException: sqlException);

            var expectedUserDependencyException =
                new UserDependencyException(
                    message: "User dependency error occurred, contact support.",
                    innerException: failedUserStorageException);

            this.userManagementBrokerMock.Setup(broker =>
                broker.ConfirmEmailTokenAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<ApplicationUser> retrieveUserTask =
                this.userService.ConfirmEmailAsync(someUser,someToken);

            UserDependencyException actualUserDependencyException =
                 await Assert.ThrowsAsync<UserDependencyException>(
                     retrieveUserTask.AsTask);

            // then
            actualUserDependencyException.Should().BeEquivalentTo(
                expectedUserDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedUserDependencyException))),
                        Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
                broker.ConfirmEmailTokenAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnConfirmEmailWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            ApplicationUser someUser = CreateRandomUser();
            string someToken = GetRandomString();
            var databaseUpdateException = new DbUpdateException();

            var failedUserStorageException =
               new FailedUserStorageException(
                   message: "Failed User storage error occurred, contact support.",
                   innerException: databaseUpdateException);

            var expectedUserDependencyException =
                new UserDependencyException(
                    message: "User dependency error occurred, contact support.",
                    innerException: failedUserStorageException);

            this.userManagementBrokerMock.Setup(broker =>
                broker.ConfirmEmailTokenAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<ApplicationUser> retrieveUserTask =
                this.userService.ConfirmEmailAsync(someUser,someToken);

            UserDependencyException actualUserDependencyException =
                    await Assert.ThrowsAsync<UserDependencyException>(
                        retrieveUserTask.AsTask);

            // then
            actualUserDependencyException.Should().BeEquivalentTo(
                expectedUserDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedUserDependencyException))),
                        Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
                broker.ConfirmEmailTokenAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnConfirmEmailWhenDbConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            ApplicationUser someUser = CreateRandomUser();
            string someToken = GetRandomString();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedUserException =
               new LockedUserException(
                   message: "Locked User record exception, please try again later",
                   innerException: databaseUpdateConcurrencyException);

            var expectedUserDependencyValidationException =
                new UserDependencyValidationException(
                    message: "User dependency validation occurred, please try again.",
                    innerException: lockedUserException);

            this.userManagementBrokerMock.Setup(broker =>
                broker.ConfirmEmailTokenAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<ApplicationUser> retrieveUserTask =
                this.userService.ConfirmEmailAsync(someUser,someToken);

            UserDependencyValidationException actualUserDependencyValidationException =
                   await Assert.ThrowsAsync<UserDependencyValidationException>(
                       retrieveUserTask.AsTask);

            // then
            actualUserDependencyValidationException.Should().BeEquivalentTo(
                expectedUserDependencyValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedUserDependencyValidationException))),
                        Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
                broker.ConfirmEmailTokenAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnConfirmEmailWhenExceptionOccursAndLogItAsync()
        {
            // given
            ApplicationUser someUser = CreateRandomUser();
            string someToken = GetRandomString();
            var serviceException = new Exception();

            var failedUserServiceException =
                new FailedUserServiceException(
                    message: "Failed User service occurred, please contact support",
                    innerException: serviceException);

            var expectedUserServiceException =
                new UserServiceException(
                    message: "User service error occurred, contact support.",
                    innerException: failedUserServiceException);

            this.userManagementBrokerMock.Setup(broker =>
                broker.ConfirmEmailTokenAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<ApplicationUser> retrieveUserTask =
                this.userService.ConfirmEmailAsync(someUser, someToken);

            UserServiceException actualUserServiceException =
                await Assert.ThrowsAsync<UserServiceException>(
                    retrieveUserTask.AsTask);

            // then
            actualUserServiceException.Should().BeEquivalentTo(
                expectedUserServiceException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedUserServiceException))),
                        Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
                broker.ConfirmEmailTokenAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
        }
    }
}
