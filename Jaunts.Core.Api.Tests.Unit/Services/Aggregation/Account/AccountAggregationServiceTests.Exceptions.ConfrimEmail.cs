// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Jaunts.Core.Api.Models.Auth;
using Jaunts.Core.Api.Models.Services.Foundations.Auth.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Microsoft.Data.SqlClient;
using Moq;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Auth
{
    public partial class AccountAggregationServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnConfirmEmailWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            ApplicationUser randomUser = CreateRandomUser(dateTime);
            string randomToken = GetRandomString();
            string randomEmail = GetRandomEmailAddresses();
            var sqlException = GetSqlException();


            var failedAuthStorageException =
                new FailedAuthStorageException(sqlException);

            var expectedAuthDependencyException =
                new AuthDependencyException(failedAuthStorageException);

            this.userOrchestrationMock.Setup(broker =>
               broker.ConfirmEmailAsync(It.IsAny<string>(),It.IsAny<string>()))
                   .ReturnsAsync(randomUser);

            this.jwtOrchestrationMock.Setup(broker =>
                broker.JwtAccountDetailsAsync(It.IsAny<ApplicationUser>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<UserAccountDetailsApiResponse> registerAuthTask =
                this.accountAggregationService.ConfirmEmailRequestAsync(randomToken, randomEmail);

            // then
            await Assert.ThrowsAsync<AuthDependencyException>(() =>
                registerAuthTask.AsTask());

            this.userOrchestrationMock.Verify(broker =>
                 broker.ConfirmEmailAsync(It.IsAny<string>(), It.IsAny<string>()),
                     Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedAuthDependencyException))),
                        Times.Once);

            this.jwtOrchestrationMock.Verify(broker =>
              broker.JwtAccountDetailsAsync(It.IsAny<ApplicationUser>()),
                  Times.Once);


            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userOrchestrationMock.VerifyNoOtherCalls();
            this.jwtOrchestrationMock.VerifyNoOtherCalls();
        }


        [Fact]
        public async Task ShouldThrowServiceExceptionOnConfirmEmailWhenExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            ApplicationUser randomUser = CreateRandomUser(dateTime);
            string randomToken = GetRandomString();
            string randomEmail = GetRandomEmailAddresses();
            var serviceException = new Exception();


            var failedAuthServiceException =
                new FailedAuthServiceException(serviceException);

            var expectedAuthServiceException =
                new AuthServiceException(failedAuthServiceException);


            this.userOrchestrationMock.Setup(broker =>
               broker.ConfirmEmailAsync(It.IsAny<string>(), It.IsAny<string>()))
                   .ReturnsAsync(randomUser);

            this.jwtOrchestrationMock.Setup(broker =>
                broker.JwtAccountDetailsAsync(It.IsAny<ApplicationUser>()))
                    .ThrowsAsync(failedAuthServiceException);

            // when
            ValueTask<UserAccountDetailsApiResponse> registerAuthTask =
                 this.accountAggregationService.ConfirmEmailRequestAsync(randomToken, randomEmail);

            // then
            await Assert.ThrowsAsync<AuthServiceException>(() =>
                registerAuthTask.AsTask());

            this.userOrchestrationMock.Verify(broker =>
               broker.ConfirmEmailAsync(It.IsAny<string>(), It.IsAny<string>()),
                   Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAuthServiceException))),
                        Times.Once);

            this.jwtOrchestrationMock.Verify(broker =>
              broker.JwtAccountDetailsAsync(It.IsAny<ApplicationUser>()),
                  Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userOrchestrationMock.VerifyNoOtherCalls();
            this.userOrchestrationMock.VerifyNoOtherCalls();
        }
    }
}
