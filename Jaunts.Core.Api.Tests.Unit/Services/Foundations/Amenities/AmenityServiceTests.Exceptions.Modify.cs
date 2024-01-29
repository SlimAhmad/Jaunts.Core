// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Amenities;
using Jaunts.Core.Api.Models.Services.Foundations.Amenitys;
using Jaunts.Core.Api.Models.Services.Foundations.Amenitys.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Amenities
{
    public partial class AmenityServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfSqlExceptionOccursAndLogItAsync()
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Amenity someAmenity = CreateRandomAmenity(randomDateTime);
            someAmenity.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            SqlException sqlException = GetSqlException();

            var expectedFailedAmenityStorageException =
              new FailedAmenityStorageException(
                  message: "Failed Amenity storage error occurred, please contact support.",
                  innerException: sqlException);

            var expectedAmenityDependencyException =
                new AmenityDependencyException(
                    message: "Amenity dependency error occurred, contact support.",
                    innerException: expectedFailedAmenityStorageException);


            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(sqlException);

            // when
            ValueTask<Amenity> modifyAmenityTask =
                this.amenityService.ModifyAmenityAsync(someAmenity);

                AmenityDependencyException actualDependencyException =
                 await Assert.ThrowsAsync<AmenityDependencyException>(
                     modifyAmenityTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedAmenityDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedAmenityDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAmenityByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDbUpdateExceptionOccursAndLogItAsync()
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Amenity someAmenity = CreateRandomAmenity(randomDateTime);
            someAmenity.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            var databaseUpdateException = new DbUpdateException();

            var expectedFailedAmenityStorageException =
              new FailedAmenityStorageException(
                  message: "Failed Amenity storage error occurred, please contact support.",
                  databaseUpdateException);

            var expectedAmenityDependencyException =
                new AmenityDependencyException(
                    message: "Amenity dependency error occurred, contact support.",
                    expectedFailedAmenityStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(databaseUpdateException);

            // when
            ValueTask<Amenity> modifyAmenityTask =
                this.amenityService.ModifyAmenityAsync(someAmenity);

            AmenityDependencyException actualDependencyException =
                await Assert.ThrowsAsync<AmenityDependencyException>(
                    modifyAmenityTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedAmenityDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAmenityDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAmenityByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Amenity randomAmenity = CreateRandomAmenity(randomDateTime);
            Amenity someAmenity = randomAmenity;
            someAmenity.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedAmenityException = new LockedAmenityException(
                message: "Locked Amenity record exception, please try again later.",
                innerException: databaseUpdateConcurrencyException);

            var expectedAmenityDependencyException =
                new AmenityDependencyException(
                    message: "Amenity dependency error occurred, contact support.",
                    innerException: lockedAmenityException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(databaseUpdateConcurrencyException);

            // when
            ValueTask<Amenity> modifyAmenityTask =
                this.amenityService.ModifyAmenityAsync(someAmenity);

            AmenityDependencyException actualDependencyException =
             await Assert.ThrowsAsync<AmenityDependencyException>(
                 modifyAmenityTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedAmenityDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAmenityDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAmenityByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnModifyIfServiceExceptionOccursAndLogItAsync()
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Amenity randomAmenity = CreateRandomAmenity(randomDateTime);
            Amenity someAmenity = randomAmenity;
            someAmenity.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            var serviceException = new Exception();

            var failedAmenityServiceException =
             new FailedAmenityServiceException(
                 message: "Failed Amenity service error occurred, Please contact support.",
                 innerException: serviceException);

            var expectedAmenityServiceException =
                new AmenityServiceException(
                    message: "Amenity service error occurred, contact support.",
                    innerException: failedAmenityServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(serviceException);

            // when
            ValueTask<Amenity> modifyAmenityTask =
                this.amenityService.ModifyAmenityAsync(someAmenity);

            AmenityServiceException actualServiceException =
             await Assert.ThrowsAsync<AmenityServiceException>(
                 modifyAmenityTask.AsTask);

            // then
            actualServiceException.Should().BeEquivalentTo(
                expectedAmenityServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAmenityServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAmenityByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
