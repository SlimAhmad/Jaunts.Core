// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Models.Auth;
using Jaunts.Core.Api.Models.Services.Foundations.Role;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Api.Services.Aggregations.Account;
using Jaunts.Core.Api.Services.Orchestration.Email;
using Jaunts.Core.Api.Services.Orchestration.Jwt;
using Jaunts.Core.Api.Services.Orchestration.SignIn;
using Jaunts.Core.Api.Services.Orchestration.User;
using Jaunts.Core.Models.Auth.LoginRegister;
using Jaunts.Core.Models.Email;
using KellermanSoftware.CompareNetObjects;
using Microsoft.Data.SqlClient;
using Moq;
using RESTFulSense.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Tynamix.ObjectFiller;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Aggregation.Account
{
    public partial class AccountAggregationServiceTests
    {
        private readonly Mock<IUserOrchestrationService> userOrchestrationMock;
        private readonly Mock<ISignInOrchestrationService> signInOrchestrationMock;
        private readonly Mock<IEmailOrchestrationService> emailOrchestrationMock;
        private readonly Mock<IJwtOrchestrationService> jwtOrchestrationMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IAccountAggregationService accountAggregationService;
        private readonly ICompareLogic compareLogic;

        public AccountAggregationServiceTests()
        {
            this.userOrchestrationMock = new Mock<IUserOrchestrationService>();
            this.signInOrchestrationMock = new Mock<ISignInOrchestrationService>();
            this.emailOrchestrationMock = new Mock<IEmailOrchestrationService>();
            this.jwtOrchestrationMock = new Mock<IJwtOrchestrationService>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.compareLogic = new CompareLogic();
            

            this.accountAggregationService = new AccountAggregationService(
                userOrchestrationService : this.userOrchestrationMock.Object,
                signInOrchestrationService : this.signInOrchestrationMock.Object,
                emailOrchestrationService: this.emailOrchestrationMock.Object,
                jwtOrchestrationService: this.jwtOrchestrationMock.Object,
                loggingBroker: this.loggingBrokerMock.Object
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

        private static UserAccountDetailsResponse CreateRegisterUserResponse(RegisterUserApiRequest user) =>
          CreateRegisterApiResponseFiller(user).Create();

        private static UserAccountDetailsResponse CreateUserAccountDetailsApiResponse(ApplicationUser user) =>
          CreateUserProfileDetailsApiResponseFiller(user).Create();

        private static ResetPasswordApiResponse CreateResetPasswordApiResponse() =>
            CreateResetPasswordApiResponseFiller().Create();

        private static ForgotPasswordApiResponse CreateForgotPasswordApiResponse() =>
            CreateForgotPasswordApiResponseFiller().Create();
        private static SendEmailResponse CreateSendEmailResponse() =>
            CreateSendEmailResponseFiller().Create();

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();
        private static bool GetRandomBoolean() =>
             Randomizer<bool>.Create();

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

        private static Filler<RegisterUserApiRequest> CreateRegisterUserApiRequestFiller(
            DateTimeOffset date)
        {
            var filler = new Filler<RegisterUserApiRequest>();

            filler.Setup()
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

        private static Filler<UserAccountDetailsResponse> CreateUserProfileDetailsApiResponseFiller(
            ApplicationUser user
            )
        {
            var filler = new Filler<UserAccountDetailsResponse>();

            filler.Setup()
                .OnProperty(x=> x.Username).Use(user.UserName)
                .OnProperty(x => x.Email).Use(user.Email)
                .OnProperty(x => x.FirstName).Use(user.FirstName)
                .OnProperty(x => x.LastName).Use(user.LastName)
                .OnProperty(x => x.Id).Use(user.Id.ToString())
                .OnProperty(x => x.TwoFactorEnabled).Use(user.TwoFactorEnabled)
                .OnType<DateTimeOffset>().IgnoreIt();

            return filler;
        }

        private static Filler<SendEmailResponse> CreateSendEmailResponseFiller()
        {
            var filler = new Filler<SendEmailResponse>();

            filler.Setup()
                .OnProperty(x=> x.Errors).IgnoreIt()
                .OnType<DateTimeOffset>().IgnoreIt();

            return filler;
        }

        private static Filler<UserAccountDetailsResponse> CreateRegisterApiResponseFiller(RegisterUserApiRequest registerUserApiRequest)
        {
            var filler = new Filler<UserAccountDetailsResponse>();

            filler.Setup()
                .OnProperty(x => x.Username).Use(registerUserApiRequest.Username)
                .OnProperty(x => x.Email).Use(registerUserApiRequest.Email)
                .OnProperty(x => x.FirstName).Use(registerUserApiRequest.FirstName)
                .OnProperty(x => x.LastName).Use(registerUserApiRequest.LastName)
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


        private ApplicationUser ConvertToUserRequest(RegisterUserApiRequest registerUserApiRequest)
        {
            return new ApplicationUser
            {
                UserName = registerUserApiRequest.Username,
                FirstName = registerUserApiRequest.FirstName,
                LastName = registerUserApiRequest.LastName,
                Email = registerUserApiRequest.Email,
                PhoneNumber = registerUserApiRequest.PhoneNumber,
                CreatedDate = registerUserApiRequest.CreatedDate,
                UpdatedDate = registerUserApiRequest.UpdatedDate,
            };
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
