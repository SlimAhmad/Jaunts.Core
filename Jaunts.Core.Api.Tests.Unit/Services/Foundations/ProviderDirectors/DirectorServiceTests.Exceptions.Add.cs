// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectors;
using Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectors.Exceptions;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.ProvidersDirectors
{
    public partial class ProvidersDirectorServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnCreateWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            ProvidersDirector someProvidersDirector = CreateRandomProvidersDirector(dateTime);
            someProvidersDirector.UpdatedBy = someProvidersDirector.CreatedBy;
            var sqlException = GetSqlException();

            var expectedFailedProvidersDirectorStorageException =
                new FailedProvidersDirectorStorageException(
                    message: "Failed ProvidersDirector storage error occurred, please contact support.",
                    innerException: sqlException);

            var expectedProvidersDirectorDependencyException =
                new ProvidersDirectorDependencyException(
                    message: "ProvidersDirector dependency error occurred, contact support.",
                    innerException: expectedFailedProvidersDirectorStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(sqlException);

            // when
            ValueTask<ProvidersDirector> createProvidersDirectorTask =
                this.providersDirectorService.CreateProvidersDirectorAsync(someProvidersDirector);

            ProvidersDirectorDependencyException actualDependencyException =
             await Assert.ThrowsAsync<ProvidersDirectorDependencyException>(
                 createProvidersDirectorTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedProvidersDirectorDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedProvidersDirectorDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertProvidersDirectorAsync(It.IsAny<ProvidersDirector>()),
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
            ProvidersDirector someProvidersDirector = CreateRandomProvidersDirector(dateTime);
            someProvidersDirector.UpdatedBy = someProvidersDirector.CreatedBy;
            var databaseUpdateException = new DbUpdateException();

            var expectedFailedProvidersDirectorStorageException =
                new FailedProvidersDirectorStorageException(
                    message: "Failed ProvidersDirector storage error occurred, please contact support.",
                    databaseUpdateException);

            var expectedProvidersDirectorDependencyException =
                new ProvidersDirectorDependencyException(
                    message: "ProvidersDirector dependency error occurred, contact support.",
                    expectedFailedProvidersDirectorStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(databaseUpdateException);

            // when
            ValueTask<ProvidersDirector> createProvidersDirectorTask =
                this.providersDirectorService.CreateProvidersDirectorAsync(someProvidersDirector);

            ProvidersDirectorDependencyException actualDependencyException =
                 await Assert.ThrowsAsync<ProvidersDirectorDependencyException>(
                     createProvidersDirectorTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedProvidersDirectorDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProvidersDirectorDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertProvidersDirectorAsync(It.IsAny<ProvidersDirector>()),
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
            ProvidersDirector someProvidersDirector = CreateRandomProvidersDirector(dateTime);
            someProvidersDirector.UpdatedBy = someProvidersDirector.CreatedBy;
            var serviceException = new Exception();

            var failedProvidersDirectorServiceException =
                new FailedProvidersDirectorServiceException(
                    message: "Failed ProvidersDirector service error occurred, Please contact support.",
                    innerException: serviceException);

            var expectedProvidersDirectorServiceException =
                new ProvidersDirectorServiceException(
                    message: "ProvidersDirector service error occurred, contact support.",
                    innerException: failedProvidersDirectorServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(serviceException);

            // when
            ValueTask<ProvidersDirector> createProvidersDirectorTask =
                 this.providersDirectorService.CreateProvidersDirectorAsync(someProvidersDirector);

            ProvidersDirectorServiceException actualDependencyException =
                 await Assert.ThrowsAsync<ProvidersDirectorServiceException>(
                     createProvidersDirectorTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedProvidersDirectorServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProvidersDirectorServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertProvidersDirectorAsync(It.IsAny<ProvidersDirector>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
