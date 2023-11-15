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
using Jaunts.Core.Api.Brokers.RoleManagement;
using Jaunts.Core.Api.Brokers.SignInManagement;
using Jaunts.Core.Api.Brokers.UserManagement;
using Jaunts.Core.Api.Models.Auth;
using Jaunts.Core.Api.Models.Services.Foundations.Role;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Api.Services.Foundations.Auth;
using Jaunts.Core.Api.Services.Foundations.Email;
using Jaunts.Core.Models.Auth.LoginRegister;
using KellermanSoftware.CompareNetObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Moq;
using RESTFulSense.Exceptions;
using Tynamix.ObjectFiller;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Auth
{
    public partial class AuthServiceTests
    {
        private readonly Mock<IUserManagementBroker> userManagementBrokerMock;
        private readonly Mock<ISignInManagementBroker> signInManagerBrokerMock;
        private readonly Mock<IRoleManagementBroker> roleManagerBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IAuthService authService;
        private readonly IConfiguration configuration;
        private readonly ICompareLogic compareLogic;
        private readonly IEmailService emailService;

        public AuthServiceTests()
        {
            this.userManagementBrokerMock = new Mock<IUserManagementBroker>();
            this.signInManagerBrokerMock = new Mock<ISignInManagementBroker>();
            this.roleManagerBrokerMock = new Mock<IRoleManagementBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.compareLogic = new CompareLogic();

            this.authService = new AuthService(
                userManagementBroker: this.userManagementBrokerMock.Object,
                signInManagementBroker: this.signInManagerBrokerMock.Object,
                roleManager: this.roleManagerBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object,
                configuration: this.configuration,
                emailService: this.emailService
              );
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

        private static RegisterUserApiRequest CreateRegisterUserApiRequest(DateTimeOffset date) =>
           CreateRegisterUserApiRequestFiller(date).Create();

        private static LoginCredentialsApiRequest CreateLoginCredentialsApiRequest() =>
           CreateLoginCredentialsApiRequestFiller().Create();

        private static ResetPasswordApiRequest CreateResetPasswordApiRequest() =>
           CreateResetPasswordApiRequestFiller().Create();

        private static RegisterResultApiResponse CreateSendAuthDetailResponse() =>
          CreateRegisterResultApiResponseFiller().Create();

        private static UserProfileDetailsApiResponse CreateUserProfileDetailsApiResponse() =>
          CreateUserProfileDetailsApiResponseFiller().Create();

        private static ResetPasswordApiResponse CreateResetPasswordApiResponse() =>
            CreateResetPasswordApiResponseFiller().Create();

        private static ForgotPasswordApiResponse CreateForgotPasswordApiResponse() =>
            CreateForgotPasswordApiResponseFiller().Create();

        private static Enable2FAApiResponse CreateEnable2FAApiResponse() =>
            CreateEnable2FAApiResponseFiller().Create();

        private static SignInResult CreateSignInResult() =>
            CreateSignInResultResponseFiller().Create();

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static string GetRandomNames() => new RealNames().GetValue();

        private static string GetRandomEmailAddresses() => new EmailAddresses().GetValue();

        private static int GetRandomNumber() => new IntRange(min: 2, max: 90).GetValue();

        private static int GetNegativeRandomNumber() => -1 * GetRandomNumber();

        private static string GetRandomString() => new MnemonicString(1, 8, 20).GetValue();

        private static Guid GetRandomGuid() => new Guid();

        private static string GetRandomSubject() => new MnemonicString().GetValue();

        private static List<string> CreateRandomStringList() => new Filler<List<string>>().Create();

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

        private static Filler<RegisterUserApiRequest> CreateRegisterUserApiRequestFiller(DateTimeOffset date)
        {
            var filler = new Filler<RegisterUserApiRequest>();

            filler.Setup()
                .OnType<object>().IgnoreIt()
                .OnProperty(x => x.Email).Use(new EmailAddresses())
                .OnType<DateTimeOffset>().Use(date);

            return filler;
        }

        private static Filler<LoginCredentialsApiRequest> CreateLoginCredentialsApiRequestFiller()
        {
            var filler = new Filler<LoginCredentialsApiRequest>();

            filler.Setup()
                .OnType<object>().IgnoreIt()
                .OnType<DateTimeOffset>().IgnoreIt();

            return filler;
        }

        private static Filler<ResetPasswordApiRequest> CreateResetPasswordApiRequestFiller()
        {
            var filler = new Filler<ResetPasswordApiRequest>();

            filler.Setup()
                .OnType<object>().IgnoreIt()
                .OnProperty(x => x.Email).Use(new EmailAddresses())
                .OnType<DateTimeOffset>().IgnoreIt();

            return filler;
        }

        private static Filler<UserProfileDetailsApiResponse> CreateUserProfileDetailsApiResponseFiller()
        {
            var filler = new Filler<UserProfileDetailsApiResponse>();

            filler.Setup()
                .OnType<object>().IgnoreIt()
                .OnType<DateTimeOffset>().IgnoreIt();

            return filler;
        }

        private static Filler<RegisterResultApiResponse> CreateRegisterResultApiResponseFiller()
        {
            var filler = new Filler<RegisterResultApiResponse>();

            filler.Setup()
                .OnType<object>().IgnoreIt()
                .OnType<DateTimeOffset>().IgnoreIt();

            return filler;
        }

        private static Filler<ForgotPasswordApiResponse> CreateForgotPasswordApiResponseFiller()
        {
            var filler = new Filler<ForgotPasswordApiResponse>();

            filler.Setup()
                .OnType<object>().IgnoreIt()
                .OnType<DateTimeOffset>().IgnoreIt();

            return filler;
        }

        private static Filler<ResetPasswordApiResponse> CreateResetPasswordApiResponseFiller()
        {
            var filler = new Filler<ResetPasswordApiResponse>();

            filler.Setup()
                .OnType<object>().IgnoreIt()
                .OnProperty(x => x.Email).Use(new EmailAddresses())
                .OnType<DateTimeOffset>().IgnoreIt();

            return filler;
        }

        private static Filler<Enable2FAApiResponse> CreateEnable2FAApiResponseFiller()
        {
            var filler = new Filler<Enable2FAApiResponse>();

            filler.Setup()
                .OnType<object>().IgnoreIt()
                .OnType<DateTimeOffset>().IgnoreIt();

            return filler;
        }

        private static Filler<Microsoft.AspNetCore.Identity.SignInResult> CreateSignInResultResponseFiller()
        {
            var filler = new Filler<Microsoft.AspNetCore.Identity.SignInResult>();

            filler.Setup()
                .OnType<bool>().Use(true)
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

        private static ApplicationRole CreateRandomRole()
        {
            var role = new ApplicationRole
            {
                Id = Guid.NewGuid(),
                Name = GetRandomNames(),
                CreatedDate = DateTimeOffset.UtcNow,
                UpdatedDate = DateTimeOffset.UtcNow

            };

            return role;
        }

        private static IQueryable<ApplicationRole> CreateRandomRoles(DateTimeOffset dates, List<string> role)
        {
            var roles = new List<ApplicationRole>();
            foreach (var rol in role)
            {
                roles.Add(new ApplicationRole
                {
                    Id = Guid.NewGuid(),
                    Name = rol,
                    CreatedDate = dates,
                    UpdatedDate = dates
                });
            }

            return roles.AsQueryable();
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
