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
using Jaunts.Core.Models.Email;
using KellermanSoftware.CompareNetObjects;
using Microsoft.Data.SqlClient;
using Moq;
using RESTFulSense.Exceptions;
using Tynamix.ObjectFiller;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Emails
{
    public partial class EmailServiceTests
    {
        private readonly Mock<IEmailBroker> emailBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IEmailService emailService;
        private readonly ICompareLogic compareLogic;
        public EmailServiceTests()
        {
            this.emailBrokerMock = new Mock<IEmailBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.compareLogic = new CompareLogic();

            this.emailService = new EmailService(
                 this.emailBrokerMock.Object,
                 this.dateTimeBrokerMock.Object,
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

        private static SendEmailDetails CreateSendEmailDetailRequest() =>
          CreateSendEmailDetailsFiller().Create();
        private static SendEmailResponse CreateSendEmailResponse() =>
          CreateSendEmailResponseFiller().Create();

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static string GetRandomNames() => new RealNames().GetValue();
        private static string GetRandomEmailAddresses() => new EmailAddresses().GetValue();
        private static int GetRandomNumber() => new IntRange(min: 2, max: 90).GetValue();
        private static int GetNegativeRandomNumber() => -1 * GetRandomNumber();
        private static string GetRandomText() => new MnemonicString(1, 8, 20).GetValue();
        private static string GetRandomSubject() => new MnemonicString().GetValue();
        private static SqlException GetSqlException() =>
            (SqlException)RuntimeHelpers.GetUninitializedObject(typeof(SqlException));

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

        private static Filler<SendEmailResponse> CreateSendEmailResponseFiller()
        {
            var filler = new Filler<SendEmailResponse>();

            filler.Setup()
                .OnProperty(x=> x.Errors).IgnoreIt()
                .OnType<DateTimeOffset>().IgnoreIt();

            return filler;
        }

        private static Filler<SendEmailDetails> CreateSendEmailDetailsFiller()
        {
            var filler = new Filler<SendEmailDetails>();

            filler.Setup()
                .OnProperty(x=> x.From.Email).Use(GetRandomEmailAddresses())
                .OnType<DateTimeOffset>().IgnoreIt();

            return filler;
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

        public static TheoryData UnauthorizedExceptions()
        {
            return new TheoryData<HttpResponseException>
            {
                new HttpResponseUnauthorizedException(),
                new HttpResponseForbiddenException()
            };
        }
    }
}
