// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Models.Auth;
using Jaunts.Core.Api.Models.Jwt.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Api.Services.Orchestration.Jwt;
using Jaunts.Core.Api.Services.Processings.Jwt;
using Jaunts.Core.Api.Services.Processings.Role;
using Jaunts.Core.Api.Services.Processings.User;
using KellermanSoftware.CompareNetObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Orchestrations.Jwt
{
    public partial class JwtOrchestrationServiceTests
    {
        private readonly Mock<IJwtProcessingService> jwtProcessingServiceMock;
        private readonly Mock<IUserProcessingService> userProcessingServiceMock;
        private readonly Mock<IRoleProcessingService> roleProcessingServiceMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IJwtOrchestrationService jwtOrchestrationService;
        private readonly ICompareLogic compareLogic;
        public JwtOrchestrationServiceTests()
        {
            this.jwtProcessingServiceMock = new Mock<IJwtProcessingService>();
            this.userProcessingServiceMock = new Mock<IUserProcessingService>();
            this.roleProcessingServiceMock = new Mock<IRoleProcessingService>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.compareLogic = new CompareLogic();

            this.jwtOrchestrationService = new JwtOrchestrationService(
                 this.jwtProcessingServiceMock.Object,
                 this.userProcessingServiceMock.Object, 
                 this.roleProcessingServiceMock.Object,
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

        private static string GetRandomPassword() => new MnemonicString(1, 8, 20).GetValue();
        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();
        private static DateTimeOffset GetCurrentDateTime() => DateTimeOffset.UtcNow;
        private static string GetRandomNames() => new RealNames().GetValue();
        private static string GetRandomEmailAddresses() => new EmailAddresses().GetValue();
        private static int GetRandomNumber() => new IntRange(min: 2, max: 90).GetValue();
        private static bool GetRandomBoolean() =>
                Randomizer<bool>.Create();
        private static int GetRandomNegativeNumber() =>
            -1 * new IntRange(min: 2, max: 10).GetValue();
        private static string GetRandomMessage() => new MnemonicString().GetValue();
        private static string GetRandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();
        private static List<string> CreateRandomStringList() =>
             new Filler<List<string>>().Create();


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

        private UserAccountDetailsResponse ConvertToAccountDetailsResponse(ApplicationUser user, string token, List<string> role)
        {

            return new UserAccountDetailsResponse
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Id = user.Id.ToString(),
                Token = token,
                Role = role,
                TwoFactorEnabled = user.TwoFactorEnabled,
                EmailConfirmed = user.EmailConfirmed,
            };
        }


        public static TheoryData DependencyValidationExceptions()
        {
            var innerException = new Xeption();

            return new TheoryData<Xeption>
            {
                new JwtValidationException(innerException),
                new JwtDependencyValidationException(innerException)
            };
        }

        public static TheoryData DependencyExceptions()
        {
            var innerException = new Xeption();

            return new TheoryData<Xeption>
            {
                new JwtDependencyException(innerException),
                new JwtServiceException(innerException)
            };
        }

    }
}
