using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Models.Email;
using Jaunts.Core.Models.Exceptions;
using Moq;
using RESTFulSense.Exceptions;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Emails
{
    public partial class EmailServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnPostOTPVerificationMailRequestIfUrlNotFoundAsync()
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            ApplicationUser randomUser = CreateRandomUser(dates: randomDateTime);
            var randomText = GetRandomSubject();
            var sendEmailDetails = CreateSendEmailDetailRequest();


            var httpResponseUrlNotFoundException =
                new HttpResponseUrlNotFoundException();

            var invalidConfigurationEmailException =
                new InvalidConfigurationEmailException(
                    message: "Invalid Email configuration error occurred, contact support.",
                    httpResponseUrlNotFoundException);

            var expectedEmailDependencyException =
                new EmailDependencyException(
                    message: "Email dependency error occurred, contact support.",
                    invalidConfigurationEmailException);

            this.emailTemplateSender.Setup(broker =>
                broker.SendVerificationEmailAsync(It.IsAny<SendEmailDetails>(),It.IsAny<string>(),It.IsAny<string>(),It.IsAny<string>(),It.IsAny<string>(),It.IsAny<string>()))
                    .ReturnsAsync(sendEmailDetails);

            this.emailBrokerMock.Setup(broker =>
                broker.PostMailAsync(It.IsAny<SendEmailDetails>()))
                    .ThrowsAsync(httpResponseUrlNotFoundException);

            // when
            ValueTask<SendEmailResponse> retrieveSendEmailResponseTask =
               this.emailService.PostOTPVerificationMailRequestAsync(randomUser,randomText, randomText,randomText,randomText);

            EmailDependencyException
                actualEmailDependencyException =
                    await Assert.ThrowsAsync<EmailDependencyException>(
                        retrieveSendEmailResponseTask.AsTask);

            // then
            actualEmailDependencyException.Should().BeEquivalentTo(
                expectedEmailDependencyException);

            this.emailTemplateSender.Verify(broker =>
                broker.SendVerificationEmailAsync(
                    It.IsAny<SendEmailDetails>(), It.IsAny<string>(),
                    It.IsAny<string>(), It.IsAny<string>(), 
                    It.IsAny<string>(), It.IsAny<string>()),
                     Times.Once);
                   

            this.emailBrokerMock.Verify(broker =>
                broker.PostMailAsync(It.IsAny<SendEmailDetails>()),
                    Times.Once);

            this.emailTemplateSender.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
            this.emailBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(UnauthorizedExceptions))]
        public async Task ShouldThrowDependencyExceptionOnPostOTPVerificationMailRequestIfUnauthorizedAsync(
            HttpResponseException unauthorizedException)
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            ApplicationUser randomUser = CreateRandomUser(dates: randomDateTime);
            var randomText = GetRandomText();
             var sendEmailDetails = CreateSendEmailDetailRequest();
         


            var unauthorizedEmailException =
                new UnauthorizedEmailException(unauthorizedException);

            var expectedEmailDependencyException =
                new EmailDependencyException(unauthorizedEmailException);


            this.emailTemplateSender.Setup(broker =>
                broker.SendVerificationEmailAsync(It.IsAny<SendEmailDetails>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                    .ReturnsAsync(sendEmailDetails);

            this.emailBrokerMock.Setup(broker =>
                broker.PostMailAsync(It.IsAny<SendEmailDetails>()))
                    .ThrowsAsync(unauthorizedException);

            // when
            ValueTask<SendEmailResponse> retrieveSendEmailResponseTask =
                this.emailService.PostOTPVerificationMailRequestAsync(randomUser, randomText, randomText, randomText, randomText);

            EmailDependencyException
                actualEmailDependencyException =
                    await Assert.ThrowsAsync<EmailDependencyException>(
                        retrieveSendEmailResponseTask.AsTask);

            // then
            actualEmailDependencyException.Should().BeEquivalentTo(
                expectedEmailDependencyException);

            this.emailTemplateSender.Verify(broker =>
                 broker.SendVerificationEmailAsync(
                     It.IsAny<SendEmailDetails>(), It.IsAny<string>(),
                     It.IsAny<string>(), It.IsAny<string>(),
                     It.IsAny<string>(), It.IsAny<string>()),
                      Times.Once);


            this.emailBrokerMock.Verify(broker =>
                broker.PostMailAsync(It.IsAny<SendEmailDetails>()),
                    Times.Once);

            this.emailTemplateSender.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
            this.emailBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnPostOTPVerificationMailRequestIfNotFoundOccurredAsync()
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            ApplicationUser randomUser = CreateRandomUser(dates: randomDateTime);
            var randomText = GetRandomText();
             var sendEmailDetails = CreateSendEmailDetailRequest();
            



            var httpResponseNotFoundException =
                new HttpResponseNotFoundException();

            var notFoundEmailException =
                new NotFoundEmailException(
                    message: "Not found Email error occurred, fix errors and try again.",
                    httpResponseNotFoundException);

            var expectedEmailDependencyValidationException =
                new EmailDependencyValidationException(
                    message: "Email dependency validation error occurred, contact support.",
                    notFoundEmailException);

            this.emailTemplateSender.Setup(broker =>
               broker.SendVerificationEmailAsync(It.IsAny<SendEmailDetails>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                   .ReturnsAsync(sendEmailDetails);

            this.emailBrokerMock.Setup(broker =>
                broker.PostMailAsync(It.IsAny<SendEmailDetails>()))
                    .ThrowsAsync(httpResponseNotFoundException);

            // when
            ValueTask<SendEmailResponse> retrieveSendEmailResponseTask =
                this.emailService.PostOTPVerificationMailRequestAsync(randomUser, randomText, randomText, randomText, randomText);

            EmailDependencyValidationException
                actualEmailDependencyValidationException =
                    await Assert.ThrowsAsync<EmailDependencyValidationException>(
                        retrieveSendEmailResponseTask.AsTask);

            // then
            actualEmailDependencyValidationException.Should().BeEquivalentTo(
                expectedEmailDependencyValidationException);

            this.emailTemplateSender.Verify(broker =>
              broker.SendVerificationEmailAsync(
                  It.IsAny<SendEmailDetails>(), It.IsAny<string>(),
                  It.IsAny<string>(), It.IsAny<string>(),
                  It.IsAny<string>(), It.IsAny<string>()),
                   Times.Once);


            this.emailBrokerMock.Verify(broker =>
                broker.PostMailAsync(It.IsAny<SendEmailDetails>()),
                    Times.Once);

            this.emailTemplateSender.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
            this.emailBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnPostOTPVerificationMailRequestIfBadRequestOccurredAsync()
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            ApplicationUser randomUser = CreateRandomUser(dates: randomDateTime);
            var randomText = GetRandomText();
             var sendEmailDetails = CreateSendEmailDetailRequest();
            



            var httpResponseBadRequestException =
                new HttpResponseBadRequestException();

            var invalidEmailException =
                new InvalidEmailException(
                    message: "Invalid Email error occurred, fix errors and try again.",
                    httpResponseBadRequestException);

            var expectedEmailDependencyValidationException =
                new EmailDependencyValidationException(
                    message: "Email dependency validation error occurred, contact support.",
                    invalidEmailException);

            this.emailTemplateSender.Setup(broker =>
                broker.SendVerificationEmailAsync(It.IsAny<SendEmailDetails>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                    .ReturnsAsync(sendEmailDetails);

            this.emailBrokerMock.Setup(broker =>
                broker.PostMailAsync(It.IsAny<SendEmailDetails>()))
                    .ThrowsAsync(httpResponseBadRequestException);

            // when
            ValueTask<SendEmailResponse> retrieveSendEmailResponseTask =
                this.emailService.PostOTPVerificationMailRequestAsync(randomUser, randomText, randomText, randomText, randomText);

            EmailDependencyValidationException
                actualEmailDependencyValidationException =
                    await Assert.ThrowsAsync<EmailDependencyValidationException>(
                        retrieveSendEmailResponseTask.AsTask);

            // then
            actualEmailDependencyValidationException.Should().BeEquivalentTo(
                expectedEmailDependencyValidationException);

            this.emailTemplateSender.Verify(broker =>
                  broker.SendVerificationEmailAsync(
                      It.IsAny<SendEmailDetails>(), It.IsAny<string>(),
                      It.IsAny<string>(), It.IsAny<string>(),
                      It.IsAny<string>(), It.IsAny<string>()),
                       Times.Once);


            this.emailBrokerMock.Verify(broker =>
                broker.PostMailAsync(It.IsAny<SendEmailDetails>()),
                    Times.Once);

            this.emailTemplateSender.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
            this.emailBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnPostOTPVerificationMailRequestIfTooManyRequestsOccurredAsync()
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            ApplicationUser randomUser = CreateRandomUser(dates: randomDateTime);
            var randomText = GetRandomText();
             var SendEmailDetails = CreateSendEmailDetailRequest();
            



            var httpResponseTooManyRequestsException =
                new HttpResponseTooManyRequestsException();

            var excessiveCallEmailException =
                new ExcessiveCallEmailException(
                    message: "Excessive call error occurred, limit your calls.",
                    httpResponseTooManyRequestsException);

            var expectedEmailDependencyValidationException =
                new EmailDependencyValidationException(
                    message: "Email dependency validation error occurred, contact support.",
                    excessiveCallEmailException);

            this.emailTemplateSender.Setup(broker =>
             broker.SendVerificationEmailAsync(It.IsAny<SendEmailDetails>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                 .ReturnsAsync(SendEmailDetails);

            this.emailBrokerMock.Setup(broker =>
                broker.PostMailAsync(It.IsAny<SendEmailDetails>()))
                    .ThrowsAsync(httpResponseTooManyRequestsException);

            // when
            ValueTask<SendEmailResponse> retrieveSendEmailResponseTask =
                this.emailService.PostOTPVerificationMailRequestAsync(randomUser, randomText, randomText, randomText, randomText);

            EmailDependencyValidationException actualEmailDependencyValidationException =
                await Assert.ThrowsAsync<EmailDependencyValidationException>(
                    retrieveSendEmailResponseTask.AsTask);

            // then
            actualEmailDependencyValidationException.Should().BeEquivalentTo(
                expectedEmailDependencyValidationException);

            this.emailTemplateSender.Verify(broker =>
              broker.SendVerificationEmailAsync(
                  It.IsAny<SendEmailDetails>(), It.IsAny<string>(),
                  It.IsAny<string>(), It.IsAny<string>(),
                  It.IsAny<string>(), It.IsAny<string>()),
                   Times.Once);


            this.emailBrokerMock.Verify(broker =>
                broker.PostMailAsync(It.IsAny<SendEmailDetails>()),
                    Times.Once);

            this.emailTemplateSender.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
            this.emailBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnPostOTPVerificationMailRequestIfHttpResponseErrorOccurredAsync()
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            ApplicationUser randomUser = CreateRandomUser(dates: randomDateTime);
            var randomText = GetRandomText();
            var sendEmailDetails = CreateSendEmailDetailRequest();



            var httpResponseException =
                new HttpResponseException();

            var failedServerEmailException =
                new FailedServerEmailException(
                    message: "Failed Email server error occurred, contact support.",
                    httpResponseException);

            var expectedEmailDependencyException =
                new EmailDependencyException(
                    message: "Email dependency error occurred, contact support.",
                    failedServerEmailException);

            this.emailTemplateSender.Setup(broker =>
              broker.SendVerificationEmailAsync(It.IsAny<SendEmailDetails>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                  .ReturnsAsync(sendEmailDetails);

            this.emailBrokerMock.Setup(broker =>
                broker.PostMailAsync(It.IsAny<SendEmailDetails>()))
                    .ThrowsAsync(httpResponseException);

            // when
            ValueTask<SendEmailResponse> retrieveSendEmailResponseTask =
                this.emailService.PostOTPVerificationMailRequestAsync(randomUser, randomText, randomText, randomText, randomText);

            EmailDependencyException actualEmailDependencyException =
                await Assert.ThrowsAsync<EmailDependencyException>(
                    retrieveSendEmailResponseTask.AsTask);

            // then
            actualEmailDependencyException.Should().BeEquivalentTo(
                expectedEmailDependencyException);

            this.emailTemplateSender.Verify(broker =>
             broker.SendVerificationEmailAsync(
                 It.IsAny<SendEmailDetails>(), It.IsAny<string>(),
                 It.IsAny<string>(), It.IsAny<string>(),
                 It.IsAny<string>(), It.IsAny<string>()),
                  Times.Once);


            this.emailBrokerMock.Verify(broker =>
                broker.PostMailAsync(It.IsAny<SendEmailDetails>()),
                    Times.Once);

            this.emailTemplateSender.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
            this.emailBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnPostOTPVerificationMailRequestIfServiceErrorOccurredAsync()
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            ApplicationUser randomUser = CreateRandomUser(dates: randomDateTime);
            var randomText = GetRandomText();
             var sendEmailDetails = CreateSendEmailDetailRequest();
            


            var serviceException = new Exception();

            var failedEmailServiceException =
                new FailedEmailServiceException(serviceException);

            var expectedEmailServiceException =
                new EmailServiceException(failedEmailServiceException);

            this.emailTemplateSender.Setup(broker =>
              broker.SendVerificationEmailAsync(It.IsAny<SendEmailDetails>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                  .ReturnsAsync(sendEmailDetails);

            this.emailBrokerMock.Setup(broker =>
                broker.PostMailAsync(It.IsAny<SendEmailDetails>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<SendEmailResponse> retrieveSendEmailResponseTask =
                this.emailService.PostOTPVerificationMailRequestAsync(randomUser, randomText, randomText, randomText, randomText);

            EmailServiceException actualEmailServiceException =
                await Assert.ThrowsAsync<EmailServiceException>(
                    retrieveSendEmailResponseTask.AsTask);

            // then
            actualEmailServiceException.Should().BeEquivalentTo(
                expectedEmailServiceException);

            this.emailTemplateSender.Verify(broker =>
                broker.SendVerificationEmailAsync(
                    It.IsAny<SendEmailDetails>(), It.IsAny<string>(),
                    It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<string>(), It.IsAny<string>()),
                     Times.Once);


            this.emailBrokerMock.Verify(broker =>
                broker.PostMailAsync(It.IsAny<SendEmailDetails>()),
                    Times.Once);

            this.emailTemplateSender.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
            this.emailBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
