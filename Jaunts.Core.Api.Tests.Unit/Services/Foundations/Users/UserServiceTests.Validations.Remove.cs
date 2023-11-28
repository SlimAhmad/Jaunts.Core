// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Api.Models.User.Exceptions;
using Moq;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Users
{
    public partial class UserServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnDeleteWhenUserIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomUserId = default;
            Guid inputUserId = randomUserId;

            var invalidUserException =
              new InvalidUserException(
                  message: "Invalid User. Please correct the errors and try again.");

            invalidUserException.AddData(
                key: nameof(ApplicationUser.Id),
                values: "Id is required");

            var expectedUserValidationException =
                new UserValidationException(
                    message: "User validation errors occurred, please try again.",
                    innerException: invalidUserException);

            // when
            ValueTask<ApplicationUser> deleteUserTask =
                this.userService.RemoveUserByIdRequestAsync(inputUserId);

            UserValidationException actualUserValidationException =
                 await Assert.ThrowsAsync<UserValidationException>(
                     deleteUserTask.AsTask);

            // then
            actualUserValidationException.Should().BeEquivalentTo(
                expectedUserValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedUserValidationException))),
                        Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.userManagementBrokerMock.Verify(broker =>
                broker.DeleteUserAsync(It.IsAny<ApplicationUser>()),
                    Times.Never);

            this.userManagementBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnDeleteWhenStorageUserIsInvalidAndLogItAsync()
        {
            // given
            Guid randomUserId = Guid.NewGuid();
            Guid inputUserId = randomUserId;
            ApplicationUser invalidStorageUser = null;
            var notFoundUserException = new NotFoundUserException(inputUserId);

            var expectedUserValidationException =
                new UserValidationException(
                    message: "User validation errors occurred, please try again.",
                    innerException: notFoundUserException);

            this.userManagementBrokerMock.Setup(broker =>
                broker.SelectUserByIdAsync(inputUserId))
                    .ReturnsAsync(invalidStorageUser);

            // when
            ValueTask<ApplicationUser> deleteUserTask =
                this.userService.RemoveUserByIdRequestAsync(inputUserId);

            UserValidationException actualUserValidationException =
                 await Assert.ThrowsAsync<UserValidationException>(
                     deleteUserTask.AsTask);

            // then
            actualUserValidationException.Should().BeEquivalentTo(
                expectedUserValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedUserValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.userManagementBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(inputUserId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
        }
    }
}
