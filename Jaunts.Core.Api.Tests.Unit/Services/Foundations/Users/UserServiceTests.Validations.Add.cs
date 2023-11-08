// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using EFxceptions.Models.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Api.Models.User.Exceptions;
using Microsoft.Extensions.Hosting;
using Moq;
using System;

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
                new UserValidationException(nullUserException);

            // when
            ValueTask<ApplicationUser> createUserTask =
                this.userService.RegisterUserRequestAsync(invalidUser, password);

            // then
            await Assert.ThrowsAsync<UserValidationException>(() =>
                createUserTask.AsTask());

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
        public async void ShouldThrowValidationExceptionOnCreateWhenIdIsInvalidAndLogItAsync()
        {
            // given
            ApplicationUser randomUser = CreateRandomUser();
            ApplicationUser inputUser = randomUser;
            inputUser.Id = default;
            string password = GetRandomPassword();

            var invalidUserInputException = new InvalidUserException();

            var expectedUserValidationException =
                new UserValidationException(invalidUserInputException);

            // when
            ValueTask<ApplicationUser> registerUserTask =
                this.userService.RegisterUserRequestAsync(inputUser, password);

            // then
            await Assert.ThrowsAsync<UserValidationException>(() =>
                registerUserTask.AsTask());

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
        [InlineData(null,null,null,null,null,null)]
        [InlineData("","","","","","")]
        [InlineData(" "," "," "," "," "," ")]
        public async Task ShouldThrowValidationExceptionOnCreateWhenUserUserNameIsInvalidAndLogItAsync(
            string invalidUserUserName,string invalidEmail,string invalidFirstName,
            string invalidLastName,string invalidPhoneNumber,string invalidPassword)
        {
            // given
            ApplicationUser randomUser = CreateRandomUser();
            ApplicationUser invalidUser = randomUser;
            invalidUser.UserName = invalidUserUserName;
            invalidUser.Email = invalidEmail;
            invalidUser.FirstName = invalidFirstName;
            invalidUser.LastName = invalidLastName; 
            invalidUser.PhoneNumber = invalidPhoneNumber;   
            string password = invalidPassword;

            var invalidUserException = new InvalidUserException();

            invalidUserException.AddData(
                key: nameof(ApplicationUser.UserName),
                values: "Text is required");

            invalidUserException.AddData(
               key: nameof(ApplicationUser.Email),
               values: "Text is required");

            invalidUserException.AddData(
               key: nameof(ApplicationUser.FirstName),
               values: "Text is required");

            invalidUserException.AddData(
               key: nameof(ApplicationUser.LastName),
               values: "Text is required");

            invalidUserException.AddData(
               key: nameof(ApplicationUser.PhoneNumber),
               values: "Text is required");

            invalidUserException.AddData(
               key: nameof(ApplicationUser),
               values: "Text is required");

            invalidUserException.AddData(
               key: nameof(ApplicationUser.CreatedDate),
               values: "Date is required");

            invalidUserException.AddData(
               key: nameof(ApplicationUser.UpdatedDate),
               values: "Date is required");

            var expectedUserValidationException =
                new UserValidationException(invalidUserException);

            // when
            ValueTask<ApplicationUser> registerUserTask =
                this.userService.RegisterUserRequestAsync(invalidUser, password);

            // then
            await Assert.ThrowsAsync<UserValidationException>(() =>
                registerUserTask.AsTask());

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

            var invalidUserException = new InvalidUserException();

            invalidUserException.AddData(
                key: nameof(ApplicationUser.UserName),
                values: "Text is required");

            invalidUserException.AddData(
               key: nameof(ApplicationUser.Email),
               values: "Text is required");

            invalidUserException.AddData(
               key: nameof(ApplicationUser.FirstName),
               values: "Text is required");

            invalidUserException.AddData(
               key: nameof(ApplicationUser.LastName),
               values: "Text is required");

            invalidUserException.AddData(
               key: nameof(ApplicationUser.PhoneNumber),
               values: "Text is required");

            invalidUserException.AddData(
               key: nameof(ApplicationUser),
               values: "Text is required");

            invalidUserException.AddData(
               key: nameof(ApplicationUser.CreatedDate),
               values: "Date is required");

            invalidUserException.AddData(
               key: nameof(ApplicationUser.UpdatedDate),
               values: "Date is required");

            var expectedUserValidationException =
                new UserValidationException(invalidUserException);

            // when
            ValueTask<ApplicationUser> registerUserTask =
                this.userService.RegisterUserRequestAsync(inputUser, password);

            // then
            await Assert.ThrowsAsync<UserValidationException>(() =>
                registerUserTask.AsTask());

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
        public async void ShouldThrowValidationExceptionOnCreateWhenUpdatedDateIsInvalidAndLogItAsync()
        {
            // given
            ApplicationUser randomUser = CreateRandomUser();
            ApplicationUser inputUser = randomUser;
            inputUser.UpdatedDate = default;
            string password = GetRandomPassword();

            var invalidUserException = new InvalidUserException();

            invalidUserException.AddData(
                key: nameof(ApplicationUser.UserName),
                values: "Text is required");

            invalidUserException.AddData(
               key: nameof(ApplicationUser.Email),
               values: "Text is required");

            invalidUserException.AddData(
               key: nameof(ApplicationUser.FirstName),
               values: "Text is required");

            invalidUserException.AddData(
               key: nameof(ApplicationUser.LastName),
               values: "Text is required");

            invalidUserException.AddData(
               key: nameof(ApplicationUser.PhoneNumber),
               values: "Text is required");

            invalidUserException.AddData(
               key: nameof(ApplicationUser),
               values: "Text is required");

            invalidUserException.AddData(
               key: nameof(ApplicationUser.CreatedDate),
               values: "Date is required");

            invalidUserException.AddData(
               key: nameof(ApplicationUser.UpdatedDate),
               values: "Date is required");

            var expectedUserValidationException =
                new UserValidationException(invalidUserException);

            // when
            ValueTask<ApplicationUser> registerUserTask =
                this.userService.RegisterUserRequestAsync(inputUser, password);

            // then
            await Assert.ThrowsAsync<UserValidationException>(() =>
                registerUserTask.AsTask());

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
                 new InvalidUserException();

            invalidUserException.AddData(
            key: nameof(ApplicationUser.UpdatedDate),
                values: $"Date is not the same as {nameof(ApplicationUser.CreatedDate)}");

            var expectedUserValidationException =
                new UserValidationException(invalidUserException);

            // when
            ValueTask<ApplicationUser> registerUserTask =
                this.userService.RegisterUserRequestAsync(inputUser, password);

            // then
            await Assert.ThrowsAsync<UserValidationException>(() =>
                registerUserTask.AsTask());

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
                 new InvalidUserException();

            invalidUserException.AddData(
                key: nameof(ApplicationUser.CreatedDate),
                values: "Date is not recent");

            var expectedUserValidationException =
                new UserValidationException(invalidUserException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<ApplicationUser> registerUserTask =
                this.userService.RegisterUserRequestAsync(inputUser, password);

            // then
            await Assert.ThrowsAsync<UserValidationException>(() =>
                registerUserTask.AsTask());

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
                new AlreadyExistsUserException(duplicateKeyException);

            var expectedUserValidationException =
                new UserDependencyValidationException(alreadyExistsUserException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.userManagementBrokerMock.Setup(broker =>
                broker.InsertUserAsync(alreadyExistsUser, password))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<ApplicationUser> registerUserTask =
                this.userService.RegisterUserRequestAsync(alreadyExistsUser, password);

            // then
            await Assert.ThrowsAsync<UserDependencyValidationException>(() =>
                registerUserTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
                broker.InsertUserAsync(alreadyExistsUser, password),
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
