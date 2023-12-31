﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Role;
using Moq;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Roles
{

    public partial class RoleServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveRoleById()
        {
            //given
            DateTimeOffset dateTime = GetRandomDateTime();
            ApplicationRole randomRole = CreateRandomRole(dates: dateTime);
            Guid inputRoleId = randomRole.Id;
            ApplicationRole inputRole = randomRole;
            ApplicationRole expectedRole = randomRole;

            this.roleManagementBrokerMock.Setup(broker =>
                    broker.SelectRoleByIdAsync(inputRoleId))
                .ReturnsAsync(inputRole);

            //when 
            ApplicationRole actualRole = await this.roleService.RetrieveRoleByIdRequestAsync(inputRoleId);

            //then
            actualRole.Should().BeEquivalentTo(expectedRole);

            this.roleManagementBrokerMock.Verify(broker =>
                broker.SelectRoleByIdAsync(inputRoleId), Times.Once);

            this.roleManagementBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

    }
}
