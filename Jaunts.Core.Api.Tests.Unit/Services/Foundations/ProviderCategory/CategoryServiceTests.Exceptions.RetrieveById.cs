// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.ProviderCategory;
using Jaunts.Core.Api.Models.Services.Foundations.ProviderCategorys;
using Jaunts.Core.Api.Models.Services.Foundations.ProviderCategorys.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.ProviderCategories
{
    public partial class ProviderCategoryServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someProviderCategoryId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var expectedFailedProviderCategoryStorageException =
               new FailedProviderCategoryStorageException(
                   message: "Failed ProviderCategory storage error occurred, Please contact support.",
                   sqlException);

            var expectedProviderCategoryDependencyException =
                new ProviderCategoryDependencyException(
                    message: "ProviderCategory dependency error occurred, contact support.",
                    expectedFailedProviderCategoryStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProviderCategoryByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<ProviderCategory> retrieveByIdProviderCategoryTask =
                this.providerCategoryService.RetrieveProviderCategoryByIdAsync(someProviderCategoryId);

            ProviderCategoryDependencyException actualDependencyException =
             await Assert.ThrowsAsync<ProviderCategoryDependencyException>(
                 retrieveByIdProviderCategoryTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedProviderCategoryDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProviderCategoryByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedProviderCategoryDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            Guid someProviderCategoryId = Guid.NewGuid();
            var databaseUpdateException = new DbUpdateException();

            var expectedFailedProviderCategoryStorageException =
              new FailedProviderCategoryStorageException(
                  message: "Failed ProviderCategory storage error occurred, Please contact support.",
                  databaseUpdateException);

            var expectedProviderCategoryDependencyException =
                new ProviderCategoryDependencyException(
                    message: "ProviderCategory dependency error occurred, contact support.",
                    expectedFailedProviderCategoryStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProviderCategoryByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<ProviderCategory> retrieveByIdProviderCategoryTask =
                this.providerCategoryService.RetrieveProviderCategoryByIdAsync(someProviderCategoryId);

            ProviderCategoryDependencyException actualDependencyException =
             await Assert.ThrowsAsync<ProviderCategoryDependencyException>(
                 retrieveByIdProviderCategoryTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedProviderCategoryDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProviderCategoryByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderCategoryDependencyException))),
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
            Guid someProviderCategoryId = Guid.NewGuid();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();


            var lockedProviderCategoryException = new LockedProviderCategoryException(
                message: "Locked ProviderCategory record exception, Please try again later.",
                innerException: databaseUpdateConcurrencyException);

            var expectedProviderCategoryDependencyException =
                new ProviderCategoryDependencyException(
                    message: "ProviderCategory dependency error occurred, contact support.",
                    innerException: lockedProviderCategoryException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProviderCategoryByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<ProviderCategory> retrieveByIdProviderCategoryTask =
                this.providerCategoryService.RetrieveProviderCategoryByIdAsync(someProviderCategoryId);

            ProviderCategoryDependencyException actualDependencyException =
             await Assert.ThrowsAsync<ProviderCategoryDependencyException>(
                 retrieveByIdProviderCategoryTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedProviderCategoryDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProviderCategoryByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderCategoryDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid someProviderCategoryId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedProviderCategoryServiceException =
                 new FailedProviderCategoryServiceException(
                     message: "Failed ProviderCategory service error occurred, contact support.",
                     innerException: serviceException);

            var expectedProviderCategoryServiceException =
                new ProviderCategoryServiceException(
                    message: "ProviderCategory service error occurred, contact support.",
                    innerException: failedProviderCategoryServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProviderCategoryByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<ProviderCategory> retrieveByIdProviderCategoryTask =
                this.providerCategoryService.RetrieveProviderCategoryByIdAsync(someProviderCategoryId);

            ProviderCategoryServiceException actualServiceException =
                 await Assert.ThrowsAsync<ProviderCategoryServiceException>(
                     retrieveByIdProviderCategoryTask.AsTask);

            // then
            actualServiceException.Should().BeEquivalentTo(
                expectedProviderCategoryServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProviderCategoryByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderCategoryServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
