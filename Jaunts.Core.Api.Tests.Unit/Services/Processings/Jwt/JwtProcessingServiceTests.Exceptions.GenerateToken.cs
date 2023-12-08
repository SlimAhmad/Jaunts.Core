// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Processings.Jwts.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Moq;
using System;
using System.Threading.Tasks;
using Xeptions;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Processings.Jwt
{
    public partial class JwtProcessingServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationExceptionOnGenerateJwtIfValidationErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            ApplicationUser someUser = CreateRandomUser();
            int someNumber = GetRandomNumber();

            var expectedJwtProcessingDependencyValidationException =
                new JwtProcessingDependencyValidationException(
                    message: "Jwt dependency validation error occurred, please try again.",
                    dependencyValidationException.InnerException as Xeption);

            this.jwtServiceMock.Setup(service =>
                service.GenerateJwtTokenRequestAsync(
                    It.IsAny<ApplicationUser>(), It.IsAny<int>()))
                        .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<string> actualJwtResponseTask =
                this.jwtProcessingService.GenerateJwtTokenAsync(someUser, someNumber);

            JwtProcessingDependencyValidationException
               actualJwtProcessingDependencyValidationException =
                   await Assert.ThrowsAsync<JwtProcessingDependencyValidationException>(
                       actualJwtResponseTask.AsTask);

            // then
            actualJwtProcessingDependencyValidationException.Should().BeEquivalentTo(
                expectedJwtProcessingDependencyValidationException);

            this.jwtServiceMock.Verify(service =>
                service.GenerateJwtTokenRequestAsync(It.IsAny<ApplicationUser>(),It.IsAny<int>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedJwtProcessingDependencyValidationException))),
                        Times.Once);

            this.jwtServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnGenerateJwtIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            ApplicationUser someUser = CreateRandomUser();
            int someNumber = GetRandomNumber();

            var expectedJwtProcessingDependencyException =
                new JwtProcessingDependencyException(
                    message: "Jwt processing dependency error occurred, please contact support",
                    dependencyException.InnerException as Xeption);

            this.jwtServiceMock.Setup(service =>
                service.GenerateJwtTokenRequestAsync(
                    It.IsAny<ApplicationUser>(), It.IsAny<int>()))
                        .ThrowsAsync(dependencyException);

            // when
            ValueTask<string> actualJwtResponseTask =
                this.jwtProcessingService.GenerateJwtTokenAsync(someUser, someNumber);


            JwtProcessingDependencyException
             actualJwtProcessingDependencyException =
                 await Assert.ThrowsAsync<JwtProcessingDependencyException>(
                     actualJwtResponseTask.AsTask);

            // then
            actualJwtProcessingDependencyException.Should().BeEquivalentTo(
                expectedJwtProcessingDependencyException);

            this.jwtServiceMock.Verify(service =>
                service.GenerateJwtTokenRequestAsync(It.IsAny<ApplicationUser>(), It.IsAny<int>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedJwtProcessingDependencyException))),
                        Times.Once);

            this.jwtServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnGenerateJwtIfServiceErrorOccursAndLogItAsync()
        {
            // given
            ApplicationUser someUser = CreateRandomUser();
            int someNumber = GetRandomNumber();

            var serviceException = new Exception();

            var failedJwtProcessingServiceException =
                new FailedJwtProcessingServiceException(
                    message: "Failed jwt processing service occurred, please contact support",
                    innerException: serviceException);

            var expectedJwtProcessingServiceException =
                new JwtProcessingServiceException(
                    message: "Failed jwt processing service occurred, please contact support",
                    innerException: failedJwtProcessingServiceException);

            this.jwtServiceMock.Setup(service =>
                service.GenerateJwtTokenRequestAsync(It.IsAny<ApplicationUser>(), It.IsAny<int>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<string> actualJwtResponseTask =
                this.jwtProcessingService.GenerateJwtTokenAsync(someUser, someNumber);

            JwtProcessingServiceException
               actualJwtProcessingServiceException =
                   await Assert.ThrowsAsync<JwtProcessingServiceException>(
                       actualJwtResponseTask.AsTask);

            // then
            actualJwtProcessingServiceException.Should().BeEquivalentTo(
                 expectedJwtProcessingServiceException);

            this.jwtServiceMock.Verify(service =>
                service.GenerateJwtTokenRequestAsync(It.IsAny<ApplicationUser>(), It.IsAny<int>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedJwtProcessingServiceException))),
                        Times.Once);

            this.jwtServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

    }
}
