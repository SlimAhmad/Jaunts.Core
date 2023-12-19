// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Models.Auth;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Api.Models.User.Exceptions;
using Jaunts.Core.Api.Services.Foundations.Users;
using Jaunts.Core.Api.Services.Processings.User;
using Jaunts.Core.Models.Auth.LoginRegister;
using KellermanSoftware.CompareNetObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Processings.Users
{
    public partial class UserProcessingServiceTests
    {
        private readonly Mock<IUserService> userServiceMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IUserProcessingService userProcessingService;
        private readonly ICompareLogic compareLogic;

        public UserProcessingServiceTests()
        {
            this.userServiceMock = new Mock<IUserService>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.compareLogic = new CompareLogic();

            this.userProcessingService = new UserProcessingService(
                userService: this.userServiceMock.Object,
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
        private static Guid GetRandomGuid() => Guid.NewGuid();
        private static bool GetRandomBoolean() => Randomizer<bool>.Create();
        private static string GetRandomEmailAddresses() => new EmailAddresses().GetValue();
        private static int GetRandomNumber() => new IntRange(min: 2, max: 90).GetValue();
        private static int GetRandomNegativeNumber() =>
            -1 * new IntRange(min: 2, max: 10).GetValue();
        private static string GetRandomMessage() => new MnemonicString().GetValue();
        private static string GetRandomString() =>
            new MnemonicString().GetValue();
        private static List<string> CreateRandomStringList() =>
            new Filler<List<string>>().Create();
        private static LoginRequest CreateRandomLoginRequest(ApplicationUser user) =>
            CreateLoginFiller(user).Create();
        private static ResetPasswordApiRequest CreateRandomResetPasswordRequest(ApplicationUser user) =>
            CreateResetPasswordFiller(user).Create();

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

        private static IQueryable<ApplicationUser> CreateRandomUsers()
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
                    CreatedDate = DateTimeOffset.UtcNow,
                    UpdatedDate = DateTimeOffset.UtcNow
                };

                users.Add(user);
            }

            return users.AsQueryable();
        }

        private static IQueryable<ApplicationUser> CreateRandomUsers(ApplicationUser roles)
        {
            List<ApplicationUser> randomUsers =
                CreateRandomUsers().ToList();

            randomUsers.Add(roles);

            return randomUsers.AsQueryable();
        }

        private static Filler<LoginRequest> CreateLoginFiller(ApplicationUser user)
        {
            var filler = new Filler<LoginRequest>();

            filler.Setup()
                .OnProperty(x => x.UsernameOrEmail).Use(user.Email)
                .OnType<DateTimeOffset>().Use(GetCurrentDateTime());

            return filler;
        }

        private static Filler<ResetPasswordApiRequest> CreateResetPasswordFiller(ApplicationUser user)
        {
            var filler = new Filler<ResetPasswordApiRequest>();

            filler.Setup()
                .OnProperty(x => x.Email).Use(user.Email)
                .OnType<DateTimeOffset>().Use(GetCurrentDateTime());

            return filler;
        }

        public static TheoryData DependencyValidationExceptions()
        {
            var innerException = new Xeption();

            return new TheoryData<Xeption>
            {
                new UserValidationException(innerException),
                new UserDependencyValidationException(innerException)
            };
        }

        public static TheoryData DependencyExceptions()
        {
            var innerException = new Xeption();

            return new TheoryData<Xeption>
            {
                new UserDependencyException(innerException),
                new UserServiceException(innerException)
            };
        }
    }
}
