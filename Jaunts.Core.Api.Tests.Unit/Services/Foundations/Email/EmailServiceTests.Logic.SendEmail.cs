// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Models.Email;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Emails
{
    public partial class EmailServiceTests
    {
        [Fact]
        public async Task ShouldSendEmailAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            SendEmailMessage sendEmailMessage = CreateSendEmailDetailRequest();
            SendEmailResponse sendEmailResponse = CreateSendEmailResponse();

            SendEmailResponse expectedSendEmailResponse = sendEmailResponse;


            this.emailBrokerMock.Setup(broker =>
                broker.SendEmailAsync(sendEmailMessage))
                    .ReturnsAsync(sendEmailResponse);
            // when
            SendEmailResponse  actualEmail =
                await this.emailService.SendEmailRequestAsync(sendEmailMessage);

            // then
            actualEmail.Should().BeEquivalentTo(expectedSendEmailResponse);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.emailBrokerMock.Verify(broker =>
                broker.SendEmailAsync(sendEmailMessage),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.emailBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

    }
}
