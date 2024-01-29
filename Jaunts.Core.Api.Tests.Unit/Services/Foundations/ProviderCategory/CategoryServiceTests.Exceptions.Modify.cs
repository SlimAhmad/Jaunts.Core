// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.ProviderCategory;
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
        public async Task ShouldThrowDependencyExceptionOnModifyIfSqlExceptionOccursAndLogItAsync()
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            ProviderCategory someProviderCategory = CreateRandomProviderCategory(randomDateTime);
            someProviderCategory.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            SqlException sqlException = GetSqlException();

            var expectedFailedProviderCategoryStorageException =
              new FailedProviderCategoryStorageException(
                  message: "Failed ProviderCategory storage error occurred, Please contact support.",
                  innerException: sqlException);

            var expectedProviderCategoryDependencyException =
                new ProviderCategoryDependencyException(
                    message: "ProviderCategory dependency error occurred, contact support.",
                    innerException: expectedFailedProviderCategoryStorageException);


            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(sqlException);

            // when
            ValueTask<ProviderCategory> modifyProviderCategoryTask =
                this.providerCategoryService.ModifyProviderCategoryAsync(someProviderCategory);

                ProviderCategoryDependencyException actualDependencyException =
                 await Assert.ThrowsAsync<ProviderCategoryDependencyException>(
                     modifyProviderCategoryTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedProviderCategoryDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedProviderCategoryDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProviderCategoryByIdAsync(It.IsAny<Guid>()),
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
            ProviderCategory someProviderCategory = CreateRandomProviderCategory(randomDateTime);
            someProviderCategory.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            var databaseUpdateException = new DbUpdateException();

            var expectedFailedProviderCategoryStorageException =
              new FailedProviderCategoryStorageException(
                  message: "Failed ProviderCategory storage error occurred, Please contact support.",
                  databaseUpdateException);

            var expectedProviderCategoryDependencyException =
                new ProviderCategoryDependencyException(
                    message: "ProviderCategory dependency error occurred, contact support.",
                    expectedFailedProviderCategoryStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(databaseUpdateException);

            // when
            ValueTask<ProviderCategory> modifyProviderCategoryTask =
                this.providerCategoryService.ModifyProviderCategoryAsync(someProviderCategory);

            ProviderCategoryDependencyException actualDependencyException =
                await Assert.ThrowsAsync<ProviderCategoryDependencyException>(
                    modifyProviderCategoryTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedProviderCategoryDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderCategoryDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProviderCategoryByIdAsync(It.IsAny<Guid>()),
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
            ProviderCategory randomProviderCategory = CreateRandomProviderCategory(randomDateTime);
            ProviderCategory someProviderCategory = randomProviderCategory;
            someProviderCategory.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedProviderCategoryException = new LockedProviderCategoryException(
                message: "Locked ProviderCategory record exception, Please try again later.",
                innerException: databaseUpdateConcurrencyException);

            var expectedProviderCategoryDependencyException =
                new ProviderCategoryDependencyException(
                    message: "ProviderCategory dependency error occurred, contact support.",
                    innerException: lockedProviderCategoryException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(databaseUpdateConcurrencyException);

            // when
            ValueTask<ProviderCategory> modifyProviderCategoryTask =
                this.providerCategoryService.ModifyProviderCategoryAsync(someProviderCategory);

            ProviderCategoryDependencyException actualDependencyException =
             await Assert.ThrowsAsync<ProviderCategoryDependencyException>(
                 modifyProviderCategoryTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedProviderCategoryDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderCategoryDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProviderCategoryByIdAsync(It.IsAny<Guid>()),
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
            ProviderCategory randomProviderCategory = CreateRandomProviderCategory(randomDateTime);
            ProviderCategory someProviderCategory = randomProviderCategory;
            someProviderCategory.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            var serviceException = new Exception();

            var failedProviderCategoryServiceException =
             new FailedProviderCategoryServiceException(
                 message: "Failed ProviderCategory service error occurred, contact support.",
                 innerException: serviceException);

            var expectedProviderCategoryServiceException =
                new ProviderCategoryServiceException(
                    message: "ProviderCategory service error occurred, contact support.",
                    innerException: failedProviderCategoryServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(serviceException);

            // when
            ValueTask<ProviderCategory> modifyProviderCategoryTask =
                this.providerCategoryService.ModifyProviderCategoryAsync(someProviderCategory);

            ProviderCategoryServiceException actualServiceException =
             await Assert.ThrowsAsync<ProviderCategoryServiceException>(
                 modifyProviderCategoryTask.AsTask);

            // then
            actualServiceException.Should().BeEquivalentTo(
                expectedProviderCategoryServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderCategoryServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProviderCategoryByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
