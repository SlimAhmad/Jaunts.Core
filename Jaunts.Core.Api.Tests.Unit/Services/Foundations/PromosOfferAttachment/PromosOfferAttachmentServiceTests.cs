// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Brokers.DateTimes;
using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Brokers.Storages;
using Jaunts.Core.Api.Models.Services.Foundations.PromosOfferAttachments;
using Jaunts.Core.Api.Services.Foundations.PromosOfferAttachments;
using Microsoft.Data.SqlClient;
using Moq;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Tynamix.ObjectFiller;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.PromosOfferAttachments
{
    public partial class PromosOfferAttachmentServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IPromosOfferAttachmentService promosOfferAttachmentService;

        public PromosOfferAttachmentServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.promosOfferAttachmentService = new PromosOfferAttachmentService(
                storageBroker: this.storageBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static IQueryable<PromosOfferAttachment> CreateRandomPromosOfferAttachments() =>
            CreatePromosOfferAttachmentFiller(DateTimeOffset.UtcNow)
            .Create(GetRandomNumber()).AsQueryable();

        private static string GetRandomMessage() => new MnemonicString().GetValue();

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static PromosOfferAttachment CreateRandomPromosOfferAttachment() =>
            CreatePromosOfferAttachmentFiller(DateTimeOffset.UtcNow).Create();

        private static PromosOfferAttachment CreateRandomPromosOfferAttachment(DateTimeOffset dates) =>
            CreatePromosOfferAttachmentFiller(dates).Create();

        private static Expression<Func<Exception, bool>> SameExceptionAs(Exception expectedException)
        {
            return actualException =>
                actualException.Message == expectedException.Message
                && actualException.InnerException.Message == expectedException.InnerException.Message;
        }

        private static SqlException GetSqlException() =>
            (SqlException)RuntimeHelpers.GetUninitializedObject(typeof(SqlException));

        private static int GetRandomNumber() => new IntRange(min: 2, max: 150).GetValue();

        private static Filler<PromosOfferAttachment> CreatePromosOfferAttachmentFiller(DateTimeOffset dates)
        {
            var filler = new Filler<PromosOfferAttachment>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dates)
                .OnProperty(PromosOfferAttachment => PromosOfferAttachment.PromosOffer).IgnoreIt()
                .OnProperty(PromosOfferAttachment => PromosOfferAttachment.Attachment).IgnoreIt();

            return filler;
        }
    }
}
