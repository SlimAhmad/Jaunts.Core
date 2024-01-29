// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.ProviderServices.Exceptions;
using Microsoft.Data.SqlClient;
using Moq;
using System;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.ProviderServices
{
    public partial class ProviderServiceServiceTests
    {
        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
            SqlException sqlException = GetSqlException();

            var expectedFailedProviderServiceStorageException =
              new FailedProviderServiceStorageException(
                  message: "Failed ProviderService storage error occurred, Please contact support.",
                  sqlException);

            var expectedProviderServiceDependencyException =
                new ProviderServiceDependencyException(
                    message: "ProviderService dependency error occurred, contact support.",
                    expectedFailedProviderServiceStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllProviderServices())
                    .Throws(sqlException);

            // when
            Action retrieveAllProviderServicesAction = () =>
                this.providerServiceService.RetrieveAllProviderServices();

            ProviderServiceDependencyException actualDependencyException =
              Assert.Throws<ProviderServiceDependencyException>(
                 retrieveAllProviderServicesAction);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedProviderServiceDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllProviderServices(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedProviderServiceDependencyException))),
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

            var failedProviderServiceServiceException =
              new FailedProviderServiceServiceException(
                  message: "Failed ProviderService service error occurred, contact support.",
                  innerException: serviceException);

            var expectedProviderServiceServiceException =
                new ProviderServiceServiceException(
                    message: "ProviderService service error occurred, contact support.",
                    innerException: failedProviderServiceServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllProviderServices())
                    .Throws(serviceException);

            // when
            Action retrieveAllProviderServicesAction = () =>
                this.providerServiceService.RetrieveAllProviderServices();

            ProviderServiceServiceException actualServiceException =
              Assert.Throws<ProviderServiceServiceException>(
                 retrieveAllProviderServicesAction);

            // then
            actualServiceException.Should().BeEquivalentTo(
                expectedProviderServiceServiceException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderServiceServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllProviderServices(),
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
