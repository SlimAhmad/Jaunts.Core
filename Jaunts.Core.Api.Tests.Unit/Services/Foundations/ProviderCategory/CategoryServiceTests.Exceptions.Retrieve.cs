// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.ProviderCategorys.Exceptions;
using Microsoft.Data.SqlClient;
using Moq;
using System;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.ProviderCategories
{
    public partial class ProviderCategoryServiceTests
    {
        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
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
                broker.SelectAllProviderCategories())
                    .Throws(sqlException);

            // when
            Action retrieveAllProviderCategorysAction = () =>
                this.providerCategoryService.RetrieveAllProviderCategories();

            ProviderCategoryDependencyException actualDependencyException =
              Assert.Throws<ProviderCategoryDependencyException>(
                 retrieveAllProviderCategorysAction);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedProviderCategoryDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllProviderCategories(),
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
        public void ShouldThrowServiceExceptionOnRetrieveAllWhenExceptionOccursAndLogIt()
        {
            // given
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
                broker.SelectAllProviderCategories())
                    .Throws(serviceException);

            // when
            Action retrieveAllProviderCategorysAction = () =>
                this.providerCategoryService.RetrieveAllProviderCategories();

            ProviderCategoryServiceException actualServiceException =
              Assert.Throws<ProviderCategoryServiceException>(
                 retrieveAllProviderCategorysAction);

            // then
            actualServiceException.Should().BeEquivalentTo(
                expectedProviderCategoryServiceException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProviderCategoryServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllProviderCategories(),
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
