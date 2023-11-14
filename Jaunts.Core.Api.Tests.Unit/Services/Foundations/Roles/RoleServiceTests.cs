// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Brokers.DateTimes;
using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Brokers.RoleManagement;
using Jaunts.Core.Api.Models.Services.Foundations.Role;
using Jaunts.Core.Api.Services.Foundations.Role;
using KellermanSoftware.CompareNetObjects;
using Microsoft.Data.SqlClient;
using Moq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Tynamix.ObjectFiller;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Roles
{
    public partial class RoleServiceTests
    {
        private readonly Mock<IRoleManagementBroker> roleManagementBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IRoleService roleService;
        private readonly ICompareLogic compareLogic;

        public RoleServiceTests()
        {
            this.roleManagementBrokerMock = new Mock<IRoleManagementBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.compareLogic = new CompareLogic();

            this.roleService = new RoleService(
                roleManagementBroker: this.roleManagementBrokerMock.Object,
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

        private static string GetRandomNames() => new RealNames().GetValue();
        private static string GetRandomEmailAddresses() => new EmailAddresses().GetValue();
        private static int GetRandomNumber() => new IntRange(min: 2, max: 90).GetValue();
        private static int GetNegativeRandomNumber() => -1 * GetRandomNumber();
        private static string GetRandomMessage() => new MnemonicString().GetValue();
        private static SqlException GetSqlException() =>
            (SqlException)RuntimeHelpers.GetUninitializedObject(typeof(SqlException));

        private static ApplicationRole CreateRandomRole()
        {
            var role = new ApplicationRole
            {
                Id = Guid.NewGuid(),
                Name = GetRandomMessage(),
                CreatedDate = DateTimeOffset.UtcNow,
                UpdatedDate = DateTimeOffset.UtcNow

            };

            return role;
        }
        private static ApplicationRole CreateRandomRole(DateTimeOffset dates)
        {
            var role = new ApplicationRole
            {
                Id = Guid.NewGuid(),
                Name = GetRandomMessage(),
                CreatedDate = dates,
                UpdatedDate = dates
            };

            return role;
        }



        private static IQueryable<ApplicationRole> CreateRandomRoles(DateTimeOffset dates)
        {
            var roles = new List<ApplicationRole>();
            for (int i = 0; i < GetRandomNumber(); i++)
            {
                var role = new ApplicationRole
                {
                    Id = Guid.NewGuid(),
                    Name = GetRandomMessage(),
                    CreatedDate = dates,
                    UpdatedDate = dates
                };

                roles.Add(role);
            }

            return roles.AsQueryable();
        }

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
    }
}
