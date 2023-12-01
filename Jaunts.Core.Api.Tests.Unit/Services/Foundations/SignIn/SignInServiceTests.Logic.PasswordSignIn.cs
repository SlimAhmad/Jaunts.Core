// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.SignIn
{
    public partial class SignInServiceTests
    {
        [Fact]
        public async Task ShouldPasswordSignInAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            ApplicationUser randomUser = CreateRandomUser(dateTime);
            ApplicationUser inputUser = randomUser;
            string randomPassword = GetRandomPassword();
            string inputPassword = randomPassword;
            bool randomBoolean = GetRandomBoolean();
            bool inputBoolean = randomBoolean;

            this.signInBrokerMock.Setup(broker =>
                broker.PasswordSignInAsync(inputUser,inputPassword,inputBoolean,inputBoolean))
                    .ReturnsAsync(new Microsoft.AspNetCore.Identity.SignInResult());
            // when
            SignInResult actualSignIn =
                await this.signInService.PasswordSignInRequestAsync(inputUser,inputPassword,inputBoolean,inputBoolean);

            // then
            actualSignIn.Should().BeEquivalentTo(new SignInResult());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.signInBrokerMock.Verify(broker =>
                broker.PasswordSignInAsync(inputUser,inputPassword,inputBoolean,inputBoolean),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.signInBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

    }
}
