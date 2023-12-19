using FluentAssertions;
using Jaunts.Core.Api.Models.Processings.Emails.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Models.Email;
using Jaunts.Core.Models.Exceptions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xeptions;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Processings.Email
{
    public partial class EmailProcessingServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationExceptionOnSendForgetPasswordMailIfValidationErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            ApplicationUser someUser = CreateRandomUser();
            SendEmailMessage someSendEmailMessage = CreateSendEmailDetailRequest();
            string someToken = GetRandomString();

            var expectedEmailProcessingDependencyValidationException =
                new EmailProcessingDependencyValidationException(
                    message: "Email dependency validation error occurred, please try again.",
                    dependencyValidationException.InnerException as Xeption);

            this.emailTemplateSenderMock.Setup(service =>
                service.SendVerificationEmailAsync(
                    It.IsAny<SendEmailMessage>(), It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                        .ReturnsAsync(someSendEmailMessage);

            this.emailServiceMock.Setup(service =>
                service.SendEmailRequestAsync(It.IsAny<SendEmailMessage>()))
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<SendEmailResponse> actualEmailResponseTask =
                this.emailProcessingService.ForgetPasswordMailRequestAsync(someUser,someToken);

            EmailProcessingDependencyValidationException
               actualEmailProcessingDependencyValidationException =
                   await Assert.ThrowsAsync<EmailProcessingDependencyValidationException>(
                       actualEmailResponseTask.AsTask);

            // then
            actualEmailProcessingDependencyValidationException.Should().BeEquivalentTo(
                expectedEmailProcessingDependencyValidationException);

            this.emailTemplateSenderMock.Verify(service =>
                service.SendVerificationEmailAsync(
                    It.IsAny<SendEmailMessage>(), It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
                    Times.Once);

            this.emailServiceMock.Verify(service =>
                service.SendEmailRequestAsync(It.IsAny<SendEmailMessage>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedEmailProcessingDependencyValidationException))),
                        Times.Once);

            this.emailTemplateSenderMock.VerifyNoOtherCalls();  
            this.emailServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnSendForgetPasswordMailIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            ApplicationUser someUser = CreateRandomUser();
            SendEmailMessage someSendEmailMessage = CreateSendEmailDetailRequest();
            string someToken = GetRandomString();

            var expectedEmailProcessingDependencyException =
                new EmailProcessingDependencyException(
                    message: "Email Processing dependency error occurred, please contact support",
                    dependencyException.InnerException as Xeption);

            this.emailTemplateSenderMock.Setup(service =>
                service.SendVerificationEmailAsync(
                    It.IsAny<SendEmailMessage>(), It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                        .ReturnsAsync(someSendEmailMessage);

            this.emailServiceMock.Setup(service =>
                service.SendEmailRequestAsync(someSendEmailMessage))
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<SendEmailResponse> actualEmailResponseTask =
                this.emailProcessingService.ForgetPasswordMailRequestAsync(someUser,someToken);


            EmailProcessingDependencyException
             actualEmailProcessingDependencyException =
                 await Assert.ThrowsAsync<EmailProcessingDependencyException>(
                     actualEmailResponseTask.AsTask);

            // then
            actualEmailProcessingDependencyException.Should().BeEquivalentTo(
                expectedEmailProcessingDependencyException);

            this.emailTemplateSenderMock.Verify(service =>
                service.SendVerificationEmailAsync(
                    It.IsAny<SendEmailMessage>(), It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
                    Times.Once);

            this.emailServiceMock.Verify(service =>
                service.SendEmailRequestAsync(someSendEmailMessage),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedEmailProcessingDependencyException))),
                        Times.Once);

            this.emailTemplateSenderMock.VerifyNoOtherCalls();
            this.emailServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnSendForgetPasswordMailIfServiceErrorOccursAndLogItAsync()
        {
            // given
            ApplicationUser someUser = CreateRandomUser();
            SendEmailMessage someSendEmailMessage = CreateSendEmailDetailRequest();
            string someToken = GetRandomString();

            var serviceException = new Exception();

            var failedEmailProcessingServiceException =
                new FailedEmailProcessingServiceException(
                    message: "Failed email processing service occurred, please contact support",
                    innerException: serviceException);

            var expectedEmailProcessingServiceException =
                new EmailProcessingServiceException(
                    message: "Failed email processing service occurred, please contact support",
                    innerException: failedEmailProcessingServiceException);

            this.emailTemplateSenderMock.Setup(service =>
                service.SendVerificationEmailAsync(
                    It.IsAny<SendEmailMessage>(), It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                        .ReturnsAsync(someSendEmailMessage);

            this.emailServiceMock.Setup(service =>
                service.SendEmailRequestAsync(someSendEmailMessage))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<SendEmailResponse> actualEmailResponseTask =
                this.emailProcessingService.ForgetPasswordMailRequestAsync(someUser,someToken);

            EmailProcessingServiceException
               actualEmailProcessingServiceException =
                   await Assert.ThrowsAsync<EmailProcessingServiceException>(
                       actualEmailResponseTask.AsTask);

            // then
            actualEmailProcessingServiceException.Should().BeEquivalentTo(
                 expectedEmailProcessingServiceException);

            this.emailTemplateSenderMock.Verify(service =>
                service.SendVerificationEmailAsync(
                    It.IsAny<SendEmailMessage>(), It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
                    Times.Once);

            this.emailServiceMock.Verify(service =>
                service.SendEmailRequestAsync(someSendEmailMessage),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedEmailProcessingServiceException))),
                        Times.Once);

            this.emailTemplateSenderMock.VerifyNoOtherCalls();
            this.emailServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
