// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectors.Exceptions;
using Microsoft.Data.SqlClient;
using Moq;
using System;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.ProvidersDirectors
{
    public partial class ProvidersDirectorServiceTests
    {
        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
            SqlException sqlException = GetSqlException();

            var expectedFailedProvidersDirectorStorageException =
              new FailedProvidersDirectorStorageException(
                  message: "Failed ProvidersDirector storage error occurred, please contact support.",
                  sqlException);

            var expectedProvidersDirectorDependencyException =
                new ProvidersDirectorDependencyException(
                    message: "ProvidersDirector dependency error occurred, contact support.",
                    expectedFailedProvidersDirectorStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllProvidersDirectors())
                    .Throws(sqlException);

            // when
            Action retrieveAllProvidersDirectorsAction = () =>
                this.providersDirectorService.RetrieveAllProvidersDirectors();

            ProvidersDirectorDependencyException actualDependencyException =
              Assert.Throws<ProvidersDirectorDependencyException>(
                 retrieveAllProvidersDirectorsAction);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedProvidersDirectorDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllProvidersDirectors(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedProvidersDirectorDependencyException))),
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

            var failedProvidersDirectorServiceException =
              new FailedProvidersDirectorServiceException(
                  message: "Failed ProvidersDirector service error occurred, Please contact support.",
                  innerException: serviceException);

            var expectedProvidersDirectorServiceException =
                new ProvidersDirectorServiceException(
                    message: "ProvidersDirector service error occurred, contact support.",
                    innerException: failedProvidersDirectorServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllProvidersDirectors())
                    .Throws(serviceException);

            // when
            Action retrieveAllProvidersDirectorsAction = () =>
                this.providersDirectorService.RetrieveAllProvidersDirectors();

            ProvidersDirectorServiceException actualServiceException =
              Assert.Throws<ProvidersDirectorServiceException>(
                 retrieveAllProvidersDirectorsAction);

            // then
            actualServiceException.Should().BeEquivalentTo(
                expectedProvidersDirectorServiceException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProvidersDirectorServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllProvidersDirectors(),
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
