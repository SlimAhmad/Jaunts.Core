// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
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
                new UserValidationException(nullUserException);

            // when
            ValueTask<ApplicationUser> modifyUserTask =
                this.userService.ModifyUserRequestAsync(nullUser);

            // then
            await Assert.ThrowsAsync<UserValidationException>(() =>
                modifyUserTask.AsTask());

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
            ApplicationUser randomUser = CreateRandomUser();
            ApplicationUser inputUser = randomUser;
            inputUser.Id = default;

            var invalidUserException =
                 new InvalidUserException();

            invalidUserException.AddData(
                key: nameof(ApplicationUser.CreatedDate),
                values: "Id is required");

            var expectedUserValidationException =
                new UserValidationException(invalidUserException);

            // when
            ValueTask<ApplicationUser> modifyUserTask =
                this.userService.ModifyUserRequestAsync(inputUser);

            // then
            await Assert.ThrowsAsync<UserValidationException>(() =>
                modifyUserTask.AsTask());


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
            ValueTask<ApplicationUser> modifyUserTask =
                this.userService.ModifyUserRequestAsync(invalidUser);

            // then
            await Assert.ThrowsAsync<UserValidationException>(() =>
                modifyUserTask.AsTask());

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
            ApplicationUser randomUser = CreateRandomUser();
            ApplicationUser inputUser = randomUser;
            inputUser.UpdatedDate = default;

            var invalidUserException =
                new InvalidUserException();

            invalidUserException.AddData(
                key: nameof(ApplicationUser.UpdatedDate),
                values: "Date is required");

            var expectedUserValidationException =
                new UserValidationException(invalidUserException);

            // when
            ValueTask<ApplicationUser> modifyUserTask =
                this.userService.ModifyUserRequestAsync(inputUser);

            // then
            await Assert.ThrowsAsync<UserValidationException>(() =>
                modifyUserTask.AsTask());


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
            ApplicationUser randomUser = CreateRandomUser();
            ApplicationUser inputUser = randomUser;
            inputUser.CreatedDate = default;

            var invalidUserException =
                new InvalidUserException();

            invalidUserException.AddData(
                key: nameof(ApplicationUser.CreatedDate),
                values: "Date is required");

            var expectedUserValidationException =
                new UserValidationException(invalidUserException);

            // when
            ValueTask<ApplicationUser> modifyUserTask =
                this.userService.ModifyUserRequestAsync(inputUser);

            // then
            await Assert.ThrowsAsync<UserValidationException>(() =>
                modifyUserTask.AsTask());

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
                 new InvalidUserException();

            invalidUserException.AddData(
            key: nameof(ApplicationUser.UpdatedDate),
                values: $"Date is not the same as {nameof(ApplicationUser.UpdatedDate)}");

            var expectedUserValidationException =
                new UserValidationException(invalidUserException);

            // when
            ValueTask<ApplicationUser> modifyUserTask =
                this.userService.ModifyUserRequestAsync(inputUser);

            // then
            await Assert.ThrowsAsync<UserValidationException>(() =>
                modifyUserTask.AsTask());


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
                   new InvalidUserException();

            invalidUserException.AddData(
                key: nameof(ApplicationUser.UpdatedDate),
                values: "Date is not recent");

            var expectedUserValidationException =
                new UserValidationException(invalidUserException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<ApplicationUser> modifyUserTask =
                this.userService.ModifyUserRequestAsync(inputUser);

            // then
            await Assert.ThrowsAsync<UserValidationException>(() =>
                modifyUserTask.AsTask());

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
            int randomNegativeMinutes = GetNegativeRandomNumber();
            DateTimeOffset dateTime = GetRandomDateTime();
            ApplicationUser randomUser = CreateRandomUser(dateTime);
            ApplicationUser nonExistentUser = randomUser;
            nonExistentUser.CreatedDate = dateTime.AddMinutes(randomNegativeMinutes);
            ApplicationUser noUser = null;
            var notFoundUserException = new NotFoundUserException(nonExistentUser.Id);

            var expectedUserValidationException =
                new UserValidationException(notFoundUserException);

            this.userManagementBrokerMock.Setup(broker =>
                broker.SelectUserByIdAsync(nonExistentUser.Id))
                    .ReturnsAsync(noUser);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<ApplicationUser> modifyUserTask =
                this.userService.ModifyUserRequestAsync(nonExistentUser);

            // then
            await Assert.ThrowsAsync<UserValidationException>(() =>
                modifyUserTask.AsTask());

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
            DateTimeOffset randomDate = GetRandomDateTime();
            ApplicationUser randomUser = CreateRandomUser(randomDate);
            ApplicationUser invalidUser = randomUser;
            invalidUser.UpdatedDate = randomDate;
            ApplicationUser storageUser = randomUser.DeepClone();
            Guid UserId = invalidUser.Id;
            invalidUser.CreatedDate = storageUser.CreatedDate.AddMinutes(randomNumber);


            var invalidUserException =
                 new InvalidUserException();

            invalidUserException.AddData(
            key: nameof(ApplicationUser.CreatedDate),
                values: $"Date is not the same as {nameof(ApplicationUser.CreatedDate)}");

            var expectedUserValidationException =
              new UserValidationException(invalidUserException);

            this.userManagementBrokerMock.Setup(broker =>
                broker.SelectUserByIdAsync(UserId))
                    .ReturnsAsync(storageUser);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            // when
            ValueTask<ApplicationUser> modifyUserTask =
                this.userService.ModifyUserRequestAsync(invalidUser);

            // then
            await Assert.ThrowsAsync<UserValidationException>(() =>
                modifyUserTask.AsTask());

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
            int randomNegativeMinutes = GetNegativeRandomNumber();
            int minutesInThePast = randomNegativeMinutes;
            DateTimeOffset randomDate = GetRandomDateTime();
            ApplicationUser randomUser = CreateRandomUser(randomDate);
            randomUser.CreatedDate = randomUser.CreatedDate.AddMinutes(minutesInThePast);
            ApplicationUser invalidUser = randomUser;
            invalidUser.UpdatedDate = randomDate;
            ApplicationUser storageUser = randomUser.DeepClone();
            Guid UserId = invalidUser.Id;


            var invalidUserException =
                 new InvalidUserException();

            invalidUserException.AddData(
            key: nameof(ApplicationUser.UpdatedDate),
                values: $"Date is not the same as {nameof(ApplicationUser.UpdatedDate)}");

            var expectedUserValidationException =
              new UserValidationException(invalidUserException);

            this.userManagementBrokerMock.Setup(broker =>
                broker.SelectUserByIdAsync(UserId))
                    .ReturnsAsync(storageUser);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            // when
            ValueTask<ApplicationUser> modifyUserTask =
                this.userService.ModifyUserRequestAsync(invalidUser);

            // then
            await Assert.ThrowsAsync<UserValidationException>(() =>
                modifyUserTask.AsTask());

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
