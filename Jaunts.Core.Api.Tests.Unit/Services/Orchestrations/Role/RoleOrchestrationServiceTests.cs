// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Models.Role.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Role;
using Jaunts.Core.Api.Services.Orchestration.Role;
using Jaunts.Core.Api.Services.Processings.Role;
using Jaunts.Core.Api.Services.Processings.User;
using Jaunts.Core.Authorization;
using KellermanSoftware.CompareNetObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Orchestrations.Role
{
    public partial class RoleOrchestrationServiceTests
    {
        private readonly Mock<IUserProcessingService> userProcessingServiceMock;
        private readonly Mock<IRoleProcessingService> roleProcessingServiceMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IRoleOrchestrationService roleOrchestrationService;
        private readonly ICompareLogic compareLogic;

        public RoleOrchestrationServiceTests()
        {
            this.userProcessingServiceMock = new Mock<IUserProcessingService>();
            this.roleProcessingServiceMock = new Mock<IRoleProcessingService>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.compareLogic = new CompareLogic();

            this.roleOrchestrationService = new RoleOrchestrationService(
                roleProcessingService: this.roleProcessingServiceMock.Object,
                userProcessingService: this.userProcessingServiceMock.Object,
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
        private static DateTimeOffset GetCurrentDateTime() =>
            DateTimeOffset.UtcNow;
        private static Guid GetRandomGuid() => Guid.NewGuid(); 
        private static int GetRandomNumber() => new IntRange(min: 2, max: 90).GetValue();
        private static List<string> CreateRandomStringList() =>
         new Filler<List<string>>().Create();
        private static IQueryable<ApplicationRole> CreateRandomRoles() =>
            CreateRoleFiller().Create(count: GetRandomNumber()).AsQueryable();
        private static IQueryable<ApplicationRole> CreateRandomRoles(List<string> role) =>
            CreateRoleFiller(role).Create(count: GetRandomNumber()).AsQueryable();
        private static ApplicationRole CreateRandomRole() =>
            CreateRoleFiller().Create();

        private static IQueryable<ApplicationRole> CreateRandomRoles(ApplicationRole roles)
        {
            List<ApplicationRole> randomRoles =
                CreateRandomRoles().ToList();

            randomRoles.Add(roles);

            return randomRoles.AsQueryable();
        }

        private static Filler<ApplicationRole> CreateRoleFiller()
        {
            var filler = new Filler<ApplicationRole>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(GetCurrentDateTime());

            return filler;
        }

        private static Filler<ApplicationRole> CreateRoleFiller(List<string> role)
        {
            var filler = new Filler<ApplicationRole>();

            filler.Setup()
                .OnProperty(x => x.Name).Use(role)
                .OnProperty(x=> x.Permissions).Use(Permissions.None)
                .OnType<DateTimeOffset>().Use(GetCurrentDateTime());

            return filler;
        }

        public static TheoryData DependencyValidationExceptions()
        {
            var innerException = new Xeption();

            return new TheoryData<Xeption>
            {
                new RoleValidationException(innerException),
                new RoleDependencyValidationException(innerException)
            };
        }

        public static TheoryData DependencyExceptions()
        {
            var innerException = new Xeption();

            return new TheoryData<Xeption>
            {
                new RoleDependencyException(innerException),
                new RoleServiceException(innerException)
            };
        }
    }
}
