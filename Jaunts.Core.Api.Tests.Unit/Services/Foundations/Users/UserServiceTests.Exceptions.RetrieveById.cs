﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
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
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someUserId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedUserStorageException =
                new FailedUserStorageException(sqlException);

            var expectedUserDependencyException =
                new UserDependencyException(failedUserStorageException);

            this.userManagementBrokerMock.Setup(broker =>
                broker.SelectUserByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<ApplicationUser> retrieveUserTask =
                this.userService.RetrieveUserByIdRequestAsync(someUserId);

            // then
            await Assert.ThrowsAsync<UserDependencyException>(() =>
                retrieveUserTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedUserDependencyException))),
                        Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            Guid someUserId = Guid.NewGuid();
            var databaseUpdateException = new DbUpdateException();

            var failedUserStorageException =
                new FailedUserStorageException(databaseUpdateException);

            var expectedUserDependencyException =
                new UserDependencyException(failedUserStorageException);

            this.userManagementBrokerMock.Setup(broker =>
                broker.SelectUserByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<ApplicationUser> retrieveUserTask =
                this.userService.RetrieveUserByIdRequestAsync(someUserId);

            // then
            await Assert.ThrowsAsync<UserDependencyException>(() =>
                retrieveUserTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedUserDependencyException))),
                        Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnRetrieveByIdWhenDbConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            Guid someUserId = Guid.NewGuid();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();
            var lockedUserException = new LockedUserException(databaseUpdateConcurrencyException);

            var expectedUserDependencyValidationException =
                new UserDependencyValidationException(lockedUserException);

            this.userManagementBrokerMock.Setup(broker =>
                broker.SelectUserByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<ApplicationUser> retrieveUserTask =
                this.userService.RetrieveUserByIdRequestAsync(someUserId);

            // then
            await Assert.ThrowsAsync<UserDependencyValidationException>(() =>
                retrieveUserTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedUserDependencyValidationException))),
                        Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid someUserId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedUserServiceException =
                new FailedUserServiceException(serviceException);

            var expectedUserServiceException =
                new UserServiceException(failedUserServiceException);

            this.userManagementBrokerMock.Setup(broker =>
                broker.SelectUserByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<ApplicationUser> retrieveUserTask =
                this.userService.RetrieveUserByIdRequestAsync(someUserId);

            // then
            await Assert.ThrowsAsync<UserServiceException>(() =>
                retrieveUserTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedUserServiceException))),
                        Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
        }
    }
}
