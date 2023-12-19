// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Api.Models.User.Exceptions;
using Moq;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Xunit;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Users
{
    public partial class UserServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenUserIsNullAndLogItAsync()
        {
            // given
            ApplicationUser invalidUser = null;

            var nullUserException = new NullUserException();
            string password = GetRandomPassword();

            var expectedUserValidationException =
                new UserValidationException(
                    message: "User validation errors occurred, please try again.",
                    innerException: nullUserException);

            // when
            ValueTask<ApplicationUser> createUserTask =
                this.userService.AddUserAsync(invalidUser, password);

            UserValidationException actualUserValidationException =
              await Assert.ThrowsAsync<UserValidationException>(
                  createUserTask.AsTask);

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

        [Theory]
        [InlineData(null, null, null, null, null, null)]
        [InlineData("", "", "", "", "", "")]
        [InlineData(" ", " ", " ", " ", " ", " ")]
        public async Task ShouldThrowValidationExceptionOnCreateWhenUserAndPasswordIsInvalidAndLogItAsync(
            string invalidUserUserName, string invalidEmail, string invalidFirstName,
            string invalidLastName, string invalidPhoneNumber, string invalidPassword)
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
            string password = invalidPassword;

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
               values: "Date is required");

            invalidUserException.AddData(
               key: nameof(ApplicationUser),
               values: "Value is required");

            var expectedUserValidationException =
                new UserValidationException(
                    message: "User validation errors occurred, please try again.",
                    innerException: invalidUserException);

            // when
            ValueTask<ApplicationUser> registerUserTask =
                this.userService.AddUserAsync(invalidUser, password);

            UserValidationException actualUserValidationException =
               await Assert.ThrowsAsync<UserValidationException>(
                   registerUserTask.AsTask);

            // then
            actualUserValidationException.Should().BeEquivalentTo(
                expectedUserValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedUserValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
               broker.GetCurrentDateTime(),
                   Times.Once());

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenCreatedDateIsInvalidAndLogItAsync()
        {
            // given
            ApplicationUser randomUser = CreateRandomUser();
            ApplicationUser inputUser = randomUser;
            inputUser.CreatedDate = default;
            string password = GetRandomPassword();

            var invalidUserException =
              new InvalidUserException(
                  message: "Invalid User. Please correct the errors and try again.");

            invalidUserException.AddData(
               key: nameof(ApplicationUser.CreatedDate),
               values: "Date is required");

            invalidUserException.AddData(
               key: nameof(ApplicationUser.UpdatedDate),
               values: ["Date is not the same as CreatedDate", "Date is not recent"]);


            var expectedUserValidationException =
                new UserValidationException(
                    message: "User validation errors occurred, please try again.",
                    innerException: invalidUserException);

            // when
            ValueTask<ApplicationUser> registerUserTask =
                this.userService.AddUserAsync(inputUser, password);

            UserValidationException actualUserValidationException =
              await Assert.ThrowsAsync<UserValidationException>(
                  registerUserTask.AsTask);

            // then
            actualUserValidationException.Should().BeEquivalentTo(
                expectedUserValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
               broker.GetCurrentDateTime(),
                   Times.Once());

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
        public async void ShouldThrowValidationExceptionOnCreateWhenUpdatedDateIsNotSameToCreatedDateAndLogItAsync()
        {
            // given
            ApplicationUser randomUser = CreateRandomUser();
            ApplicationUser inputUser = randomUser;
            inputUser.UpdatedDate = GetRandomDateTime();
            string password = GetRandomPassword();

            var invalidUserException =
                new InvalidUserException(
                    message: "Invalid User. Please correct the errors and try again.");

            invalidUserException.AddData(
            key: nameof(ApplicationUser.UpdatedDate),
                values: [$"Date is not the same as {nameof(ApplicationUser.CreatedDate)}", "Date is not recent"]);

            var expectedUserValidationException =
               new UserValidationException(
                   message: "User validation errors occurred, please try again.",
                   innerException: invalidUserException);

            // when
            ValueTask<ApplicationUser> registerUserTask =
                this.userService.AddUserAsync(inputUser, password);

            UserValidationException actualUserValidationException =
              await Assert.ThrowsAsync<UserValidationException>(
                  registerUserTask.AsTask);

            // then
            actualUserValidationException.Should().BeEquivalentTo(
                expectedUserValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
               broker.GetCurrentDateTime(),
                   Times.Once());

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
        [MemberData(nameof(InvalidMinuteCases))]
        public async void ShouldThrowValidationExceptionOnCreateWhenCreatedDateIsNotRecentAndLogItAsync(
            int minutes)
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            ApplicationUser randomUser = CreateRandomUser(dates: dateTime);
            ApplicationUser inputUser = randomUser;
            inputUser.CreatedDate = dateTime.AddMinutes(minutes);
            inputUser.UpdatedDate = inputUser.CreatedDate;
            string password = GetRandomPassword();

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
            ValueTask<ApplicationUser> registerUserTask =
                this.userService.AddUserAsync(inputUser, password);

            UserValidationException actualUserValidationException =
                 await Assert.ThrowsAsync<UserValidationException>(
                     registerUserTask.AsTask);

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
        public async void ShouldThrowValidationExceptionOnCreateWhenUserAlreadyExistsAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            DateTimeOffset dateTime = randomDateTime;
            ApplicationUser randomUser = CreateRandomUser(dateTime);
            ApplicationUser alreadyExistsUser = randomUser;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var duplicateKeyException = new DuplicateKeyException(exceptionMessage);
            string password = GetRandomPassword();

            var alreadyExistsUserException =
               new AlreadyExistsUserException(
                   message: "User with the same id already exists.",
                   duplicateKeyException);

            var expectedUserDependencyValidationException =
               new UserDependencyValidationException(
                   message: "User dependency validation occurred, please try again.",
                   innerException: alreadyExistsUserException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.userManagementBrokerMock.Setup(broker =>
                broker.InsertUserAsync(alreadyExistsUser, password))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<ApplicationUser> registerUserTask =
                this.userService.AddUserAsync(alreadyExistsUser, password);

            UserDependencyValidationException actualUserDependencyValidationException =
                 await Assert.ThrowsAsync<UserDependencyValidationException>(
                     registerUserTask.AsTask);

            // then
            actualUserDependencyValidationException.Should().BeEquivalentTo(
                expectedUserDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
                broker.InsertUserAsync(alreadyExistsUser, password),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedUserDependencyValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
