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
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Processings.Email
{
    public partial class EmailProcessingServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnForgetPasswordMailIfUserIsNullAsync()
        {
            // given
            ApplicationUser nullUser = null;
            string nullToken = null;

            var nullEmailProcessingException =
                new NullEmailProcessingException();

            var expectedEmailProcessingValidationException =
                new EmailProcessingValidationException(
                    message: "Email validation error occurred, please try again.",
                    nullEmailProcessingException);

            // when
            ValueTask<SendEmailResponse> emailTask =
                this.emailProcessingService.SendForgetPasswordMailRequestAsync(nullUser,nullToken);

            EmailProcessingValidationException actualEmailProcessingValidationException =
                await Assert.ThrowsAsync<EmailProcessingValidationException>(
                    emailTask.AsTask);

            // then
            actualEmailProcessingValidationException.Should()
                .BeEquivalentTo(expectedEmailProcessingValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedEmailProcessingValidationException))),
                        Times.Once);

            this.emailTemplateSenderMock.Verify(service =>
                 service.SendVerificationEmailAsync(
                     It.IsAny<SendEmailMessage>(), It.IsAny<string>(), It.IsAny<string>(),
                     It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
                     Times.Never);

            this.emailServiceMock.Verify(broker =>
                broker.SendEmailRequestAsync(
                    It.IsAny<SendEmailMessage>()),
                        Times.Never);

            this.emailServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
