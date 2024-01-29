// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Brokers.DateTimes;
using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Brokers.Storages;
using Jaunts.Core.Api.Models.Services.Foundations.Amenities;
using Jaunts.Core.Api.Services.Foundations.Amenities;
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

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Amenities
{
    public partial class AmenityServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IAmenityService amenityService;

        public AmenityServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.amenityService = new AmenityService(
                storageBroker: this.storageBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static DateTimeOffset GetCurrentDateTime() =>
            DateTimeOffset.UtcNow;

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static IEnumerable<Amenity> CreateRandomAmenities(DateTimeOffset dateTime) =>
            CreateRandomAmenityFiller(dateTime).Create(GetRandomNumber());

        private static Amenity CreateRandomAmenity(DateTimeOffset dateTime) =>
            CreateRandomAmenityFiller(dateTime).Create();

        private static IQueryable<Amenity> CreateRandomAmenities() =>
            CreateRandomAmenityFiller(DateTimeOffset.UtcNow).Create(GetRandomNumber()).AsQueryable();

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

        private static Amenity CreateRandomModifyAmenity(DateTimeOffset dates)
        {
            int randomDaysInPast = GetNegativeRandomNumber();
            Amenity randomAmenity = CreateRandomAmenity(dates);

            randomAmenity.CreatedDate =
                randomAmenity.CreatedDate.AddDays(randomDaysInPast);

            return randomAmenity;
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

        private static Filler<Amenity> CreateRandomAmenityFiller(DateTimeOffset dateTime)
        {
            var filler = new Filler<Amenity>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTime);

            return filler;
        }
    }
}