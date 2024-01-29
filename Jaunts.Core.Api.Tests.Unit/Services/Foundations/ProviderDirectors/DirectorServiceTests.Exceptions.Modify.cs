// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectors;
using Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectors.Exceptions;
using Microsoft.Data.SqlClient;
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
        public async Task ShouldThrowDependencyExceptionOnModifyIfSqlExceptionOccursAndLogItAsync()
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            ProvidersDirector someProvidersDirector = CreateRandomProvidersDirector(randomDateTime);
            someProvidersDirector.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            SqlException sqlException = GetSqlException();

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
            ValueTask<ProvidersDirector> modifyProvidersDirectorTask =
                this.providersDirectorService.ModifyProvidersDirectorAsync(someProvidersDirector);

                ProvidersDirectorDependencyException actualDependencyException =
                 await Assert.ThrowsAsync<ProvidersDirectorDependencyException>(
                     modifyProvidersDirectorTask.AsTask);

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
                broker.SelectProvidersDirectorByIdAsync(It.IsAny<Guid>()),
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
            ProvidersDirector someProvidersDirector = CreateRandomProvidersDirector(randomDateTime);
            someProvidersDirector.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
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
            ValueTask<ProvidersDirector> modifyProvidersDirectorTask =
                this.providersDirectorService.ModifyProvidersDirectorAsync(someProvidersDirector);

            ProvidersDirectorDependencyException actualDependencyException =
                await Assert.ThrowsAsync<ProvidersDirectorDependencyException>(
                    modifyProvidersDirectorTask.AsTask);

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
                broker.SelectProvidersDirectorByIdAsync(It.IsAny<Guid>()),
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
            ProvidersDirector randomProvidersDirector = CreateRandomProvidersDirector(randomDateTime);
            ProvidersDirector someProvidersDirector = randomProvidersDirector;
            someProvidersDirector.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedProvidersDirectorException = new LockedProvidersDirectorException(
                message: "Locked ProvidersDirector record exception, please try again later.",
                innerException: databaseUpdateConcurrencyException);

            var expectedProvidersDirectorDependencyException =
                new ProvidersDirectorDependencyException(
                    message: "ProvidersDirector dependency error occurred, contact support.",
                    innerException: lockedProvidersDirectorException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(databaseUpdateConcurrencyException);

            // when
            ValueTask<ProvidersDirector> modifyProvidersDirectorTask =
                this.providersDirectorService.ModifyProvidersDirectorAsync(someProvidersDirector);

            ProvidersDirectorDependencyException actualDependencyException =
             await Assert.ThrowsAsync<ProvidersDirectorDependencyException>(
                 modifyProvidersDirectorTask.AsTask);

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
                broker.SelectProvidersDirectorByIdAsync(It.IsAny<Guid>()),
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
            ProvidersDirector randomProvidersDirector = CreateRandomProvidersDirector(randomDateTime);
            ProvidersDirector someProvidersDirector = randomProvidersDirector;
            someProvidersDirector.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
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
            ValueTask<ProvidersDirector> modifyProvidersDirectorTask =
                this.providersDirectorService.ModifyProvidersDirectorAsync(someProvidersDirector);

            ProvidersDirectorServiceException actualServiceException =
             await Assert.ThrowsAsync<ProvidersDirectorServiceException>(
                 modifyProvidersDirectorTask.AsTask);

            // then
            actualServiceException.Should().BeEquivalentTo(
                expectedProvidersDirectorServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProvidersDirectorServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProvidersDirectorByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
