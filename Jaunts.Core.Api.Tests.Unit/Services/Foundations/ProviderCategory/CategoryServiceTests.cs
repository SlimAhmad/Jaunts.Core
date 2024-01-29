// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Brokers.DateTimes;
using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Brokers.Storages;
using Jaunts.Core.Api.Models.Services.Foundations.ProviderCategory;
using Jaunts.Core.Api.Services.Foundations.ProviderCategories;
using Microsoft.Data.SqlClient;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.ProviderCategories
{
    public partial class ProviderCategoryServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IProviderCategoryService providerCategoryService;

        public ProviderCategoryServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.providerCategoryService = new ProviderCategoryService(
                storageBroker: this.storageBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static DateTimeOffset GetCurrentDateTime() =>
            DateTimeOffset.UtcNow;

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static IEnumerable<ProviderCategory> CreateRandomProviderCategorys(DateTimeOffset dateTime) =>
            CreateRandomProviderCategoryFiller(dateTime).Create(GetRandomNumber());

        private static ProviderCategory CreateRandomProviderCategory(DateTimeOffset dateTime) =>
            CreateRandomProviderCategoryFiller(dateTime).Create();

        private static IQueryable<ProviderCategory> CreateRandomProviderCategories() =>
            CreateRandomProviderCategoryFiller(DateTimeOffset.UtcNow).Create(GetRandomNumber()).AsQueryable();

        private static SqlException GetSqlException() =>
            (SqlException)RuntimeHelpers.GetUninitializedObject(typeof(SqlException));

        private static int GetNegativeRandomNumber() => -1 * GetRandomNumber();
        private static string GetRandomMessage() => new MnemonicString().GetValue();

        private static Expression<Func<Exception, bool>> SameExceptionAs(Exception expectedException)
        {
            return actualException =>
                actualException.Message == expectedException.Message
                && actualException.InnerException.Message == expectedException.InnerException.Message;
        }

        private static Expression<Func<Exception, bool>> SameValidationExceptionAs(Exception expectedException)
        {
            return actualException =>
                actualException.Message == expectedException.Message
                && actualException.InnerException.Message == expectedException.InnerException.Message
                && (actualException.InnerException as Xeption).DataEquals(expectedException.InnerException.Data);
        }

        private static int GetRandomNumber() => new IntRange(min: 2, max: 10).GetValue();

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

        private static ProviderCategory CreateRandomModifyProviderCategory(DateTimeOffset dates)
        {
            int randomDaysInPast = GetNegativeRandomNumber();
            ProviderCategory randomProviderCategory = CreateRandomProviderCategory(dates);

            randomProviderCategory.CreatedDate =
                randomProviderCategory.CreatedDate.AddDays(randomDaysInPast);

            return randomProviderCategory;
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

        private static Filler<ProviderCategory> CreateRandomProviderCategoryFiller(DateTimeOffset dateTime)
        {
            var filler = new Filler<ProviderCategory>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTime);

            return filler;
        }
    }
}