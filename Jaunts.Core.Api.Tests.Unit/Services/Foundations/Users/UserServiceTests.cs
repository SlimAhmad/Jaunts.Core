// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Jaunts.Core.Api.Brokers.DateTimes;
using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Brokers.UserManagement;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Api.Services.Foundations.Users;
using KellermanSoftware.CompareNetObjects;
using Microsoft.Data.SqlClient;
using Moq;
using Tynamix.ObjectFiller;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Users
{
    public partial class UserServiceTests
    {
        private readonly Mock<IUserManagementBroker> userManagementBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IUserService userService;
        private readonly ICompareLogic compareLogic;

        public UserServiceTests()
        {
            this.userManagementBrokerMock = new Mock<IUserManagementBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.compareLogic = new CompareLogic();

            this.userService = new UserService(
                userManagementBroker: this.userManagementBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object,
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

        private static string GetRandomPassword() => new MnemonicString(1, 8, 20).GetValue();
        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();
        private static DateTimeOffset GetCurrentDateTime() =>  DateTimeOffset.UtcNow;
        private static string GetRandomNames() => new RealNames().GetValue();
        private static string GetRandomEmailAddresses() => new EmailAddresses().GetValue();
        private static int GetRandomNumber() => new IntRange(min: 2, max: 90).GetValue();
        private static int GetRandomNegativeNumber() =>
            -1 * new IntRange(min: 2, max: 10).GetValue();
        private static string GetRandomMessage() => new MnemonicString().GetValue();
        private static string GetRandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();
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
                PhoneNumber = GetRandomMessage(),
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
                PhoneNumber = GetRandomMessage(),
                CreatedDate = dates,
                UpdatedDate = dates
            };

            return user;
        }

        private static ApplicationUser CreateRandomModifyUser(DateTimeOffset dates)
        {
            int randomDaysInPast = GetRandomNegativeNumber();
            ApplicationUser randomUser = CreateRandomUser(dates);

            randomUser.CreatedDate =
                randomUser.CreatedDate.AddDays(randomDaysInPast);

            return randomUser;
        }

        private static IQueryable<ApplicationUser> CreateRandomUsers(DateTimeOffset dates)
        {
            var users = new List<ApplicationUser>();
            for (int i = 0; i < GetRandomNumber(); i++)
            {
                var user = new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    Email = GetRandomEmailAddresses(),
                    LastName = GetRandomNames(),
                    FirstName = GetRandomNames(),
                    UserName = GetRandomNames(),
                    PhoneNumber = GetRandomMessage(),
                    CreatedDate = dates,
                    UpdatedDate = dates
                };

                users.Add(user);
            }

            return users.AsQueryable();
        }

        public static TheoryData InvalidMinuteCases()
        {
            int randomMoreThanMinuteFromNow = GetRandomNumber();
            int randomMoreThanMinuteBeforeNow = GetRandomNegativeNumber();

            return new TheoryData<int>
            {
                randomMoreThanMinuteFromNow,
                randomMoreThanMinuteBeforeNow
            };
        }
    }
}
