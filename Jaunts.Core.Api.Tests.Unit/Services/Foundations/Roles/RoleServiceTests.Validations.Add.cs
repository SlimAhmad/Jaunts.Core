// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using EFxceptions.Models.Exceptions;
using Jaunts.Core.Api.Models.Role.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Role;
using Moq;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Roles
{
    public partial class RoleServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenRoleIsNullAndLogItAsync()
        {
            // given
            ApplicationRole invalidRole = null;

            var nullRoleException = new NullRoleException();
            string password = GetRandomPassword();

            var expectedRoleValidationException =
                new RoleValidationException(nullRoleException);

            // when
            ValueTask<ApplicationRole> createRoleTask =
                this.roleService.RegisterRoleRequestAsync(invalidRole);

            // then
            await Assert.ThrowsAsync<RoleValidationException>(() =>
                createRoleTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRoleValidationException))),
                        Times.Once);

            this.roleManagementBrokerMock.Verify(broker =>
                broker.SelectRoleByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.roleManagementBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenIdIsInvalidAndLogItAsync()
        {
            // given
            ApplicationRole randomRole = CreateRandomRole();
            ApplicationRole inputRole = randomRole;
            inputRole.Id = default;
            string password = GetRandomPassword();

            var invalidRoleInputException = new InvalidRoleException();

            var expectedRoleValidationException =
                new RoleValidationException(invalidRoleInputException);

            // when
            ValueTask<ApplicationRole> registerRoleTask =
                this.roleService.RegisterRoleRequestAsync(inputRole);

            // then
            await Assert.ThrowsAsync<RoleValidationException>(() =>
                registerRoleTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
               broker.GetCurrentDateTime(),
                   Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRoleValidationException))),
                        Times.Once);

            this.roleManagementBrokerMock.Verify(broker =>
                broker.SelectRoleByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.roleManagementBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnCreateWhenRoleRoleNameIsInvalidAndLogItAsync(
            string invalidRoleRoleName)
        {
            // given
            ApplicationRole randomRole = CreateRandomRole();
            ApplicationRole invalidRole = randomRole;
            invalidRole.Name = invalidRoleRoleName;
     


            var invalidRoleException = new InvalidRoleException();

            invalidRoleException.AddData(
                key: nameof(ApplicationRole.Name),
                values: "Text is required");

    

            invalidRoleException.AddData(
               key: nameof(ApplicationRole.CreatedDate),
               values: "Date is required");

            invalidRoleException.AddData(
               key: nameof(ApplicationRole.UpdatedDate),
               values: "Date is required");

            var expectedRoleValidationException =
                new RoleValidationException(invalidRoleException);

            // when
            ValueTask<ApplicationRole> registerRoleTask =
                this.roleService.RegisterRoleRequestAsync(invalidRole);

            // then
            await Assert.ThrowsAsync<RoleValidationException>(() =>
                registerRoleTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRoleValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
               broker.GetCurrentDateTime(),
                   Times.Once());

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.roleManagementBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

      

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenCreatedDateIsInvalidAndLogItAsync()
        {
            // given
            ApplicationRole randomRole = CreateRandomRole();
            ApplicationRole inputRole = randomRole;
            inputRole.CreatedDate = default;
            string password = GetRandomPassword();

            var invalidRoleException = new InvalidRoleException();

            invalidRoleException.AddData(
                key: nameof(ApplicationRole.Name),
                values: "Text is required");

            invalidRoleException.AddData(
               key: nameof(ApplicationRole.CreatedDate),
               values: "Date is required");

            invalidRoleException.AddData(
               key: nameof(ApplicationRole.UpdatedDate),
               values: "Date is required");

            var expectedRoleValidationException =
                new RoleValidationException(invalidRoleException);

            // when
            ValueTask<ApplicationRole> registerRoleTask =
                this.roleService.RegisterRoleRequestAsync(inputRole);

            // then
            await Assert.ThrowsAsync<RoleValidationException>(() =>
                registerRoleTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
               broker.GetCurrentDateTime(),
                   Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRoleValidationException))),
                        Times.Once);

            this.roleManagementBrokerMock.Verify(broker =>
                broker.SelectRoleByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.roleManagementBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenUpdatedDateIsInvalidAndLogItAsync()
        {
            // given
            ApplicationRole randomRole = CreateRandomRole();
            ApplicationRole inputRole = randomRole;
            inputRole.UpdatedDate = default;
            string password = GetRandomPassword();

            var invalidRoleException = new InvalidRoleException();

            invalidRoleException.AddData(
                key: nameof(ApplicationRole.Name),
                values: "Text is required");

            invalidRoleException.AddData(
               key: nameof(ApplicationRole.CreatedDate),
               values: "Date is required");

            invalidRoleException.AddData(
               key: nameof(ApplicationRole.UpdatedDate),
               values: "Date is required");

            var expectedRoleValidationException =
                new RoleValidationException(invalidRoleException);

            // when
            ValueTask<ApplicationRole> registerRoleTask =
                this.roleService.RegisterRoleRequestAsync(inputRole);

            // then
            await Assert.ThrowsAsync<RoleValidationException>(() =>
                registerRoleTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRoleValidationException))),
                        Times.Once);

            this.roleManagementBrokerMock.Verify(broker =>
                broker.SelectRoleByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.roleManagementBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenUpdatedDateIsNotSameToCreatedDateAndLogItAsync()
        {
            // given
            ApplicationRole randomRole = CreateRandomRole();
            ApplicationRole inputRole = randomRole;
            inputRole.UpdatedDate = GetRandomDateTime();
            string password = GetRandomPassword();

            var invalidRoleException =
                 new InvalidRoleException();

            invalidRoleException.AddData(
            key: nameof(ApplicationRole.UpdatedDate),
                values: $"Date is not the same as {nameof(ApplicationRole.CreatedDate)}");

            var expectedRoleValidationException =
                new RoleValidationException(invalidRoleException);

            // when
            ValueTask<ApplicationRole> registerRoleTask =
                this.roleService.RegisterRoleRequestAsync(inputRole);

            // then
            await Assert.ThrowsAsync<RoleValidationException>(() =>
                registerRoleTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
               broker.GetCurrentDateTime(),
                   Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRoleValidationException))),
                        Times.Once);

            this.roleManagementBrokerMock.Verify(broker =>
                broker.SelectRoleByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.roleManagementBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidMinuteCases))]
        public async void ShouldThrowValidationExceptionOnCreateWhenCreatedDateIsNotRecentAndLogItAsync(
            int minutes)
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            ApplicationRole randomRole = CreateRandomRole(dates: dateTime);
            ApplicationRole inputRole = randomRole;
            inputRole.CreatedDate = dateTime.AddMinutes(minutes);
            inputRole.UpdatedDate = inputRole.CreatedDate;
            string password = GetRandomPassword();

            var invalidRoleException =
                 new InvalidRoleException();

            invalidRoleException.AddData(
                key: nameof(ApplicationRole.CreatedDate),
                values: "Date is not recent");

            var expectedRoleValidationException =
                new RoleValidationException(invalidRoleException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<ApplicationRole> registerRoleTask =
                this.roleService.RegisterRoleRequestAsync(inputRole);

            // then
            await Assert.ThrowsAsync<RoleValidationException>(() =>
                registerRoleTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRoleValidationException))),
                        Times.Once);

            this.roleManagementBrokerMock.Verify(broker =>
                broker.SelectRoleByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.roleManagementBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenRoleAlreadyExistsAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            DateTimeOffset dateTime = randomDateTime;
            ApplicationRole randomRole = CreateRandomRole(dateTime);
            ApplicationRole alreadyExistsRole = randomRole;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var duplicateKeyException = new DuplicateKeyException(exceptionMessage);
            string password = GetRandomPassword();

            var alreadyExistsRoleException =
                new AlreadyExistsRoleException(duplicateKeyException);

            var expectedRoleValidationException =
                new RoleDependencyValidationException(alreadyExistsRoleException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.roleManagementBrokerMock.Setup(broker =>
                broker.InsertRoleAsync(alreadyExistsRole))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<ApplicationRole> registerRoleTask =
                this.roleService.RegisterRoleRequestAsync(alreadyExistsRole);

            // then
            await Assert.ThrowsAsync<RoleDependencyValidationException>(() =>
                registerRoleTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.roleManagementBrokerMock.Verify(broker =>
                broker.InsertRoleAsync(alreadyExistsRole),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedRoleValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.roleManagementBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
