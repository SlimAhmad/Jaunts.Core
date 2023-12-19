// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Api.Models.User.Exceptions;
using Moq;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Users
{
    public partial class UserServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyWhenUserIsNullAndLogItAsync()
        {
            // given
            ApplicationUser randomUser = null;
            ApplicationUser nullUser = randomUser;
            var nullUserException = new NullUserException();

            var expectedUserValidationException =
                new UserValidationException(
                    message: "User validation errors occurred, please try again.",
                    innerException: nullUserException);

            // when
            ValueTask<ApplicationUser> modifyUserTask =
                this.userService.ModifyUserAsync(nullUser);

            UserValidationException actualUserValidationException =
                 await Assert.ThrowsAsync<UserValidationException>(
                     modifyUserTask.AsTask);

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

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyWhenIdIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetCurrentDateTime();
            ApplicationUser randomUser = CreateRandomUser();
            ApplicationUser inputUser = randomUser;
            inputUser.Id = default;

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

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);
            // when
            ValueTask<ApplicationUser> modifyUserTask =
                this.userService.ModifyUserAsync(inputUser);

            UserValidationException actualUserValidationException =
                 await Assert.ThrowsAsync<UserValidationException>(
                     modifyUserTask.AsTask);

            // then
            actualUserValidationException.Should().BeEquivalentTo(
                expectedUserValidationException);


            this.dateTimeBrokerMock.Verify(broker =>
                 broker.GetCurrentDateTime(),
                 Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedUserValidationException))),
                        Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null, null, null, null, null)]
        [InlineData("", "", "", "", "")]
        [InlineData(" ", " ", " ", " ", " ")]
        public async Task ShouldThrowValidationExceptionOnModifyWhenUserIsInvalidAndLogItAsync(
            string invalidUserUserName, string invalidEmail, string invalidFirstName,
            string invalidLastName, string invalidPhoneNumber)
        {
            // given
            ApplicationUser randomUser = CreateRandomUser();
            ApplicationUser invalidUser = randomUser;
            invalidUser.UserName = invalidUserUserName;
            invalidUser.Email = invalidEmail;
            invalidUser.FirstName = invalidFirstName;
            invalidUser.LastName = invalidLastName;
            invalidUser.PhoneNumber = invalidPhoneNumber;
            invalidUser.CreatedDate = default;
            invalidUser.UpdatedDate = default;
            invalidUser.Id = default;

            var invalidUserException =
                new InvalidUserException(
                    message: "Invalid User. Please correct the errors and try again.");

            invalidUserException.AddData(
                key: nameof(ApplicationUser.UserName),
                values: "Value is required");

            invalidUserException.AddData(
               key: nameof(ApplicationUser.Email),
               values: "Value is required");

            invalidUserException.AddData(
               key: nameof(ApplicationUser.FirstName),
               values: "Value is required");

            invalidUserException.AddData(
               key: nameof(ApplicationUser.LastName),
               values: "Value is required");

            invalidUserException.AddData(
               key: nameof(ApplicationUser.PhoneNumber),
               values: "Value is required");

            invalidUserException.AddData(
               key: nameof(ApplicationUser.CreatedDate),
               values: "Date is required");

            invalidUserException.AddData(
               key: nameof(ApplicationUser.UpdatedDate),
               values: ["Date is required", "Date is the same as CreatedDate"]);

            invalidUserException.AddData(
               key: nameof(ApplicationUser.Id),
               values: "Id is required");

            var expectedUserValidationException =
                new UserValidationException(
                    message: "User validation errors occurred, please try again.",
                    innerException: invalidUserException);

            // when
            ValueTask<ApplicationUser> modifyUserTask =
                this.userService.ModifyUserAsync(invalidUser);

            UserValidationException actualUserValidationException =
                 await Assert.ThrowsAsync<UserValidationException>(
                     modifyUserTask.AsTask);

            // then
            actualUserValidationException.Should().BeEquivalentTo(
                expectedUserValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedUserValidationException))),
                        Times.Once);


            this.dateTimeBrokerMock.Verify(broker =>
                 broker.GetCurrentDateTime(),
                 Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyWhenUpdatedDateIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetCurrentDateTime();
            ApplicationUser randomUser = CreateRandomUser(dateTime);
            ApplicationUser inputUser = randomUser;
            inputUser.UpdatedDate = default;

            var invalidUserException =
                new InvalidUserException(
                    message: "Invalid User. Please correct the errors and try again.");

            invalidUserException.AddData(
                key: nameof(ApplicationUser.UpdatedDate),
                values: ["Date is required","Date is not recent"]);

            var expectedUserValidationException =
                new UserValidationException(
                    message: "User validation errors occurred, please try again.",
                    innerException: invalidUserException);

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Returns(dateTime);

            // when
            ValueTask<ApplicationUser> modifyUserTask =
                this.userService.ModifyUserAsync(inputUser);

            UserValidationException actualUserValidationException =
                 await Assert.ThrowsAsync<UserValidationException>(
                     modifyUserTask.AsTask);

            // then
            actualUserValidationException.Should().BeEquivalentTo(
                expectedUserValidationException);


            this.dateTimeBrokerMock.Verify(broker =>
                 broker.GetCurrentDateTime(),
                 Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedUserValidationException))),
                        Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyWhenCreatedDateIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetCurrentDateTime();
            ApplicationUser randomUser = CreateRandomUser();
            ApplicationUser inputUser = randomUser;
            inputUser.CreatedDate = default;

            var invalidUserException =
                new InvalidUserException(
                    message: "Invalid User. Please correct the errors and try again.");

            invalidUserException.AddData(
                key: nameof(ApplicationUser.CreatedDate),
                values: "Date is required");

            var expectedUserValidationException =
                new UserValidationException(
                    message: "User validation errors occurred, please try again.",
                    innerException: invalidUserException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<ApplicationUser> modifyUserTask =
                this.userService.ModifyUserAsync(inputUser);

            UserValidationException actualUserValidationException =
                 await Assert.ThrowsAsync<UserValidationException>(
                     modifyUserTask.AsTask);

            // then
            actualUserValidationException.Should().BeEquivalentTo(
                expectedUserValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                 broker.GetCurrentDateTime(),
                 Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedUserValidationException))),
                        Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyWhenUpdatedDateIsSameAsCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            ApplicationUser randomUser = CreateRandomUser(dateTime);
            ApplicationUser inputUser = randomUser;


            var invalidUserException =
                new InvalidUserException(
                    message: "Invalid User. Please correct the errors and try again.");

            invalidUserException.AddData(
            key: nameof(ApplicationUser.UpdatedDate),
                values: [$"Date is the same as {nameof(ApplicationUser.CreatedDate)}", "Date is not recent"]);

            var expectedUserValidationException =
               new UserValidationException(
                   message: "User validation errors occurred, please try again.",
                   innerException: invalidUserException);

            // when
            ValueTask<ApplicationUser> modifyUserTask =
                this.userService.ModifyUserAsync(inputUser);

            UserValidationException actualUserValidationException =
                 await Assert.ThrowsAsync<UserValidationException>(
                     modifyUserTask.AsTask);

            // then
            actualUserValidationException.Should().BeEquivalentTo(
                expectedUserValidationException);


            this.dateTimeBrokerMock.Verify(broker =>
                 broker.GetCurrentDateTime(),
                 Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedUserValidationException))),
                        Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.userManagementBrokerMock.Verify(broker =>
                broker.UpdateUserAsync(It.IsAny<ApplicationUser>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidMinuteCases))]
        public async void ShouldThrowValidationExceptionOnModifyWhenUpdatedDateIsNotRecentAndLogItAsync(
            int minutes)
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            ApplicationUser randomUser = CreateRandomUser(dateTime);
            ApplicationUser inputUser = randomUser;
            inputUser.UpdatedDate = dateTime.AddMinutes(minutes);

            var invalidUserException =
                 new InvalidUserException(
                     message: "Invalid User. Please correct the errors and try again.");

            invalidUserException.AddData(
            key: nameof(ApplicationUser.UpdatedDate),
                values:  "Date is not recent");

            var expectedUserValidationException =
               new UserValidationException(
                   message: "User validation errors occurred, please try again.",
                   innerException: invalidUserException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<ApplicationUser> modifyUserTask =
                this.userService.ModifyUserAsync(inputUser);

            UserValidationException actualUserValidationException =
                 await Assert.ThrowsAsync<UserValidationException>(
                     modifyUserTask.AsTask);

            // then
            actualUserValidationException.Should().BeEquivalentTo(
                expectedUserValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedUserValidationException))),
                        Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.userManagementBrokerMock.Verify(broker =>
                broker.UpdateUserAsync(It.IsAny<ApplicationUser>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfUserDoesntExistAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetRandomNegativeNumber();
            DateTimeOffset dateTime = GetRandomDateTime();
            ApplicationUser randomUser = CreateRandomUser(dateTime);
            ApplicationUser nonExistentUser = randomUser;
            nonExistentUser.CreatedDate = dateTime.AddMinutes(randomNegativeMinutes);
            ApplicationUser noUser = null;

            var notFoundUserException = new NotFoundUserException(nonExistentUser.Id);

            var expectedUserValidationException =
               new UserValidationException(
                   message: "User validation errors occurred, please try again.",
                   innerException: notFoundUserException);

            this.userManagementBrokerMock.Setup(broker =>
                broker.SelectUserByIdAsync(nonExistentUser.Id))
                    .ReturnsAsync(noUser);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<ApplicationUser> modifyUserTask =
                this.userService.ModifyUserAsync(nonExistentUser);

            UserValidationException actualUserValidationException =
                 await Assert.ThrowsAsync<UserValidationException>(
                     modifyUserTask.AsTask);

            // then
            actualUserValidationException.Should().BeEquivalentTo(
                expectedUserValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(nonExistentUser.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedUserValidationException))),
                        Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
                broker.UpdateUserAsync(It.IsAny<ApplicationUser>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageCreatedDateNotSameAsCreateDateAndLogItAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            int randomMinutes = randomNumber;
            DateTimeOffset randomDate = GetCurrentDateTime();
            ApplicationUser randomUser = CreateRandomModifyUser(randomDate);
            ApplicationUser invalidUser = randomUser;
            invalidUser.UpdatedDate = randomDate;
            ApplicationUser storageUser = randomUser.DeepClone();
            storageUser.CreatedDate = storageUser.CreatedDate.AddMinutes(randomMinutes);
            storageUser.UpdatedDate = storageUser.UpdatedDate.AddMinutes(randomMinutes);
            Guid UserId = invalidUser.Id;

            var invalidUserException =
                new InvalidUserException(
                    message: "Invalid User. Please correct the errors and try again.");

            invalidUserException.AddData(
                key: nameof(ApplicationUser.CreatedDate),
                    values: $"Date is not the same as {nameof(ApplicationUser.CreatedDate)}");

            var expectedUserValidationException =
               new UserValidationException(
                   message: "User validation errors occurred, please try again.",
                   innerException: invalidUserException);

            this.userManagementBrokerMock.Setup(broker =>
                broker.SelectUserByIdAsync(UserId))
                    .ReturnsAsync(storageUser);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            // when
            ValueTask<ApplicationUser> modifyUserTask =
                this.userService.ModifyUserAsync(invalidUser);

            UserValidationException actualUserValidationException =
                 await Assert.ThrowsAsync<UserValidationException>(
                     modifyUserTask.AsTask);

            // then
            actualUserValidationException.Should().BeEquivalentTo(
                expectedUserValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(invalidUser.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedUserValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageUpdatedDateSameAsUpdatedDateAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetRandomNegativeNumber();
            int minutesInThePast = randomNegativeMinutes;
            DateTimeOffset randomDate = GetRandomDateTime();
            ApplicationUser randomUser = CreateRandomModifyUser(randomDate);
            randomUser.CreatedDate = randomUser.CreatedDate.AddMinutes(minutesInThePast);
            ApplicationUser invalidUser = randomUser;
            invalidUser.UpdatedDate = randomDate;
            ApplicationUser storageUser = randomUser.DeepClone();
            Guid UserId = invalidUser.Id;


            var invalidUserException =
                 new InvalidUserException(
                     message: "Invalid User. Please correct the errors and try again.");

            invalidUserException.AddData(
                key: nameof(ApplicationUser.UpdatedDate),
                    values: $"Date is the same as {nameof(ApplicationUser.UpdatedDate)}");

            var expectedUserValidationException =
               new UserValidationException(
                   message: "User validation errors occurred, please try again.",
                   innerException: invalidUserException);

            this.userManagementBrokerMock.Setup(broker =>
                broker.SelectUserByIdAsync(UserId))
                    .ReturnsAsync(storageUser);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            // when
            ValueTask<ApplicationUser> modifyUserTask =
                this.userService.ModifyUserAsync(invalidUser);

            UserValidationException actualUserValidationException =
                 await Assert.ThrowsAsync<UserValidationException>(
                     modifyUserTask.AsTask);

            // then
            actualUserValidationException.Should().BeEquivalentTo(
                expectedUserValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(invalidUser.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedUserValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
