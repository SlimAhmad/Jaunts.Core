// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.ProviderCategory;
using Jaunts.Core.Api.Models.Services.Foundations.ProviderCategorys;
using Jaunts.Core.Api.Models.Services.Foundations.ProviderCategorys.Exceptions;
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
        public async Task ShouldThrowDependencyExceptionOnCreateWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            ProviderCategory someProviderCategory = CreateRandomProviderCategory(dateTime);
            someProviderCategory.UpdatedBy = someProviderCategory.CreatedBy;
            var sqlException = GetSqlException();

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
            ValueTask<ProviderCategory> createProviderCategoryTask =
                this.providerCategoryService.CreateProviderCategoryAsync(someProviderCategory);

            ProviderCategoryDependencyException actualDependencyException =
             await Assert.ThrowsAsync<ProviderCategoryDependencyException>(
                 createProviderCategoryTask.AsTask);

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
                broker.InsertProviderCategoryAsync(It.IsAny<ProviderCategory>()),
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
            ProviderCategory someProviderCategory = CreateRandomProviderCategory(dateTime);
            someProviderCategory.UpdatedBy = someProviderCategory.CreatedBy;
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
            ValueTask<ProviderCategory> createProviderCategoryTask =
                this.providerCategoryService.CreateProviderCategoryAsync(someProviderCategory);

            ProviderCategoryDependencyException actualDependencyException =
                 await Assert.ThrowsAsync<ProviderCategoryDependencyException>(
                     createProviderCategoryTask.AsTask);

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
                broker.InsertProviderCategoryAsync(It.IsAny<ProviderCategory>()),
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
            ProviderCategory someProviderCategory = CreateRandomProviderCategory(dateTime);
            someProviderCategory.UpdatedBy = someProviderCategory.CreatedBy;
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
            ValueTask<ProviderCategory> createProviderCategoryTask =
                 this.providerCategoryService.CreateProviderCategoryAsync(someProviderCategory);

            ProviderCategoryServiceException actualDependencyException =
                 await Assert.ThrowsAsync<ProviderCategoryServiceException>(
                     createProviderCategoryTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedProviderCategoryServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderCategoryServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertProviderCategoryAsync(It.IsAny<ProviderCategory>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
