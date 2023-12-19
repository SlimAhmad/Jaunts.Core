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
        public async Task ShouldThrowDependencyExceptionOnModifyIfSqlExceptionOccursAndLogItAsync()
        {
            // given
            int randomNegativeNumber = GetRandomNegativeNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            ApplicationUser randomUser = CreateRandomUser(dates: randomDateTime);
            ApplicationUser someUser = randomUser;
            someUser.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
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
                 broker.SelectUserByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTime);

            // when
            ValueTask<ApplicationUser> modifyUserTask =
                this.userService.ModifyUserAsync(someUser);

            UserDependencyException actualUserDependencyException =
             await Assert.ThrowsAsync<UserDependencyException>(
                 modifyUserTask.AsTask);

            // then
            actualUserDependencyException.Should().BeEquivalentTo(
                expectedUserDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedUserDependencyException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDbUpdateExceptionOccursAndLogItAsync()
        {
            // given
            int randomNegativeNumber = GetRandomNegativeNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            ApplicationUser randomUser = CreateRandomUser(randomDateTime);
            ApplicationUser someUser = randomUser;
            someUser.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
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
                broker.SelectUserByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTime);

            // when
            ValueTask<ApplicationUser> modifyUserTask =
                this.userService.ModifyUserAsync(someUser);

            UserDependencyException actualUserDependencyException =
                await Assert.ThrowsAsync<UserDependencyException>(
                    modifyUserTask.AsTask);

            // then
            actualUserDependencyException.Should().BeEquivalentTo(
                expectedUserDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedUserDependencyException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnModifyIfDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            int randomNegativeNumber = GetRandomNegativeNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            ApplicationUser randomUser = CreateRandomUser(randomDateTime);
            ApplicationUser someUser = randomUser;
            someUser.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
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
                broker.SelectUserByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTime);

            // when
            ValueTask<ApplicationUser> modifyUserTask =
                this.userService.ModifyUserAsync(someUser);

            UserDependencyValidationException actualUserDependencyValidationException =
                   await Assert.ThrowsAsync<UserDependencyValidationException>(
                       modifyUserTask.AsTask);

            // then
            actualUserDependencyValidationException.Should().BeEquivalentTo(
                expectedUserDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedUserDependencyValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnModifyIfServiceExceptionOccursAndLogItAsync()
        {
            // given
            int randomNegativeNumber = GetRandomNegativeNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            ApplicationUser randomUser = CreateRandomUser(randomDateTime);
            ApplicationUser someUser = randomUser;
            someUser.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
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
                broker.SelectUserByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTime);

            // when
            ValueTask<ApplicationUser> modifyUserTask =
                this.userService.ModifyUserAsync(someUser);

            UserServiceException actualUserServiceException =
               await Assert.ThrowsAsync<UserServiceException>(
                   modifyUserTask.AsTask);

            // then
            actualUserServiceException.Should().BeEquivalentTo(
                expectedUserServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedUserServiceException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
