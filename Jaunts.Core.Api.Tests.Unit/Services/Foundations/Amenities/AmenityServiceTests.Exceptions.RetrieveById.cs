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
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAmenityId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var expectedFailedAmenityStorageException =
               new FailedAmenityStorageException(
                   message: "Failed Amenity storage error occurred, please contact support.",
                   sqlException);

            var expectedAmenityDependencyException =
                new AmenityDependencyException(
                    message: "Amenity dependency error occurred, contact support.",
                    expectedFailedAmenityStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAmenityByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Amenity> retrieveByIdAmenityTask =
                this.amenityService.RetrieveAmenityByIdAsync(someAmenityId);

            AmenityDependencyException actualDependencyException =
             await Assert.ThrowsAsync<AmenityDependencyException>(
                 retrieveByIdAmenityTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedAmenityDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAmenityByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedAmenityDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAmenityId = Guid.NewGuid();
            var databaseUpdateException = new DbUpdateException();

            var expectedFailedAmenityStorageException =
              new FailedAmenityStorageException(
                  message: "Failed Amenity storage error occurred, please contact support.",
                  databaseUpdateException);

            var expectedAmenityDependencyException =
                new AmenityDependencyException(
                    message: "Amenity dependency error occurred, contact support.",
                    expectedFailedAmenityStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAmenityByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<Amenity> retrieveByIdAmenityTask =
                this.amenityService.RetrieveAmenityByIdAsync(someAmenityId);

            AmenityDependencyException actualDependencyException =
             await Assert.ThrowsAsync<AmenityDependencyException>(
                 retrieveByIdAmenityTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedAmenityDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAmenityByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAmenityDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
            ShouldThrowDependencyExceptionOnRetrieveByIdWhenDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAmenityId = Guid.NewGuid();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();


            var lockedAmenityException = new LockedAmenityException(
                message: "Locked Amenity record exception, please try again later.",
                innerException: databaseUpdateConcurrencyException);

            var expectedAmenityDependencyException =
                new AmenityDependencyException(
                    message: "Amenity dependency error occurred, contact support.",
                    innerException: lockedAmenityException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAmenityByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<Amenity> retrieveByIdAmenityTask =
                this.amenityService.RetrieveAmenityByIdAsync(someAmenityId);

            AmenityDependencyException actualDependencyException =
             await Assert.ThrowsAsync<AmenityDependencyException>(
                 retrieveByIdAmenityTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedAmenityDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAmenityByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAmenityDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAmenityId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedAmenityServiceException =
                 new FailedAmenityServiceException(
                     message: "Failed Amenity service error occurred, Please contact support.",
                     innerException: serviceException);

            var expectedAmenityServiceException =
                new AmenityServiceException(
                    message: "Amenity service error occurred, contact support.",
                    innerException: failedAmenityServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAmenityByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<Amenity> retrieveByIdAmenityTask =
                this.amenityService.RetrieveAmenityByIdAsync(someAmenityId);

            AmenityServiceException actualServiceException =
                 await Assert.ThrowsAsync<AmenityServiceException>(
                     retrieveByIdAmenityTask.AsTask);

            // then
            actualServiceException.Should().BeEquivalentTo(
                expectedAmenityServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAmenityByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAmenityServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
