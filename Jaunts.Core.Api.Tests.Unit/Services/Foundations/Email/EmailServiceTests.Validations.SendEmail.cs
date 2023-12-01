using FluentAssertions;
using Jaunts.Core.Models.Email;
using Jaunts.Core.Models.Exceptions;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Emails
{
    public partial class EmailServiceTests
    {


        [Fact]
        public async Task ShouldThrowValidationExceptionSendEmailDetailsIfEmailDetailsIsNullAsync()
        {
            // given
            SendEmailMessage nullEmailDetails = null;
            var nullEmailDetailsException = new NullEmailException();

            var exceptedEmailValidationException =
                new EmailValidationException(
                    message: "Email validation error occurred, fix errors and try again.",
                    innerException: nullEmailDetailsException);

            // when
            ValueTask<SendEmailResponse> EmailDetailsTask =
                this.emailService.SendEmailRequestAsync(nullEmailDetails);

            EmailValidationException actualEmailValidationException =
                await Assert.ThrowsAsync<EmailValidationException>(
                    EmailDetailsTask.AsTask);

            // then
            actualEmailValidationException.Should()
                .BeEquivalentTo(exceptedEmailValidationException);

            this.emailBrokerMock.Verify(broker =>
                broker.SendEmailAsync(
                    It.IsAny<SendEmailMessage>()),
                        Times.Never);

            this.emailBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null,null)]
        [InlineData("","")]
        [InlineData("  "," ")]
        public async Task ShouldThrowValidationExceptionSendGetEmailDetailsIfEmailDetailsIsInvalidAsync(
            string invalidSubject, string invalidHtml)
        {
            // given
            SendEmailMessage invalidEmailMessage = new SendEmailMessage 
            {
              Subject = invalidSubject,
              Html = invalidHtml,
            };


            var invalidEmailException = 
                new InvalidEmailException();

            invalidEmailException.AddData(
                key: nameof(SendEmailMessage.Html),
                values: "Text is required");

            invalidEmailException.AddData(
                key: nameof(SendEmailMessage.Subject),
                values: "Text is required");

            invalidEmailException.AddData(
                key: nameof(SendEmailMessage.From),
                values: "Value is required");

            invalidEmailException.AddData(
                key: nameof(SendEmailMessage.To),
                values: "Value is required");

            var expectedEmailValidationException =
              new EmailValidationException(
                  message: "Email validation error occurred, fix errors and try again.",
                  innerException: invalidEmailException);

            // when
            ValueTask<SendEmailResponse> EmailDetailsTask =
                this.emailService.SendEmailRequestAsync(invalidEmailMessage);

            EmailValidationException actualEmailValidationException =
                await Assert.ThrowsAsync<EmailValidationException>(EmailDetailsTask.AsTask);

            // then
            actualEmailValidationException.Should().BeEquivalentTo(
                expectedEmailValidationException);

            this.emailBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
        [Fact]
        public async Task ShouldThrowValidationExceptionSendGetEmailDetailsIfEmailDetailsIsEmptyAsync()
        {
            // given
            var emptyEmailMessage = new SendEmailMessage();
            emptyEmailMessage.Subject = string.Empty;
            emptyEmailMessage.Subject = string.Empty;

            var invalidEmailException = new InvalidEmailException();

            invalidEmailException.AddData(
                key: nameof(SendEmailMessage.Html),
                values: "Text is required");

            invalidEmailException.AddData(
                key: nameof(SendEmailMessage.Subject),
                values: "Text is required");

            invalidEmailException.AddData(
             key: nameof(SendEmailMessage.From),
             values: "Value is required");

            invalidEmailException.AddData(
                key: nameof(SendEmailMessage.To),
                values: "Value is required");

            var expectedEmailValidationException =
              new EmailValidationException(
                  message: "Email validation error occurred, fix errors and try again.",
                  innerException: invalidEmailException);

            // when
            ValueTask<SendEmailResponse> EmailDetailsTask =
                this.emailService.SendEmailRequestAsync(emptyEmailMessage);

            EmailValidationException actualEmailValidationException =
                await Assert.ThrowsAsync<EmailValidationException>(
                    EmailDetailsTask.AsTask);

            // then
            actualEmailValidationException.Should().BeEquivalentTo(
                expectedEmailValidationException);

            this.emailBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

    }
}