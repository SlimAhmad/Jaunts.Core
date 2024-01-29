// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Amenitys.Exceptions;
using Microsoft.Data.SqlClient;
using Moq;
using System;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Amenities
{
    public partial class AmenityServiceTests
    {
        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
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
                broker.SelectAllAmenities())
                    .Throws(sqlException);

            // when
            Action retrieveAllAmenitysAction = () =>
                this.amenityService.RetrieveAllAmenities();

            AmenityDependencyException actualDependencyException =
              Assert.Throws<AmenityDependencyException>(
                 retrieveAllAmenitysAction);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedAmenityDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllAmenities(),
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
        public void ShouldThrowServiceExceptionOnRetrieveAllWhenExceptionOccursAndLogIt()
        {
            // given
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
                broker.SelectAllAmenities())
                    .Throws(serviceException);

            // when
            Action retrieveAllAmenitysAction = () =>
                this.amenityService.RetrieveAllAmenities();

            AmenityServiceException actualServiceException =
              Assert.Throws<AmenityServiceException>(
                 retrieveAllAmenitysAction);

            // then
            actualServiceException.Should().BeEquivalentTo(
                expectedAmenityServiceException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAmenityServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllAmenities(),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
