// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Providers.Exceptions;
using Microsoft.Data.SqlClient;
using Moq;
using System;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Providers
{
    public partial class ProviderServiceTests
    {
        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
            SqlException sqlException = GetSqlException();

            var expectedFailedProviderStorageException =
              new FailedProviderStorageException(
                  message: "Failed Provider storage error occurred, please contact support.",
                  sqlException);

            var expectedProviderDependencyException =
                new ProviderDependencyException(
                    message: "Provider dependency error occurred, contact support.",
                    expectedFailedProviderStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllProviders())
                    .Throws(sqlException);

            // when
            Action retrieveAllProvidersAction = () =>
                this.providerService.RetrieveAllProviders();

            ProviderDependencyException actualDependencyException =
              Assert.Throws<ProviderDependencyException>(
                 retrieveAllProvidersAction);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedProviderDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllProviders(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedProviderDependencyException))),
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

            var failedProviderServiceException =
              new FailedProviderServiceException(
                  message: "Failed Provider service error occurred, contact support.",
                  innerException: serviceException);

            var expectedProviderServiceException =
                new ProviderServiceException(
                    message: "Provider service error occurred, contact support.",
                    innerException: failedProviderServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllProviders())
                    .Throws(serviceException);

            // when
            Action retrieveAllProvidersAction = () =>
                this.providerService.RetrieveAllProviders();

            ProviderServiceException actualServiceException =
              Assert.Throws<ProviderServiceException>(
                 retrieveAllProvidersAction);

            // then
            actualServiceException.Should().BeEquivalentTo(
                expectedProviderServiceException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllProviders(),
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
