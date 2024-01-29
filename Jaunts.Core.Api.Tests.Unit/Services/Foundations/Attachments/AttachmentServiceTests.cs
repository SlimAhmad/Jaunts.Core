// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Brokers.DateTimes;
using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Brokers.Storages;
using Jaunts.Core.Api.Models.Services.Foundations.Attachments;
using Jaunts.Core.Api.Services.Foundations.Attachments;
using Microsoft.Data.SqlClient;
using Moq;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Tynamix.ObjectFiller;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Attachments
{
    public partial class AttachmentServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly AttachmentService attachmentService;

        public AttachmentServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.attachmentService = new AttachmentService(
                storageBroker: this.storageBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static DateTimeOffset GetCurrentDateTime() =>
            DateTimeOffset.UtcNow;

        private static IQueryable<Attachment> CreateRandomAttachments() =>
            CreateAttachmentFiller(dates: GetRandomDateTime())
            .Create(GetRandomNumber()).AsQueryable();

        private static Attachment CreateRandomAttachment() =>
            CreateAttachmentFiller(dates: GetRandomDateTime()).Create();

        private static Attachment CreateRandomAttachment(DateTimeOffset dates) =>
            CreateAttachmentFiller(dates).Create();

        private static Expression<Func<Exception, bool>> SameExceptionAs(Exception expectedException)
        {
            return actualException =>
                actualException.Message == expectedException.Message
                && actualException.InnerException.Message == expectedException.InnerException.Message;
        }

        private static T GetInvalidEnum<T>()
        {
            int randomNumber = GetRandomNumber();

            while (Enum.IsDefined(typeof(T), randomNumber) is true)
            {
                randomNumber = GetLocalRandomNumber();
            }

            return (T)(object)randomNumber;

            static int GetLocalRandomNumber() =>
                new IntRange(min: int.MinValue, max: int.MaxValue).GetValue();
        }

        private static SqlException GetSqlException() =>
             (SqlException)RuntimeHelpers.GetUninitializedObject(typeof(SqlException));

        public static TheoryData InvalidMinuteCases()
        {
            int randomMoreThanMinuteFromNow = GetRandomNumber();
            int randomMoreThanMinuteBeforeNow = GetNegativeRandomNumber();

            return new TheoryData<int>
            {
                randomMoreThanMinuteFromNow,
                randomMoreThanMinuteBeforeNow
            };
        }

        private static Attachment CreateRandomModifyAttachment(DateTimeOffset dates)
        {
            int randomDaysInPast = GetNegativeRandomNumber();
            Attachment randomAttachment = CreateRandomAttachment(dates);

            randomAttachment.CreatedDate =
                randomAttachment.CreatedDate.AddDays(randomDaysInPast);

            return randomAttachment;
        }

        private static int GetRandomNumber() => new IntRange(min: 2, max: 150).GetValue();
        private static int GetNegativeRandomNumber() => -1 * GetRandomNumber();
        private static string GetRandomMessage() => new MnemonicString().GetValue();

        private static Filler<Attachment> CreateAttachmentFiller(DateTimeOffset dates)
        {
            var filler = new Filler<Attachment>();

            filler.Setup()
                .OnProperty(attachment => attachment.CreatedDate).Use(dates)
                .OnProperty(attachment => attachment.UpdatedDate).Use(dates)
                .OnProperty(attachment => attachment.CustomerAttachments).IgnoreIt()
                .OnProperty(attachment => attachment.ProviderAttachments).IgnoreIt()
                .OnProperty(attachment => attachment.PackageAttachments).IgnoreIt()
                .OnProperty(attachment => attachment.ProvidersDirectorsAttachments).IgnoreIt()
                .OnProperty(attachment => attachment.RideAttachments).IgnoreIt()
                .OnProperty(attachment => attachment.AdvertAttachments).IgnoreIt()
                .OnProperty(attachment => attachment.PromosOfferAttachment).IgnoreIt()
                .OnProperty(attachment => attachment.FlightDealAttachment).IgnoreIt()
                .OnProperty(attachment => attachment.ShortLetAttachments).IgnoreIt()
                .OnProperty(attachment => attachment.DriverAttachments).IgnoreIt();

            return filler;
        }
    }
}
