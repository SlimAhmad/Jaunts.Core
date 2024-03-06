// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Processings.Wallet.Exceptions
{
    public class FailedWalletProcessingServiceException : Xeption
    {
        public FailedWalletProcessingServiceException(Exception innerException)
            : base(message: "Failed Wallet service occurred, Please contact support", innerException)
        { }

        public FailedWalletProcessingServiceException(string message,Exception innerException)
            : base(message, innerException)
        { }
    }
}
