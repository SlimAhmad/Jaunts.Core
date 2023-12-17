using FluentAssertions;
using Jaunts.Core.Api.Models.Processings.User.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Moq;
using System;
using System.Threading.Tasks;
using Xeptions;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Processings.Users
{
    public partial class UserProcessingServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationExceptionOnRetrieveUserByIdIfValidationErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            Guid someUserId= GetRandomGuid();

            var expectedUserProcessingDependencyValidationException =
                new UserProcessingDependencyValidationException(
                message: "User dependency validation error occurred, please try again.",
                        dependencyValidationException.InnerException as Xeption);

            this.userServiceMock.Setup(service =>
                service.RetrieveUserByIdAsync(It.IsAny<Guid>()))
                    .Throws(dependencyValidationException);

            // when
            ValueTask<ApplicationUser> actualUserResponseTask =
                this.userProcessingService.RetrieveUserById(someUserId);

            UserProcessingDependencyValidationException
               actualUserProcessingDependencyValidationException =
                   await Assert.ThrowsAsync<UserProcessingDependencyValidationException>(
                       actualUserResponseTask.AsTask);

            // then
            actualUserProcessingDependencyValidationException.Should().BeEquivalentTo(
                expectedUserProcessingDependencyValidationException);

            this.userServiceMock.Verify(service =>
                service.RetrieveUserByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedUserProcessingDependencyValidationException))),
                        Times.Once);

            this.userServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnRetrieveUserByIdIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            Guid someUserId = GetRandomGuid();

            var expectedUserProcessingDependencyException =
                new UserProcessingDependencyException(
                    message: "User dependency error occurred, please contact support",
                    dependencyException.InnerException as Xeption);

            this.userServiceMock.Setup(service =>
                service.RetrieveUserByIdAsync(It.IsAny<Guid>()))
                        .Throws(dependencyException);

            // when
            ValueTask<ApplicationUser> actualUserResponseTask =
                this.userProcessingService.RetrieveUserById(someUserId);


            UserProcessingDependencyException
             actualUserProcessingDependencyException =
                 await Assert.ThrowsAsync<UserProcessingDependencyException>(
                     actualUserResponseTask.AsTask);

            // then
            actualUserProcessingDependencyException.Should().BeEquivalentTo(
                expectedUserProcessingDependencyException);

            this.userServiceMock.Verify(service =>
                service.RetrieveUserByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedUserProcessingDependencyException))),
                        Times.Once);

            this.userServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveUserByIdIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Guid someUserId = GetRandomGuid();

            var serviceException = new Exception();

            var failedUserProcessingServiceException =
                new FailedUserProcessingServiceException(
                    message: "Failed user service occurred, please contact support",
                    innerException: serviceException);

            var expectedUserProcessingServiceException =
                new UserProcessingServiceException(
                    message: "Failed user service occurred, please contact support",
                    innerException: failedUserProcessingServiceException);

            this.userServiceMock.Setup(service =>
                service.RetrieveUserByIdAsync(It.IsAny<Guid>()))
                    .Throws(serviceException);

            // when
            ValueTask<ApplicationUser> actualUserResponseTask =
                this.userProcessingService.RetrieveUserById(someUserId);

            UserProcessingServiceException
               actualUserProcessingServiceException =
                   await Assert.ThrowsAsync<UserProcessingServiceException>(
                       actualUserResponseTask.AsTask);

            // then
            actualUserProcessingServiceException.Should().BeEquivalentTo(
                 expectedUserProcessingServiceException);

            this.userServiceMock.Verify(service =>
                service.RetrieveUserByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedUserProcessingServiceException))),
                        Times.Once);

            this.userServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
