// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Orchestration.Jwt.Exceptions
{
    public class JwtOrchestrationServiceException : Xeption
    {
        public JwtOrchestrationServiceException(Xeption innerException)
            : base(message: "Jwt service error occurred, please fix the errors and try again.",
                  innerException)
        { }
    }
}
