// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.ShortLets.Exceptions;
using Microsoft.Data.SqlClient;
using Moq;
using System;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.ShortLets
{
    public partial class ShortLetServiceTests
    {
        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
            SqlException sqlException = GetSqlException();

            var expectedFailedShortLetStorageException =
              new FailedShortLetStorageException(
                  message: "Failed ShortLet storage error occurred, please contact support.",
                  sqlException);

            var expectedShortLetDependencyException =
                new ShortLetDependencyException(
                    message: "ShortLet dependency error occurred, contact support.",
                    expectedFailedShortLetStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllShortLets())
                    .Throws(sqlException);

            // when
            Action retrieveAllShortLetsAction = () =>
                this.shortLetService.RetrieveAllShortLets();

            ShortLetDependencyException actualDependencyException =
              Assert.Throws<ShortLetDependencyException>(
                 retrieveAllShortLetsAction);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedShortLetDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllShortLets(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedShortLetDependencyException))),
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

            var failedShortLetServiceException =
              new FailedShortLetServiceException(
                  message: "Failed ShortLet service error occurred, contact support.",
                  innerException: serviceException);

            var expectedShortLetServiceException =
                new ShortLetServiceException(
                    message: "ShortLet service error occurred, contact support.",
                    innerException: failedShortLetServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllShortLets())
                    .Throws(serviceException);

            // when
            Action retrieveAllShortLetsAction = () =>
                this.shortLetService.RetrieveAllShortLets();

            ShortLetServiceException actualServiceException =
              Assert.Throws<ShortLetServiceException>(
                 retrieveAllShortLetsAction);

            // then
            actualServiceException.Should().BeEquivalentTo(
                expectedShortLetServiceException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShortLetServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllShortLets(),
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
