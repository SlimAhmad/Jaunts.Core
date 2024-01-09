﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.VacationPackages.Exceptions
{
    public class FailedVacationPackageServiceException : Xeption
    {
        public FailedVacationPackageServiceException(Exception innerException)
            : base(message: "Failed VacationPackage service error occurred, contact support",
                  innerException)
        { }
        public FailedVacationPackageServiceException(string message,Exception innerException)
          : base(message,
                innerException)
        { }
    }
}