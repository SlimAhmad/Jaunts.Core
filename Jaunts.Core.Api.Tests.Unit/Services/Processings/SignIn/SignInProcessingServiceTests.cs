// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Brokers.DateTimes;
using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Api.Models.SignIn.Exceptions;
using Jaunts.Core.Api.Services.Foundations.SignIn;
using Jaunts.Core.Api.Services.Processings.SignIn;
using Jaunts.Core.Authorization;
using Jaunts.Core.Models.Email;
using KellermanSoftware.CompareNetObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Processings.SignIn
{
    public partial class SignInProcessingServiceTests
    {
        private readonly Mock<ISignInService> signInServiceMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly ISignInProcessingService signInProcessingService;
        private readonly ICompareLogic compareLogic;

        public SignInProcessingServiceTests()
        {
            this.signInServiceMock = new Mock<ISignInService>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.compareLogic = new CompareLogic();

            this.signInProcessingService = new SignInProcessingService(
                signInService: this.signInServiceMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }



        private Expression<Func<Exception, bool>> SameExceptionAs(
           Exception expectedException)
        {
            return actualException =>
                this.compareLogic.Compare(
                    expectedException.InnerException.Message,
                    actualException.InnerException.Message)
                        .AreEqual;
        }

        private static string GetRandomString() => new MnemonicString(1, 8, 20).GetValue();

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();
        private static string GetRandomEmailAddresses() => new EmailAddresses().GetValue();
        private static DateTimeOffset GetCurrentDateTime() =>
            DateTimeOffset.UtcNow;
        private static string GetRandomNames() => new RealNames().GetValue();
        private static bool GetRandomBoolean() =>  Randomizer<bool>.Create();
        private static int GetRandomNumber() => new IntRange(min: 2, max: 90).GetValue();
        private static List<string> CreateRandomStringList() =>
         new Filler<List<string>>().Create();



        private static ApplicationUser CreateRandomUser()
        {
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                Email = GetRandomEmailAddresses(),
                LastName = GetRandomNames(),
                FirstName = GetRandomNames(),
                UserName = GetRandomNames(),
                PhoneNumber = GetRandomString(),
                CreatedDate = DateTimeOffset.UtcNow,
                UpdatedDate = DateTimeOffset.UtcNow

            };

            return user;
        }
        public static TheoryData DependencyValidationExceptions()
        {
            var innerException = new Xeption();

            return new TheoryData<Xeption>
            {
                new SignInValidationException(innerException),
                new SignInDependencyValidationException(innerException)
            };
        }

        public static TheoryData DependencyExceptions()
        {
            var innerException = new Xeption();

            return new TheoryData<Xeption>
            {
                new SignInDependencyException(innerException),
                new SignInServiceException(innerException)
            };
        }
    }
}
