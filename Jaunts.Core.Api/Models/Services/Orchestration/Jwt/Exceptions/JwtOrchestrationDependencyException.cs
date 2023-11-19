﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Orchestration.Jwt.Exceptions
{
    public class JwtOrchestrationDependencyException : Xeption
    {
        public JwtOrchestrationDependencyException(Xeption innerException)
             : base(message: "Jwt dependency error occurred, please fix the errors and try again.",
                  innerException)
        { }
    }
}
