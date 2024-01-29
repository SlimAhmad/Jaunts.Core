// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Brokers.DateTimes;
using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Brokers.Storages;
using Jaunts.Core.Api.Models.Services.Foundations.ShortLetAttachments;
using Jaunts.Core.Api.Services.Foundations.ShortLetAttachments;
using Microsoft.Data.SqlClient;
using Moq;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Tynamix.ObjectFiller;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.ShortLetAttachments
{
    public partial class ShortLetAttachmentServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IShortLetAttachmentService shortLetAttachmentService;

        public ShortLetAttachmentServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.shortLetAttachmentService = new ShortLetAttachmentService(
                storageBroker: this.storageBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static IQueryable<ShortLetAttachment> CreateRandomShortLetAttachments() =>
            CreateShortLetAttachmentFiller(DateTimeOffset.UtcNow)
            .Create(GetRandomNumber()).AsQueryable();

        private static string GetRandomMessage() => new MnemonicString().GetValue();

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static ShortLetAttachment CreateRandomShortLetAttachment() =>
            CreateShortLetAttachmentFiller(DateTimeOffset.UtcNow).Create();

        private static ShortLetAttachment CreateRandomShortLetAttachment(DateTimeOffset dates) =>
            CreateShortLetAttachmentFiller(dates).Create();

        private static Expression<Func<Exception, bool>> SameExceptionAs(Exception expectedException)
        {
            return actualException =>
                actualException.Message == expectedException.Message
                && actualException.InnerException.Message == expectedException.InnerException.Message;
        }

        private static SqlException GetSqlException() =>
            (SqlException)RuntimeHelpers.GetUninitializedObject(typeof(SqlException));

        private static int GetRandomNumber() => new IntRange(min: 2, max: 150).GetValue();

        private static Filler<ShortLetAttachment> CreateShortLetAttachmentFiller(DateTimeOffset dates)
        {
            var filler = new Filler<ShortLetAttachment>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dates)
                .OnProperty(ShortLetAttachment => ShortLetAttachment.ShortLet).IgnoreIt()
                .OnProperty(ShortLetAttachment => ShortLetAttachment.Attachment).IgnoreIt();

            return filler;
        }
    }
}
