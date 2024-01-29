// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Amenities;
using Jaunts.Core.Api.Models.Services.Foundations.Amenitys;
using Jaunts.Core.Api.Models.Services.Foundations.Amenitys.Exceptions;
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
        public async Task ShouldThrowDependencyExceptionOnCreateWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Amenity someAmenity = CreateRandomAmenity(dateTime);
            someAmenity.UpdatedBy = someAmenity.CreatedBy;
            var sqlException = GetSqlException();

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
            ValueTask<Amenity> createAmenityTask =
                this.amenityService.CreateAmenityAsync(someAmenity);

            AmenityDependencyException actualDependencyException =
             await Assert.ThrowsAsync<AmenityDependencyException>(
                 createAmenityTask.AsTask);

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
                broker.InsertAmenityAsync(It.IsAny<Amenity>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnCreateWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Amenity someAmenity = CreateRandomAmenity(dateTime);
            someAmenity.UpdatedBy = someAmenity.CreatedBy;
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
            ValueTask<Amenity> createAmenityTask =
                this.amenityService.CreateAmenityAsync(someAmenity);

            AmenityDependencyException actualDependencyException =
                 await Assert.ThrowsAsync<AmenityDependencyException>(
                     createAmenityTask.AsTask);

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
                broker.InsertAmenityAsync(It.IsAny<Amenity>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnCreateWhenExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Amenity someAmenity = CreateRandomAmenity(dateTime);
            someAmenity.UpdatedBy = someAmenity.CreatedBy;
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
            ValueTask<Amenity> createAmenityTask =
                 this.amenityService.CreateAmenityAsync(someAmenity);

            AmenityServiceException actualDependencyException =
                 await Assert.ThrowsAsync<AmenityServiceException>(
                     createAmenityTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedAmenityServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAmenityServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAmenityAsync(It.IsAny<Amenity>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
