// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Castle.Core.Configuration;
using Jaunts.Core.Api.Brokers.DateTimes;
using Jaunts.Core.Api.Brokers.EmailBroker;
using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Brokers.UserManagement;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Api.Services.Foundations.Email;
using Jaunts.Core.Api.Services.Processings.Email;
using Jaunts.Core.Models.AppSettings;
using Jaunts.Core.Models.Email;
using Jaunts.Core.Models.Exceptions;
using KellermanSoftware.CompareNetObjects;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Moq;
using RESTFulSense.Exceptions;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Processings.Email
{
    public partial class EmailProcessingServiceTests
    {
        private readonly Mock<IEmailTemplateSender> emailTemplateSenderMock;
        private readonly Mock<IEmailService> emailServiceMock;
        private readonly Mock<IOptions<MailTrap>>  configurationMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IEmailProcessingService emailProcessingService;
        private readonly ICompareLogic compareLogic;

        public EmailProcessingServiceTests()
        {
            this.emailTemplateSenderMock = new Mock<IEmailTemplateSender>();
            this.emailServiceMock = new Mock<IEmailService>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.configurationMock = new Mock<IOptions<MailTrap>>();
            this.compareLogic = new CompareLogic();
            this.configurationMock.Setup(x => x.Value).Returns(mailTrap);

            this.emailProcessingService = new EmailProcessingService(
                 this.emailServiceMock.Object,
                 this.loggingBrokerMock.Object,
                 this.emailTemplateSenderMock.Object,
                 this.configurationMock.Object);
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


        private static SendEmailMessage CreateSendEmailDetailRequest() =>
          CreateSendEmailDetailsFiller().Create();
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

        private static Filler<SendEmailMessage> CreateSendEmailDetailsFiller()
        {
            var filler = new Filler<SendEmailMessage>();

            filler.Setup()
                .OnProperty(x=> x.From.Email).Use(GetRandomEmailAddresses())
                .OnType<DateTimeOffset>().IgnoreIt();

            return filler;
        }

        MailTrap mailTrap = new MailTrap
        {
            Email = GetRandomEmailAddresses(),
            InboxId = new Guid().ToString(),
            TestUrl = GetRandomString(),
            Name = GetRandomString(),
            Token = GetRandomString(),
            Url = GetRandomString(),
        };

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

        private static ApplicationUser CreateRandomUser(DateTimeOffset dates)
        {
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                Email = GetRandomEmailAddresses(),
                LastName = GetRandomNames(),
                FirstName = GetRandomNames(),
                UserName = GetRandomNames(),
                PhoneNumber = GetRandomSubject(),
                CreatedDate = dates,
                UpdatedDate = dates
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
