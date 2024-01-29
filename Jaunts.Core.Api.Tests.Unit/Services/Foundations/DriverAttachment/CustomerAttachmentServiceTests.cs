// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Brokers.DateTimes;
using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Brokers.Storages;
using Jaunts.Core.Api.Models.Services.Foundations.DriverAttachments;
using Jaunts.Core.Api.Services.Foundations.DriverAttachments;
using Microsoft.Data.SqlClient;
using Moq;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Tynamix.ObjectFiller;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.DriverAttachments
{
    public partial class DriverAttachmentServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IDriverAttachmentService driverAttachmentService;

        public DriverAttachmentServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.driverAttachmentService = new DriverAttachmentService(
                storageBroker: this.storageBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static IQueryable<DriverAttachment> CreateRandomDriverAttachments() =>
            CreateDriverAttachmentFiller(DateTimeOffset.UtcNow)
            .Create(GetRandomNumber()).AsQueryable();

        private static string GetRandomMessage() => new MnemonicString().GetValue();

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static DriverAttachment CreateRandomDriverAttachment() =>
            CreateDriverAttachmentFiller(DateTimeOffset.UtcNow).Create();

        private static DriverAttachment CreateRandomDriverAttachment(DateTimeOffset dates) =>
            CreateDriverAttachmentFiller(dates).Create();

        private static Expression<Func<Exception, bool>> SameExceptionAs(Exception expectedException)
        {
            return actualException =>
                actualException.Message == expectedException.Message
                && actualException.InnerException.Message == expectedException.InnerException.Message;
        }

        private static SqlException GetSqlException() =>
            (SqlException)RuntimeHelpers.GetUninitializedObject(typeof(SqlException));

        private static int GetRandomNumber() => new IntRange(min: 2, max: 150).GetValue();

        private static Filler<DriverAttachment> CreateDriverAttachmentFiller(DateTimeOffset dates)
        {
            var filler = new Filler<DriverAttachment>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dates)
                .OnProperty(DriverAttachment => DriverAttachment.Driver).IgnoreIt()
                .OnProperty(DriverAttachment => DriverAttachment.Attachment).IgnoreIt();

            return filler;
        }
    }
}
