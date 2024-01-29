// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.ShortLets;
using Jaunts.Core.Api.Models.Services.Foundations.ShortLets.Exceptions;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.ShortLets
{
    public partial class ShortLetServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnCreateWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            ShortLet someShortLet = CreateRandomShortLet(dateTime);
            someShortLet.UpdatedBy = someShortLet.CreatedBy;
            var sqlException = GetSqlException();

            var expectedFailedShortLetStorageException =
                new FailedShortLetStorageException(
                    message: "Failed ShortLet storage error occurred, please contact support.",
                    innerException: sqlException);

            var expectedShortLetDependencyException =
                new ShortLetDependencyException(
                    message: "ShortLet dependency error occurred, contact support.",
                    innerException: expectedFailedShortLetStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(sqlException);

            // when
            ValueTask<ShortLet> createShortLetTask =
                this.shortLetService.CreateShortLetAsync(someShortLet);

            ShortLetDependencyException actualDependencyException =
             await Assert.ThrowsAsync<ShortLetDependencyException>(
                 createShortLetTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedShortLetDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedShortLetDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertShortLetAsync(It.IsAny<ShortLet>()),
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
            ShortLet someShortLet = CreateRandomShortLet(dateTime);
            someShortLet.UpdatedBy = someShortLet.CreatedBy;
            var databaseUpdateException = new DbUpdateException();

            var expectedFailedShortLetStorageException =
                new FailedShortLetStorageException(
                    message: "Failed ShortLet storage error occurred, please contact support.",
                    databaseUpdateException);

            var expectedShortLetDependencyException =
                new ShortLetDependencyException(
                    message: "ShortLet dependency error occurred, contact support.",
                    expectedFailedShortLetStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(databaseUpdateException);

            // when
            ValueTask<ShortLet> createShortLetTask =
                this.shortLetService.CreateShortLetAsync(someShortLet);

            ShortLetDependencyException actualDependencyException =
                 await Assert.ThrowsAsync<ShortLetDependencyException>(
                     createShortLetTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedShortLetDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShortLetDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertShortLetAsync(It.IsAny<ShortLet>()),
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
            ShortLet someShortLet = CreateRandomShortLet(dateTime);
            someShortLet.UpdatedBy = someShortLet.CreatedBy;
            var serviceException = new Exception();

            var failedShortLetServiceException =
                new FailedShortLetServiceException(
                    message: "Failed ShortLet service error occurred, contact support.",
                    innerException: serviceException);

            var expectedShortLetServiceException =
                new ShortLetServiceException(
                    message: "ShortLet service error occurred, contact support.",
                    innerException: failedShortLetServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(serviceException);

            // when
            ValueTask<ShortLet> createShortLetTask =
                 this.shortLetService.CreateShortLetAsync(someShortLet);

            ShortLetServiceException actualDependencyException =
                 await Assert.ThrowsAsync<ShortLetServiceException>(
                     createShortLetTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedShortLetServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShortLetServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertShortLetAsync(It.IsAny<ShortLet>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
