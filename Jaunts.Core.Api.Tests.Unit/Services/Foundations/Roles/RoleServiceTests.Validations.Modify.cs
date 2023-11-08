// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Force.DeepCloner;
using Jaunts.Core.Api.Models.Role.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Role;
using Moq;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Roles
{
    public partial class RoleServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyWhenRoleIsNullAndLogItAsync()
        {
            // given
            ApplicationRole randomRole = null;
            ApplicationRole nullRole = randomRole;
            var nullRoleException = new NullRoleException();

            var expectedRoleValidationException =
                new RoleValidationException(nullRoleException);

            // when
            ValueTask<ApplicationRole> modifyRoleTask =
                this.roleService.ModifyRoleRequestAsync(nullRole);

            // then
            await Assert.ThrowsAsync<RoleValidationException>(() =>
                modifyRoleTask.AsTask());

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
        public async void ShouldThrowValidationExceptionOnModifyWhenIdIsInvalidAndLogItAsync()
        {
            // given
            ApplicationRole randomRole = CreateRandomRole();
            ApplicationRole inputRole = randomRole;
            inputRole.Id = default;

            var invalidRoleException =
                 new InvalidRoleException();

            invalidRoleException.AddData(
                key: nameof(ApplicationRole.CreatedDate),
                values: "Id is required");

            var expectedRoleValidationException =
                new RoleValidationException(invalidRoleException);

            // when
            ValueTask<ApplicationRole> modifyRoleTask =
                this.roleService.ModifyRoleRequestAsync(inputRole);

            // then
            await Assert.ThrowsAsync<RoleValidationException>(() =>
                modifyRoleTask.AsTask());


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

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnModifyWhenRoleRoleNameIsInvalidAndLogItAsync(
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
            ValueTask<ApplicationRole> modifyRoleTask =
                this.roleService.ModifyRoleRequestAsync(invalidRole);

            // then
            await Assert.ThrowsAsync<RoleValidationException>(() =>
                modifyRoleTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRoleValidationException))),
                        Times.Once);


            this.dateTimeBrokerMock.Verify(broker =>
                 broker.GetCurrentDateTime(),
                 Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.roleManagementBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyWhenUpdatedDateIsInvalidAndLogItAsync()
        {
            // given
            ApplicationRole randomRole = CreateRandomRole();
            ApplicationRole inputRole = randomRole;
            inputRole.UpdatedDate = default;

            var invalidRoleException =
                new InvalidRoleException();

            invalidRoleException.AddData(
                key: nameof(ApplicationRole.UpdatedDate),
                values: "Date is required");

            var expectedRoleValidationException =
                new RoleValidationException(invalidRoleException);

            // when
            ValueTask<ApplicationRole> modifyRoleTask =
                this.roleService.ModifyRoleRequestAsync(inputRole);

            // then
            await Assert.ThrowsAsync<RoleValidationException>(() =>
                modifyRoleTask.AsTask());


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
        public async void ShouldThrowValidationExceptionOnModifyWhenCreatedDateIsInvalidAndLogItAsync()
        {
            // given
            ApplicationRole randomRole = CreateRandomRole();
            ApplicationRole inputRole = randomRole;
            inputRole.CreatedDate = default;

            var invalidRoleException =
                new InvalidRoleException();

            invalidRoleException.AddData(
                key: nameof(ApplicationRole.CreatedDate),
                values: "Date is required");

            var expectedRoleValidationException =
                new RoleValidationException(invalidRoleException);

            // when
            ValueTask<ApplicationRole> modifyRoleTask =
                this.roleService.ModifyRoleRequestAsync(inputRole);

            // then
            await Assert.ThrowsAsync<RoleValidationException>(() =>
                modifyRoleTask.AsTask());

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
        public async void ShouldThrowValidationExceptionOnModifyWhenUpdatedDateIsSameAsCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            ApplicationRole randomRole = CreateRandomRole(dateTime);
            ApplicationRole inputRole = randomRole;


            var invalidRoleException =
                 new InvalidRoleException();

            invalidRoleException.AddData(
            key: nameof(ApplicationRole.UpdatedDate),
                values: $"Date is not the same as {nameof(ApplicationRole.UpdatedDate)}");

            var expectedRoleValidationException =
                new RoleValidationException(invalidRoleException);

            // when
            ValueTask<ApplicationRole> modifyRoleTask =
                this.roleService.ModifyRoleRequestAsync(inputRole);

            // then
            await Assert.ThrowsAsync<RoleValidationException>(() =>
                modifyRoleTask.AsTask());


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

            this.roleManagementBrokerMock.Verify(broker =>
                broker.UpdateRoleAsync(It.IsAny<ApplicationRole>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.roleManagementBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidMinuteCases))]
        public async void ShouldThrowValidationExceptionOnModifyWhenUpdatedDateIsNotRecentAndLogItAsync(
            int minutes)
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            ApplicationRole randomRole = CreateRandomRole(dateTime);
            ApplicationRole inputRole = randomRole;
            inputRole.UpdatedDate = dateTime.AddMinutes(minutes);

            var invalidRoleException =
                   new InvalidRoleException();

            invalidRoleException.AddData(
                key: nameof(ApplicationRole.UpdatedDate),
                values: "Date is not recent");

            var expectedRoleValidationException =
                new RoleValidationException(invalidRoleException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<ApplicationRole> modifyRoleTask =
                this.roleService.ModifyRoleRequestAsync(inputRole);

            // then
            await Assert.ThrowsAsync<RoleValidationException>(() =>
                modifyRoleTask.AsTask());

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

            this.roleManagementBrokerMock.Verify(broker =>
                broker.UpdateRoleAsync(It.IsAny<ApplicationRole>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.roleManagementBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfRoleDoesntExistAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetNegativeRandomNumber();
            DateTimeOffset dateTime = GetRandomDateTime();
            ApplicationRole randomRole = CreateRandomRole(dateTime);
            ApplicationRole nonExistentRole = randomRole;
            nonExistentRole.CreatedDate = dateTime.AddMinutes(randomNegativeMinutes);
            ApplicationRole noRole = null;
            var notFoundRoleException = new NotFoundRoleException(nonExistentRole.Id);

            var expectedRoleValidationException =
                new RoleValidationException(notFoundRoleException);

            this.roleManagementBrokerMock.Setup(broker =>
                broker.SelectRoleByIdAsync(nonExistentRole.Id))
                    .ReturnsAsync(noRole);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<ApplicationRole> modifyRoleTask =
                this.roleService.ModifyRoleRequestAsync(nonExistentRole);

            // then
            await Assert.ThrowsAsync<RoleValidationException>(() =>
                modifyRoleTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.roleManagementBrokerMock.Verify(broker =>
                broker.SelectRoleByIdAsync(nonExistentRole.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRoleValidationException))),
                        Times.Once);

            this.roleManagementBrokerMock.Verify(broker =>
                broker.UpdateRoleAsync(It.IsAny<ApplicationRole>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.roleManagementBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageCreatedDateNotSameAsCreateDateAndLogItAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            int randomMinutes = randomNumber;
            DateTimeOffset randomDate = GetRandomDateTime();
            ApplicationRole randomRole = CreateRandomRole(randomDate);
            ApplicationRole invalidRole = randomRole;
            invalidRole.UpdatedDate = randomDate;
            ApplicationRole storageRole = randomRole.DeepClone();
            Guid RoleId = invalidRole.Id;
            invalidRole.CreatedDate = storageRole.CreatedDate.AddMinutes(randomNumber);


            var invalidRoleException =
                 new InvalidRoleException();

            invalidRoleException.AddData(
            key: nameof(ApplicationRole.CreatedDate),
                values: $"Date is not the same as {nameof(ApplicationRole.CreatedDate)}");

            var expectedRoleValidationException =
              new RoleValidationException(invalidRoleException);

            this.roleManagementBrokerMock.Setup(broker =>
                broker.SelectRoleByIdAsync(RoleId))
                    .ReturnsAsync(storageRole);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            // when
            ValueTask<ApplicationRole> modifyRoleTask =
                this.roleService.ModifyRoleRequestAsync(invalidRole);

            // then
            await Assert.ThrowsAsync<RoleValidationException>(() =>
                modifyRoleTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.roleManagementBrokerMock.Verify(broker =>
                broker.SelectRoleByIdAsync(invalidRole.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRoleValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.roleManagementBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageUpdatedDateSameAsUpdatedDateAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetNegativeRandomNumber();
            int minutesInThePast = randomNegativeMinutes;
            DateTimeOffset randomDate = GetRandomDateTime();
            ApplicationRole randomRole = CreateRandomRole(randomDate);
            randomRole.CreatedDate = randomRole.CreatedDate.AddMinutes(minutesInThePast);
            ApplicationRole invalidRole = randomRole;
            invalidRole.UpdatedDate = randomDate;
            ApplicationRole storageRole = randomRole.DeepClone();
            Guid RoleId = invalidRole.Id;


            var invalidRoleException =
                 new InvalidRoleException();

            invalidRoleException.AddData(
            key: nameof(ApplicationRole.UpdatedDate),
                values: $"Date is not the same as {nameof(ApplicationRole.UpdatedDate)}");

            var expectedRoleValidationException =
              new RoleValidationException(invalidRoleException);

            this.roleManagementBrokerMock.Setup(broker =>
                broker.SelectRoleByIdAsync(RoleId))
                    .ReturnsAsync(storageRole);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            // when
            ValueTask<ApplicationRole> modifyRoleTask =
                this.roleService.ModifyRoleRequestAsync(invalidRole);

            // then
            await Assert.ThrowsAsync<RoleValidationException>(() =>
                modifyRoleTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.roleManagementBrokerMock.Verify(broker =>
                broker.SelectRoleByIdAsync(invalidRole.Id),
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
