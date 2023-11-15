// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Models.Email;
using Jaunts.Core.Models.Exceptions;
using Moq;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Emails
{
    public partial class EmailServiceTests
    {

        [Fact]
        public async Task ShouldThrowValidationExceptionOnVerificationMailIfParametersIsNullAsync()
        {
            // given
            ApplicationUser nullUser = null;
            string nullText = null;
            var nullUserException = new NullEmailException();

            var exceptedEmailValidationException =
                new EmailValidationException(nullUserException);

            // when
            ValueTask<SendEmailResponse> emailTask =
                this.emailService.PostVerificationMailRequestAsync(nullUser, nullText, nullText, nullText, nullText);

            EmailValidationException actualEmailValidationException =
                await Assert.ThrowsAsync<EmailValidationException>(
                    emailTask.AsTask);

            // then
            actualEmailValidationException.Should()
                .BeEquivalentTo(exceptedEmailValidationException);

            this.emailBrokerMock.Verify(broker =>
                broker.PostMailAsync(
                    It.IsAny<SendEmailDetails>()),
                        Times.Never);

            this.emailBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }


        [Theory]
        [InlineData(null, null, null, null, null, null, null)]
        [InlineData("", "", "", "", "", "", "")]
        [InlineData("  ", "  ", " ", " ", " ", " ", " ")]
        public async Task ShouldThrowValidationExceptionOnPostVerificationMailIfParametersIsInvalidAsync(
           string invalidFirstName, string invalidLastName, string invalidEmail,
           string invalidSubject, string invalidFrom, string invalidToken, string invalidFromName)
        {
            // given
            var invalidUser = new ApplicationUser
            {
                FirstName = invalidFirstName,
                LastName = invalidLastName,
                Email = invalidEmail
            };


            var invalidUserException = new InvalidEmailException();

            invalidUserException.AddData(
               key: nameof(ApplicationUser.FirstName),
               values: "Text is required");

            invalidUserException.AddData(
               key: nameof(ApplicationUser.LastName),
               values: "Text is required");

            invalidUserException.AddData(
               key: nameof(ApplicationUser.Email),
               values: "Text is required");



            var expectedEmailValidationException =
                new EmailValidationException(invalidUserException);

            // when
            ValueTask<SendEmailResponse> emailTask =
                this.emailService.PostVerificationMailRequestAsync(invalidUser, invalidSubject, invalidToken, invalidFrom, invalidFromName);

            EmailValidationException actualEmailValidationException =
                await Assert.ThrowsAsync<EmailValidationException>(emailTask.AsTask);

            // then
            actualEmailValidationException.Should().BeEquivalentTo(
                expectedEmailValidationException);

            this.emailBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnPostVerificationMailIfParametersIsEmptyAsync()
        {
            // given
            var emptyUser = new ApplicationUser
            {
                FirstName = string.Empty,
                LastName = string.Empty,
                Email = string.Empty
            };

            string emptySubject = string.Empty;
            string emptyFrom = string.Empty;
            string emptyFromName = string.Empty;
            string emptyToken = string.Empty;

            var invalidUserException = new InvalidEmailException();

            invalidUserException.AddData(
                      key: nameof(ApplicationUser.FirstName),
                      values: "Text is required");

            invalidUserException.AddData(
               key: nameof(ApplicationUser.LastName),
               values: "Text is required");

            invalidUserException.AddData(
               key: nameof(ApplicationUser.Email),
               values: "Text is required");



            var expectedEmailValidationException =
                new EmailValidationException(invalidUserException);

            // when
            ValueTask<SendEmailResponse> emailTask =
                this.emailService.PostVerificationMailRequestAsync(emptyUser, emptySubject, emptyToken, emptyFrom, emptyFromName);

            EmailValidationException actualEmailValidationException =
                await Assert.ThrowsAsync<EmailValidationException>(
                    emailTask.AsTask);

            // then
            actualEmailValidationException.Should().BeEquivalentTo(
                expectedEmailValidationException);

            this.emailBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

    }
}