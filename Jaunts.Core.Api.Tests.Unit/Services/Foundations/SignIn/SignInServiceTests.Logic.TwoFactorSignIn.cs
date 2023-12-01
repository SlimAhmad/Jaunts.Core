// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.SignIn
{
    public partial class SignInServiceTests
    {
        [Fact]
        public async Task ShouldTwoFactorSignInAsync()
        {
            // given
            string randomProvider = GetRandomString();
            string inputProvider = randomProvider;
            string randomCode = GetRandomString();
            string inputCode = randomCode;
            bool randomBoolean = GetRandomBoolean();
            bool inputBoolean = randomBoolean;

            this.signInBrokerMock.Setup(broker =>
                broker.TwoFactorSignInAsync(inputProvider,inputCode,inputBoolean,inputBoolean))
                    .ReturnsAsync(new Microsoft.AspNetCore.Identity.SignInResult());
            // when
            SignInResult actualSignIn =
                await this.signInService.TwoFactorSignInRequestAsync(inputProvider,inputCode,inputBoolean,inputBoolean);

            // then
            actualSignIn.Should().BeEquivalentTo(new SignInResult());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.signInBrokerMock.Verify(broker =>
                broker.TwoFactorSignInAsync(inputProvider,inputCode,inputBoolean,inputBoolean),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.signInBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

    }
}
