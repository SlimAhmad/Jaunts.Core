// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.ProviderServices;
using Jaunts.Core.Api.Models.Services.Foundations.ProviderServices.Exceptions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.ProviderServices
{
    public partial class ProviderServiceServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenProviderServiceIsNullAndLogItAsync()
        {
            // given
            ProviderService randomProviderService = null;
            ProviderService nullProviderService = randomProviderService;

            var nullProviderServiceException = new NullProviderServiceException(
                message: "The ProviderService is null.");

            var expectedProviderServiceValidationException =
                new ProviderServiceValidationException(
                    message: "ProviderService validation error occurred, Please try again.",
                    innerException: nullProviderServiceException);

            // when
            ValueTask<ProviderService> createProviderServiceTask =
                this.providerServiceService.CreateProviderServiceAsync(nullProviderService);

             ProviderServiceValidationException actualProviderServiceDependencyValidationException =
             await Assert.ThrowsAsync<ProviderServiceValidationException>(
                 createProviderServiceTask.AsTask);

            // then
            actualProviderServiceDependencyValidationException.Should().BeEquivalentTo(
                expectedProviderServiceValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderServiceValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProviderServiceByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public async void ShouldThrowValidationExceptionOnCreateWhenProviderServiceIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            var invalidProviderService = new ProviderService
            {
                Description = invalidText,
                ServiceName = invalidText,

            };

            var invalidProviderServiceException = new InvalidProviderServiceException();

            invalidProviderServiceException.AddData(
                key: nameof(ProviderService.Id),
                values: "Id is required");

            invalidProviderServiceException.AddData(
                key: nameof(ProviderService.ProviderId),
                values: "Id is required");

            invalidProviderServiceException.AddData(
                key: nameof(ProviderService.ServiceName),
                values: "Text is required");

            invalidProviderServiceException.AddData(
                key: nameof(ProviderService.Description),
                values: "Text is required");

            invalidProviderServiceException.AddData(
                key: nameof(ProviderService.CreatedBy),
                values: "Id is required");

            invalidProviderServiceException.AddData(
                key: nameof(ProviderService.UpdatedBy),
                values: "Id is required");

            invalidProviderServiceException.AddData(
                key: nameof(ProviderService.CreatedDate),
                values: "Date is required");

            invalidProviderServiceException.AddData(
                key: nameof(ProviderService.UpdatedDate),
                values: "Date is required");


            var expectedProviderServiceValidationException =
                new ProviderServiceValidationException(
                    message: "ProviderService validation error occurred, Please try again.",
                    innerException: invalidProviderServiceException);

            // when
            ValueTask<ProviderService> createProviderServiceTask =
                this.providerServiceService.CreateProviderServiceAsync(invalidProviderService);

             ProviderServiceValidationException actualProviderServiceDependencyValidationException =
             await Assert.ThrowsAsync<ProviderServiceValidationException>(
                 createProviderServiceTask.AsTask);

            // then
            actualProviderServiceDependencyValidationException.Should().BeEquivalentTo(
                expectedProviderServiceValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedProviderServiceValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProviderServiceByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenUpdatedByIsNotSameToCreatedByAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetCurrentDateTime();
            ProviderService randomProviderService = CreateRandomProviderService(dateTime);
            ProviderService inputProviderService = randomProviderService;
            inputProviderService.UpdatedBy = Guid.NewGuid();

            var invalidProviderServiceException = new InvalidProviderServiceException();

            invalidProviderServiceException.AddData(
                key: nameof(ProviderService.UpdatedBy),
                values: $"Id is not the same as {nameof(ProviderService.CreatedBy)}");

            var expectedProviderServiceValidationException =
                new ProviderServiceValidationException(
                    message: "ProviderService validation error occurred, Please try again.",
                    innerException: invalidProviderServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<ProviderService> createProviderServiceTask =
                this.providerServiceService.CreateProviderServiceAsync(inputProviderService);

             ProviderServiceValidationException actualProviderServiceDependencyValidationException =
             await Assert.ThrowsAsync<ProviderServiceValidationException>(
                 createProviderServiceTask.AsTask);

            // then
            actualProviderServiceDependencyValidationException.Should().BeEquivalentTo(
                expectedProviderServiceValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderServiceValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProviderServiceByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenUpdatedDateIsNotSameToCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            ProviderService randomProviderService = CreateRandomProviderService(dateTime);
            ProviderService inputProviderService = randomProviderService;
            inputProviderService.UpdatedBy = randomProviderService.CreatedBy;
            inputProviderService.UpdatedDate = GetRandomDateTime();

            var invalidProviderServiceException = new InvalidProviderServiceException();

            invalidProviderServiceException.AddData(
                key: nameof(ProviderService.UpdatedDate),
                values: $"Date is not the same as {nameof(ProviderService.CreatedDate)}");

            var expectedProviderServiceValidationException =
                new ProviderServiceValidationException(
                    message: "ProviderService validation error occurred, Please try again.",
                    innerException: invalidProviderServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
             broker.GetCurrentDateTime())
                 .Returns(dateTime);

            // when
            ValueTask<ProviderService> createProviderServiceTask =
                this.providerServiceService.CreateProviderServiceAsync(inputProviderService);

             ProviderServiceValidationException actualProviderServiceDependencyValidationException =
             await Assert.ThrowsAsync<ProviderServiceValidationException>(
                 createProviderServiceTask.AsTask);

            // then
            actualProviderServiceDependencyValidationException.Should().BeEquivalentTo(
                expectedProviderServiceValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderServiceValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProviderServiceByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidMinuteCases))]
        public async void ShouldThrowValidationExceptionOnCreateWhenCreatedDateIsNotRecentAndLogItAsync(
            int minutes)
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            ProviderService randomProviderService = CreateRandomProviderService(dateTime);
            ProviderService inputProviderService = randomProviderService;
            inputProviderService.UpdatedBy = inputProviderService.CreatedBy;
            inputProviderService.CreatedDate = dateTime.AddMinutes(minutes);
            inputProviderService.UpdatedDate = inputProviderService.CreatedDate;

            var invalidProviderServiceException = new InvalidProviderServiceException();

            invalidProviderServiceException.AddData(
                key: nameof(ProviderService.CreatedDate),
                values: $"Date is not recent");

            var expectedProviderServiceValidationException =
                new ProviderServiceValidationException(
                    message: "ProviderService validation error occurred, Please try again.",
                    innerException: invalidProviderServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<ProviderService> createProviderServiceTask =
                this.providerServiceService.CreateProviderServiceAsync(inputProviderService);

             ProviderServiceValidationException actualProviderServiceDependencyValidationException =
             await Assert.ThrowsAsync<ProviderServiceValidationException>(
                 createProviderServiceTask.AsTask);

            // then
            actualProviderServiceDependencyValidationException.Should().BeEquivalentTo(
                expectedProviderServiceValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderServiceValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProviderServiceByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenProviderServiceAlreadyExistsAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            ProviderService randomProviderService = CreateRandomProviderService(dateTime);
            ProviderService alreadyExistsProviderService = randomProviderService;
            alreadyExistsProviderService.UpdatedBy = alreadyExistsProviderService.CreatedBy;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var duplicateKeyException = new DuplicateKeyException(exceptionMessage);

            var alreadyExistsProviderServiceException =
                new AlreadyExistsProviderServiceException(
                   message: "ProviderService with the same id already exists.",
                   innerException: duplicateKeyException);

            var expectedProviderServiceValidationException =
                new ProviderServiceDependencyValidationException(
                    message: "ProviderService dependency validation error occurred, fix the errors.",
                    innerException: alreadyExistsProviderServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertProviderServiceAsync(alreadyExistsProviderService))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<ProviderService> createProviderServiceTask =
                this.providerServiceService.CreateProviderServiceAsync(alreadyExistsProviderService);

             ProviderServiceDependencyValidationException actualProviderServiceDependencyValidationException =
             await Assert.ThrowsAsync<ProviderServiceDependencyValidationException>(
                 createProviderServiceTask.AsTask);

            // then
            actualProviderServiceDependencyValidationException.Should().BeEquivalentTo(
                expectedProviderServiceValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedProviderServiceValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertProviderServiceAsync(alreadyExistsProviderService),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
