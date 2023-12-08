using FluentAssertions;
using Jaunts.Core.Api.Models.Processings.Role.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Role;
using Moq;
using System;
using System.Threading.Tasks;
using Xeptions;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Processings.Role
{
    public partial class RoleProcessingServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationExceptionOnRemoveRoleByIdIfValidationErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            Guid someRoleId= GetRandomGuid();

            var expectedRoleProcessingDependencyValidationException =
                new RoleProcessingDependencyValidationException(
                message: "Role dependency validation error occurred, please try again.",
                        dependencyValidationException.InnerException as Xeption);

            this.roleServiceMock.Setup(service =>
                service.RemoveRoleByIdRequestAsync(It.IsAny<Guid>()))
                    .Throws(dependencyValidationException);

            // when
            ValueTask<bool> actualRoleResponseTask =
                this.roleProcessingService.RemoveRoleByIdAsync(someRoleId);

            RoleProcessingDependencyValidationException
               actualRoleProcessingDependencyValidationException =
                   await Assert.ThrowsAsync<RoleProcessingDependencyValidationException>(
                       actualRoleResponseTask.AsTask);

            // then
            actualRoleProcessingDependencyValidationException.Should().BeEquivalentTo(
                expectedRoleProcessingDependencyValidationException);

            this.roleServiceMock.Verify(service =>
                service.RemoveRoleByIdRequestAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRoleProcessingDependencyValidationException))),
                        Times.Once);

            this.roleServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnRemoveRoleByIdIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            Guid someRoleId = GetRandomGuid();

            var expectedRoleProcessingDependencyException =
                new RoleProcessingDependencyException(
                    message: "Role dependency error occurred, please contact support",
                    dependencyException.InnerException as Xeption);

            this.roleServiceMock.Setup(service =>
                service.RemoveRoleByIdRequestAsync(It.IsAny<Guid>()))
                        .Throws(dependencyException);

            // when
            ValueTask<bool> actualRoleResponseTask =
                this.roleProcessingService.RemoveRoleByIdAsync(someRoleId);


            RoleProcessingDependencyException
             actualRoleProcessingDependencyException =
                 await Assert.ThrowsAsync<RoleProcessingDependencyException>(
                     actualRoleResponseTask.AsTask);

            // then
            actualRoleProcessingDependencyException.Should().BeEquivalentTo(
                expectedRoleProcessingDependencyException);

            this.roleServiceMock.Verify(service =>
                service.RemoveRoleByIdRequestAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRoleProcessingDependencyException))),
                        Times.Once);

            this.roleServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRemoveRoleByIdIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Guid someRoleId = GetRandomGuid();

            var serviceException = new Exception();

            var failedRoleProcessingServiceException =
                new FailedRoleProcessingServiceException(
                    message: "Failed role service occurred, please contact support",
                    innerException: serviceException);

            var expectedRoleProcessingServiceException =
                new RoleProcessingServiceException(
                    message: "Failed role processing service occurred, please contact support",
                    innerException: failedRoleProcessingServiceException);

            this.roleServiceMock.Setup(service =>
                service.RemoveRoleByIdRequestAsync(It.IsAny<Guid>()))
                    .Throws(serviceException);

            // when
            ValueTask<bool> actualRoleResponseTask =
                this.roleProcessingService.RemoveRoleByIdAsync(someRoleId);

            RoleProcessingServiceException
               actualRoleProcessingServiceException =
                   await Assert.ThrowsAsync<RoleProcessingServiceException>(
                       actualRoleResponseTask.AsTask);

            // then
            actualRoleProcessingServiceException.Should().BeEquivalentTo(
                 expectedRoleProcessingServiceException);

            this.roleServiceMock.Verify(service =>
                service.RemoveRoleByIdRequestAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRoleProcessingServiceException))),
                        Times.Once);

            this.roleServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
