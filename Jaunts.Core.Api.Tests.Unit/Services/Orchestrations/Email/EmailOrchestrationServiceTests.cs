// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Api.Services.Foundations.Email;
using Jaunts.Core.Api.Services.Orchestration.Email;
using Jaunts.Core.Api.Services.Processings.Email;
using Jaunts.Core.Api.Services.Processings.User;
using Jaunts.Core.Models.AppSettings;
using Jaunts.Core.Models.Email;
using Jaunts.Core.Models.Exceptions;
using KellermanSoftware.CompareNetObjects;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Linq.Expressions;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Orchestrations.Email
{
    public partial class EmailOrchestrationServiceTests
    {
        private readonly Mock<IEmailProcessingService> emailProcessingServiceMock;
        private readonly Mock<IUserProcessingService> userProcessingServiceMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IEmailOrchestrationService emailOrchestrationService;
        private readonly ICompareLogic compareLogic;

        public EmailOrchestrationServiceTests()
        {
            this.emailProcessingServiceMock = new Mock<IEmailProcessingService>();
            this.userProcessingServiceMock = new Mock<IUserProcessingService>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.compareLogic = new CompareLogic();

            this.emailOrchestrationService = new EmailOrchestrationService(
                 this.emailProcessingServiceMock.Object,
                 this.userProcessingServiceMock.Object,
                 this.loggingBrokerMock.Object);
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

        private static SendEmailResponse CreateSendEmailResponse() =>
          CreateSendEmailResponseFiller().Create();

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static string GetRandomNames() => new RealNames().GetValue();
        private static string GetRandomEmailAddresses() => new EmailAddresses().GetValue();
        private static bool GetRandomBoolean() => Randomizer<bool>.Create();
        private static string GetRandomString() => new MnemonicString(1, 8, 20).GetValue();
        private static string GetRandomSubject() => new MnemonicString().GetValue();

        private static Filler<SendEmailResponse> CreateSendEmailResponseFiller()
        {
            var filler = new Filler<SendEmailResponse>();

            filler.Setup()
                .OnProperty(x=> x.Errors).IgnoreIt()
                .OnType<DateTimeOffset>().IgnoreIt();

            return filler;
        }

        private static ApplicationUser CreateRandomUser()
        {
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                Email = GetRandomEmailAddresses(),
                LastName = GetRandomNames(),
                FirstName = GetRandomNames(),
                UserName = GetRandomNames(),
                PhoneNumber = GetRandomSubject(),
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
                new EmailValidationException(innerException),
                new EmailDependencyValidationException(innerException)
            };
        }

        public static TheoryData DependencyExceptions()
        {
            var innerException = new Xeption();

            return new TheoryData<Xeption>
            {
                new EmailDependencyException(innerException),
                new EmailServiceException(innerException)
            };
        }
    }
}
