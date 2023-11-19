﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Processings.Role.Exceptions
{
    public class RoleProcessingServiceException : Xeption
    {
        public RoleProcessingServiceException(Exception innerException)
            : base(message: "Failed external Role service occurred, please contact support", innerException)
        { }
    }
}
