// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Models.Email;
using Jaunts.Core.Models.Exceptions;
using Moq;
using RESTFulSense.Exceptions;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Emails
{
    public partial class EmailServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnPostSendEmailRequestIfUrlNotFoundAsync()
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            ApplicationUser randomUser = CreateRandomUser(dates: randomDateTime);
            var randomText = GetRandomSubject();
            var SendEmailDetails = CreateSendEmailDetailRequest();


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

            this.emailBrokerMock.Setup(broker =>
                broker.SendEmailAsync(It.IsAny<SendEmailDetails>()))
                    .ThrowsAsync(httpResponseUrlNotFoundException);

            // when
            ValueTask<SendEmailResponse> retrieveSendEmailResponseTask =
               this.emailService.SendEmailRequestAsync(SendEmailDetails);

            EmailDependencyException
                actualEmailDependencyException =
                    await Assert.ThrowsAsync<EmailDependencyException>(
                        retrieveSendEmailResponseTask.AsTask);

            // then
            actualEmailDependencyException.Should().BeEquivalentTo(
                expectedEmailDependencyException);

            this.emailBrokerMock.Verify(broker =>
                broker.SendEmailAsync(It.IsAny<SendEmailDetails>()),
                    Times.Once);

            
            
            this.emailBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(UnauthorizedExceptions))]
        public async Task ShouldThrowDependencyExceptionOnPostSendEmailRequestIfUnauthorizedAsync(
            HttpResponseException unauthorizedException)
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            ApplicationUser randomUser = CreateRandomUser(dates: randomDateTime);
            var randomText = GetRandomText();
            var SendEmailDetails = CreateSendEmailDetailRequest();



            var unauthorizedEmailException =
                new UnauthorizedEmailException(unauthorizedException);

            var expectedEmailDependencyException =
                new EmailDependencyException(unauthorizedEmailException);


            this.emailBrokerMock.Setup(broker =>
                broker.SendEmailAsync(It.IsAny<SendEmailDetails>()))
                    .ThrowsAsync(unauthorizedException);

            // when
            ValueTask<SendEmailResponse> retrieveSendEmailResponseTask =
                this.emailService.SendEmailRequestAsync(SendEmailDetails);

            EmailDependencyException
                actualEmailDependencyException =
                    await Assert.ThrowsAsync<EmailDependencyException>(
                        retrieveSendEmailResponseTask.AsTask);

            // then
            actualEmailDependencyException.Should().BeEquivalentTo(
                expectedEmailDependencyException);

            this.emailBrokerMock.Verify(broker =>
                broker.SendEmailAsync(It.IsAny<SendEmailDetails>()),
                    Times.Once);

            
            
            this.emailBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnPostSendEmailRequestIfNotFoundOccurredAsync()
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            ApplicationUser randomUser = CreateRandomUser(dates: randomDateTime);
            var randomText = GetRandomText();
            var SendEmailDetails = CreateSendEmailDetailRequest();




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

            this.emailBrokerMock.Setup(broker =>
                broker.SendEmailAsync(It.IsAny<SendEmailDetails>()))
                    .ThrowsAsync(httpResponseNotFoundException);

            // when
            ValueTask<SendEmailResponse> retrieveSendEmailResponseTask =
                this.emailService.SendEmailRequestAsync(SendEmailDetails);

            EmailDependencyValidationException
                actualEmailDependencyValidationException =
                    await Assert.ThrowsAsync<EmailDependencyValidationException>(
                        retrieveSendEmailResponseTask.AsTask);

            // then
            actualEmailDependencyValidationException.Should().BeEquivalentTo(
                expectedEmailDependencyValidationException);

            this.emailBrokerMock.Verify(broker =>
                broker.SendEmailAsync(It.IsAny<SendEmailDetails>()),
                    Times.Once);

            
            
            this.emailBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnPostSendEmailRequestIfBadRequestOccurredAsync()
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            ApplicationUser randomUser = CreateRandomUser(dates: randomDateTime);
            var randomText = GetRandomText();
            var SendEmailDetails = CreateSendEmailDetailRequest();

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

            this.emailBrokerMock.Setup(broker =>
                broker.SendEmailAsync(It.IsAny<SendEmailDetails>()))
                    .ThrowsAsync(httpResponseBadRequestException);

            // when
            ValueTask<SendEmailResponse> retrieveSendEmailResponseTask =
                this.emailService.SendEmailRequestAsync(SendEmailDetails);

            EmailDependencyValidationException
                actualEmailDependencyValidationException =
                    await Assert.ThrowsAsync<EmailDependencyValidationException>(
                        retrieveSendEmailResponseTask.AsTask);

            // then
            actualEmailDependencyValidationException.Should().BeEquivalentTo(
                expectedEmailDependencyValidationException);

            this.emailBrokerMock.Verify(broker =>
                broker.SendEmailAsync(It.IsAny<SendEmailDetails>()),
                    Times.Once);

            
            
            this.emailBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnPostSendEmailRequestIfTooManyRequestsOccurredAsync()
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

            this.emailBrokerMock.Setup(broker =>
                broker.SendEmailAsync(It.IsAny<SendEmailDetails>()))
                    .ThrowsAsync(httpResponseTooManyRequestsException);

            // when
            ValueTask<SendEmailResponse> retrieveSendEmailResponseTask =
                this.emailService.SendEmailRequestAsync(SendEmailDetails);

            EmailDependencyValidationException actualEmailDependencyValidationException =
                await Assert.ThrowsAsync<EmailDependencyValidationException>(
                    retrieveSendEmailResponseTask.AsTask);

            // then
            actualEmailDependencyValidationException.Should().BeEquivalentTo(
                expectedEmailDependencyValidationException);

            this.emailBrokerMock.Verify(broker =>
                broker.SendEmailAsync(It.IsAny<SendEmailDetails>()),
                    Times.Once);

            
            
            this.emailBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnPostSendEmailRequestIfHttpResponseErrorOccurredAsync()
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            ApplicationUser randomUser = CreateRandomUser(dates: randomDateTime);
            var randomText = GetRandomText();
            var SendEmailDetails = CreateSendEmailDetailRequest();


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

            this.emailBrokerMock.Setup(broker =>
                broker.SendEmailAsync(It.IsAny<SendEmailDetails>()))
                    .ThrowsAsync(httpResponseException);

            // when
            ValueTask<SendEmailResponse> retrieveSendEmailResponseTask =
                this.emailService.SendEmailRequestAsync(SendEmailDetails);

            EmailDependencyException actualEmailDependencyException =
                await Assert.ThrowsAsync<EmailDependencyException>(
                    retrieveSendEmailResponseTask.AsTask);

            // then
            actualEmailDependencyException.Should().BeEquivalentTo(
                expectedEmailDependencyException);

            this.emailBrokerMock.Verify(broker =>
                broker.SendEmailAsync(It.IsAny<SendEmailDetails>()),
                    Times.Once);

            
            
            this.emailBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnPostSendEmailRequestIfServiceErrorOccurredAsync()
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            ApplicationUser randomUser = CreateRandomUser(dates: randomDateTime);
            var randomText = GetRandomText();
            var SendEmailDetails = CreateSendEmailDetailRequest();


            var serviceException = new Exception();

            var failedEmailServiceException =
                new FailedEmailServiceException(serviceException);

            var expectedEmailServiceException =
                new EmailServiceException(failedEmailServiceException);

            this.emailBrokerMock.Setup(broker =>
                broker.SendEmailAsync(It.IsAny<SendEmailDetails>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<SendEmailResponse> retrieveSendEmailResponseTask =
                this.emailService.SendEmailRequestAsync(SendEmailDetails);

            EmailServiceException actualEmailServiceException =
                await Assert.ThrowsAsync<EmailServiceException>(
                    retrieveSendEmailResponseTask.AsTask);

            // then
            actualEmailServiceException.Should().BeEquivalentTo(
                expectedEmailServiceException);

            this.emailBrokerMock.Verify(broker =>
                broker.SendEmailAsync(It.IsAny<SendEmailDetails>()),
                    Times.Once);    
            
            this.emailBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
